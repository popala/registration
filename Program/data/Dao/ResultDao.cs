/*
 * Copyright (C) 2016 Paweł Opała https://github.com/popala/registration
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program.  If not, see http://www.gnu.org/licenses/.
 */
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Rejestracja.Data.Dao {
    class ResultDao {
        public int addCategoryResult(int entryId, int place) {
            return add(entryId, (int?)null, place);
        }

        public int addAwardWinner(int entryId, int awardId) {
            return add(entryId, awardId, (int?)null);
        }

        private int add(int entryId, int? awardId, int? place) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("INSERT INTO Results(EntryId,AwardId,Place) VALUES(@entryId, @awardId, @place)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@entryId", System.Data.DbType.Int32).Value = entryId;
                if (awardId.HasValue) {
                    cm.Parameters.Add("@awardId", System.Data.DbType.Int32).Value = awardId;
                }
                else {
                    cm.Parameters.Add("@awardId", System.Data.DbType.Int32).Value = DBNull.Value;
                }

                if (place.HasValue) {
                    cm.Parameters.Add("@place", System.Data.DbType.Int32).Value = place;
                }
                else {
                    cm.Parameters.Add("@place", System.Data.DbType.Int32).Value = DBNull.Value;
                }
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public Result get(int resultId) {

            Result ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                    @"SELECT 
	                    r.Id AS RegistrationId, r.TmStamp, r.AgeGroupName,
	                    r.CategoryId, c.Code, COALESCE(c.Name, r.CategoryName) AS CategoryName, c.ModelClass, CASE WHEN c.DisplayOrder IS NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder,
	                    mr.Id AS ModelerId, mr.FirstName, mr.LastName, mr.ClubName, mr.YearOfBirth, mr.Email,
	                    ml.Id AS ModelId, ml.Name AS ModelName, ml.Publisher, ml.Scale,
	                    res.ResultId, res.Place, res.AwardId, a.Title, a.DisplayOrder
                    FROM Registration r
	                    JOIN Models ml ON r.ModelId = ml.Id
	                    JOIN Modelers mr ON ml.ModelerId = mr.Id
	                    JOIN Categories c ON r.CategoryId = c.Id
	                    JOIN Results res ON res.RegistrationId = r.Id
	                    LEFT JOIN Awards a ON res.AwardId = a.Id
                    WHERE ResultId =  @ResultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@ResultId", System.Data.DbType.Int32).Value = resultId;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    if (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            new Registration(
                                dr.GetInt32(dr.GetOrdinal("RegistrationId")),
                                (DateTime)dr["TmStamp"],
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["CategoryName"].ToString(),
                                dr["AgeGroupName"].ToString()
                            ),
                            new Category(
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["Code"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            ),
                            new Modeler(
                                dr.GetInt32(dr.GetOrdinal("ModelerId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["ClubName"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                                dr["Email"].ToString()
                            ),
                            new Model(
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr["ModelName"].ToString(),
                                dr["Publisher"].ToString(),
                                dr["Scale"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("ModelerId"))
                            )
                        );

                        if (!dr.IsDBNull(dr.GetOrdinal("Place"))) {
                            ret = new Result(dr.GetInt32(dr.GetOrdinal("ResultId")), entry, dr.GetInt32(dr.GetOrdinal("Place")));
                        }
                        else {
                            Award award = new Award(
                                dr.GetInt32(dr.GetOrdinal("AwardId")),
                                dr["Title"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            );
                            ret = new Result(dr.GetInt32(dr.GetOrdinal("ResultId")), entry, award);
                        }
                    }
                }
            }

            return ret;
        }

        public void delete(int resultId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE ResultId = @resultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@resultId", System.Data.DbType.Int32).Value = resultId;
                cm.ExecuteNonQuery();
            }
        }

        public void deleteAwardResult(int entryId, int awardId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId = @EntryId AND AwardId = @AwardId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;
                cm.Parameters.Add("@AwardId", System.Data.DbType.Int32).Value = awardId;
                cm.ExecuteNonQuery();
            }
        }

        public void deleteCategoryResult(int entryId, int place) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId = @EntryId AND Place = @Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;
                cm.Parameters.Add("@Place", System.Data.DbType.Int32).Value = place;
                cm.ExecuteNonQuery();
            }
        }

        public bool awardResultExists(int entryId, int awardId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT ResultId FROM Results WHERE EntryId = @EntryId AND AwardId = @AwardId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;
                cm.Parameters.Add("@AwardId", System.Data.DbType.Int32).Value = awardId;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public IEnumerable<String[]> getCategoryResultList() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT res.ResultId, r.Id AS RegistrationId, r.AgeGroupName, c.ModelClass, c.Name AS CategoryName, m.Name AS ModelName, res.Place, a.Age,
                    CASE WHEN c.DisplayOrder IS NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder
                FROM Results res 
	                JOIN Registration r ON res.RegistrationId = r.Id
                    JOIN Models m ON r.ModelId = m.Id
	                JOIN AgeGroups a ON r.AgeGroupName = a.Name
                    LEFT JOIN Categories c ON c.Id = r.CategoryId
                WHERE res.AwardId IS NULL
                ORDER BY Age, DisplayOrder, CategoryName, ModelClass, Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new String[] 
                            {
                                dr["ResultId"].ToString(),
                                dr["AgeGroupName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["RegistrationId"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["Place"].ToString()
                            };
                    }
                }
            }
        }

        public IEnumerable<Result> getCategoryResults() {
            return getCategoryResults(-1);
        }

        public IEnumerable<Result> getCategoryResults(int categoryId) {

            String query = 
                @"SELECT 
	                r.Id AS RegistrationId, r.TmStamp, r.AgeGroupName,
	                r.CategoryId, c.Code, COALESCE(c.Name, r.CategoryName) AS CategoryName, c.ModelClass, CASE WHEN c.DisplayOrder IS NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder,
	                mr.Id AS ModelerId, mr.FirstName, mr.LastName, mr.ClubName, mr.YearOfBirth, mr.Email,
	                ml.Id AS ModelId, ml.Name AS ModelName, ml.Publisher, ml.Scale,
	                res.ResultId, res.Place, ag.Age
                FROM Registration r
	                JOIN Models ml ON r.ModelId = ml.Id
	                JOIN Modelers mr ON ml.ModelerId = mr.Id
	                JOIN Categories c ON r.CategoryId = c.Id
	                JOIN Results res ON res.RegistrationId = r.Id
	                JOIN AgeGroups ag ON ag.Name = r.AgeGroupName
                WHERE res.Place IS NOT NULL";

            if(categoryId > -1) {
                query += " AND r.CategoryId = @CategoryId ";
            }
            query += " ORDER BY ag.Age ASC, DisplayOrder, CategoryName, ModelClass, Place";

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                if(categoryId > -1) {
                    cm.Parameters.Add("@CategoryId", DbType.Int32).Value = categoryId;
                }

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            new Registration(
                                dr.GetInt32(dr.GetOrdinal("RegistrationId")),
                                (DateTime)dr["TmStamp"],
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["CategoryName"].ToString(),
                                dr["AgeGroupName"].ToString()
                            ),
                            new Category(
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["Code"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            ),
                            new Modeler(
                                dr.GetInt32(dr.GetOrdinal("ModelerId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["ClubName"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                                dr["Email"].ToString()
                            ),
                            new Model(
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr["ModelName"].ToString(),
                                dr["Publisher"].ToString(),
                                dr["Scale"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("ModelerId"))
                            )
                        );
                        
                        yield return
                            new Result(
                                dr.GetInt32(dr.GetOrdinal("ResultId")),
                                entry,
                                dr.GetInt32(dr.GetOrdinal("Place"))
                            );
                    }
                }
            }
        }

        public IEnumerable<String[]> getAwardResultList() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT res.ResultId, a.Title, a.DisplayOrder, res.AwardId, r.Id AS RegistrationId, m.Name AS ModelName
                    FROM Results res
                    JOIN Registration r ON res.RegistrationId = r.Id
                    JOIN Models m ON r.ModelId = m.Id
                    JOIN Awards a ON res.AwardId = a.Id
                WHERE res.AwardId IS NOT NULL
                    ORDER BY DisplayOrder, AwardId, RegistrationId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new String[] 
                            {
                                dr["ResultId"].ToString(),
                                "",
                                "",
                                dr["Title"].ToString(),
                                dr["RegistrationId"].ToString(),
                                dr["ModelName"].ToString(),
                                ""
                            };
                    }
                }
            }
        }

        public IEnumerable<Result> getAwardResults() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                    @"SELECT 
	                    r.Id AS RegistrationId, r.TmStamp, 
	                    mr.Id AS ModelerId, mr.FirstName, mr.LastName, mr.ClubName, mr.YearOfBirth, mr.Email,
	                    ml.Id AS ModelId, ml.Name AS ModelName, ml.Publisher, ml.Scale,
	                    res.ResultId, res.AwardId, a.Title, a.DisplayOrder
                    FROM Registration r
	                    JOIN Models ml ON r.ModelId = ml.Id
	                    JOIN Modelers mr ON ml.ModelerId = mr.Id
	                    JOIN Results res ON res.RegistrationId = r.Id
	                    JOIN Awards a ON res.AwardId = a.Id
                    WHERE res.AwardId IS NOT NULL
	                    ORDER BY DisplayOrder, a.Id, res.ResultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            new Registration(
                                dr.GetInt32(dr.GetOrdinal("RegistrationId")),
                                (DateTime)dr["TmStamp"],
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["CategoryName"].ToString(),
                                dr["AgeGroupName"].ToString()
                            ),
                            new Category(
                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["Code"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            ),
                            new Modeler(
                                dr.GetInt32(dr.GetOrdinal("ModelerId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["ClubName"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                                dr["Email"].ToString()
                            ),
                            new Model(
                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr["ModelName"].ToString(),
                                dr["Publisher"].ToString(),
                                dr["Scale"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("ModelerId"))
                            )
                        );

                        Award award = new Award(
                            dr.GetInt32(dr.GetOrdinal("AwardId")),
                            dr["Title"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                        );
                        yield return
                            new Result(dr.GetInt32(dr.GetOrdinal("ResultId")), entry, award);
                    }
                }
            }
        }

        public int getDiplomaCount() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT COUNT(ResultId) FROM Results", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                return (int)cm.ExecuteScalar();
            }
        }
    }
}
