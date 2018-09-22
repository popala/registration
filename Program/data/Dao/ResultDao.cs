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
using System.Data.SQLite;
using System.Linq;

namespace Rejestracja.Data.Dao {
    class ResultDao {
        public static long addCategoryResult(long entryId, int place) {
            return add(entryId, (int?)null, place);
        }

        public static long addAwardWinner(long entryId, long awardId) {
            return add(entryId, awardId, (int?)null);
        }

        private static long add(long entryId, long? awardId, int? place) {
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

                return cn.LastInsertRowId;
            }
        }

        public static Result get(int resultId) {

            Result ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT e.EntryId, e.TmStamp, e.Email, e.FirstName, e.LastName, e.ClubName, e.AgeGroup, 
                        e.ModelName, e.ModelCategory, COALESCE(e.ModelCategoryId, -1) AS ModelCategoryId,
                        e.ModelScale, e.ModelPublisher, e.ModelClass, e.YearOfBirth, COALESCE(e.SkipErrorValidation, 0) AS SkipErrorValidation,
                        r.ResultId, r.Place, a.Id AS AwardId, a.Title, a.DisplayOrder
                    FROM Results r
                        JOIN Registration e ON r.EntryId = e.EntryId
                        LEFT JOIN SpecialAwards a ON r.AwardId = a.Id
                    WHERE r.ResultId = @ResultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@ResultId", System.Data.DbType.Int32).Value = resultId;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    if (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            dr.GetInt64(dr.GetOrdinal("EntryId")),
                            (DateTime)dr["TmStamp"],
                            dr["Email"].ToString(),
                            dr["FirstName"].ToString(),
                            dr["LastName"].ToString(),
                            dr["ClubName"].ToString(),
                            dr["AgeGroup"].ToString(),
                            dr["ModelName"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["ModelScale"].ToString(),
                            dr["ModelPublisher"].ToString(),
                            dr["ModelCategory"].ToString(),
                            dr.GetInt64(dr.GetOrdinal("ModelCategoryId")),
                            dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                            dr.GetBoolean(dr.GetOrdinal("SkipErrorValidation"))
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

        public static void delete(int resultId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE ResultId = @resultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@resultId", System.Data.DbType.Int32).Value = resultId;
                cm.ExecuteNonQuery();
            }
        }

        public static void deleteAwardResult(long entryId, long awardId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId = @EntryId AND AwardId = @AwardId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@EntryId", System.Data.DbType.Int64).Value = entryId;
                cm.Parameters.Add("@AwardId", System.Data.DbType.Int64).Value = awardId;
                cm.ExecuteNonQuery();
            }
        }

        public static void deleteCategoryResult(long entryId, int place) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId = @EntryId AND Place = @Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@EntryId", System.Data.DbType.Int64).Value = entryId;
                cm.Parameters.Add("@Place", System.Data.DbType.Int32).Value = place;
                cm.ExecuteNonQuery();
            }
        }

        public static bool awardResultExists(long entryId, long awardId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT ResultId FROM Results WHERE EntryId = @EntryId AND AwardId = @AwardId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@EntryId", System.Data.DbType.Int64).Value = entryId;
                cm.Parameters.Add("@AwardId", System.Data.DbType.Int64).Value = awardId;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static IEnumerable<String[]> getCategoryResultList() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.ResultId, e.EntryId, e.AgeGroup, e.ModelClass, e.ModelCategory, e.ModelName, r.Place, a.Age,
                    CASE WHEN mc.DisplayOrder IS NULL THEN -1 ELSE mc.DisplayOrder END AS DisplayOrder
                FROM Results r 
	                JOIN Registration e ON r.EntryId = e.EntryId
	                JOIN AgeGroup a ON e.AgeGroup = a.Name
                    LEFT JOIN ModelCategory mc ON mc.Id = e.ModelCategoryId
                WHERE r.AwardId IS NULL
                ORDER BY a.Age, e.ModelClass, DisplayOrder, e.ModelCategory, r.Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new String[] 
                            {
                                dr["ResultId"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["EntryId"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["Place"].ToString()
                            };
                    }
                }
            }
        }

        public static IEnumerable<RegistrationEntry> getCategoryResults(String modelCategory) {
            if (String.IsNullOrWhiteSpace(modelCategory)) {
                throw new ArgumentNullException("Kategoria jest wymagana");
            }

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.EntryId, r.AgeGroup, r.ModelName, r.ModelScale, r.ModelClass, r.ModelPublisher, COALESCE(s.Place,0) AS Place 
                FROM Registration r
                    LEFT JOIN Results s ON r.EntryId = s.EntryId AND s.AwardId IS NULL
                WHERE r.ModelCategory = @modelCategory
                ORDER BY r.AgeGroup, r.ModelClass, r.EntryId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = modelCategory;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new RegistrationEntry(
                                dr.GetInt64(dr.GetOrdinal("EntryId")),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("Place"))
                            );
                    }
                }
            }
        }

        public static IEnumerable<Result> getCategoryResults() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT e.EntryId, e.TmStamp, e.Email, e.FirstName, e.LastName, e.ClubName, e.AgeGroup, 
                        e.ModelName, e.ModelCategory, COALESCE(e.ModelCategoryId, -1) AS ModelCategoryId,
                        e.ModelScale, e.ModelPublisher, e.ModelClass, e.YearOfBirth, COALESCE(e.SkipErrorValidation, 0) AS SkipErrorValidation,
                        r.ResultId, r.Place, ag.Age,
                    CASE WHEN mc.DisplayOrder IS NULL THEN -1 ELSE mc.DisplayOrder END AS DisplayOrder
                FROM Results r 
                    JOIN Registration e ON r.EntryId = e.EntryId
                    JOIN AgeGroup ag ON e.AgeGroup = ag.Name
                    LEFT JOIN ModelCategory mc ON mc.Id = e.ModelCategoryId
                WHERE r.Place IS NOT NULL
                    ORDER BY ag.Age ASC, e.ModelClass, mc.DisplayOrder, e.ModelCategory, r.Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            dr.GetInt64(dr.GetOrdinal("EntryId")),
                            (DateTime)dr["TmStamp"],
                            dr["Email"].ToString(),
                            dr["FirstName"].ToString(),
                            dr["LastName"].ToString(),
                            dr["ClubName"].ToString(),
                            dr["AgeGroup"].ToString(),
                            dr["ModelName"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["ModelScale"].ToString(),
                            dr["ModelPublisher"].ToString(),
                            dr["ModelCategory"].ToString(),
                            dr.GetInt64(dr.GetOrdinal("ModelCategoryId")),
                            dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                            dr.GetBoolean(dr.GetOrdinal("SkipErrorValidation"))
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

        public static IEnumerable<String[]> getAwardResultList() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.ResultId, a.Title, e.EntryId, e.ModelName
                    FROM Results r 
                    JOIN Registration e ON r.EntryId = e.EntryId
                    JOIN SpecialAwards a ON r.AwardId = a.Id
                WHERE r.AwardId IS NOT NULL
                    ORDER BY a.Id", cn)) {
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
                                dr["EntryId"].ToString(),
                                dr["ModelName"].ToString(),
                                ""
                            };
                    }
                }
            }
        }

        public static IEnumerable<Result> getAwardResults() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT e.EntryId, e.TmStamp, e.Email, e.FirstName, e.LastName, e.ClubName, e.AgeGroup, 
                        e.ModelName, e.ModelCategory, COALESCE(e.ModelCategoryId, -1) AS ModelCategoryId,
                        e.ModelScale, e.ModelPublisher, e.ModelClass, e.YearOfBirth, COALESCE(e.SkipErrorValidation, 0) AS SkipErrorValidation,
                        r.ResultId, a.Id AS AwardId, a.Title, a.DisplayOrder
                    FROM Results r
                        JOIN Registration e ON r.EntryId = e.EntryId
                        JOIN SpecialAwards a ON r.AwardId = a.Id
                    WHERE r.AwardId IS NOT NULL
                        ORDER BY a.DisplayOrder, a.Id, r.ResultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {

                        RegistrationEntry entry = new RegistrationEntry(
                            dr.GetInt64(dr.GetOrdinal("EntryId")),
                            (DateTime)dr["TmStamp"],
                            dr["Email"].ToString(),
                            dr["FirstName"].ToString(),
                            dr["LastName"].ToString(),
                            dr["ClubName"].ToString(),
                            dr["AgeGroup"].ToString(),
                            dr["ModelName"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["ModelScale"].ToString(),
                            dr["ModelPublisher"].ToString(),
                            dr["ModelCategory"].ToString(),
                            dr.GetInt64(dr.GetOrdinal("ModelCategoryId")),
                            dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                            dr.GetBoolean(dr.GetOrdinal("SkipErrorValidation"))
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

        public static List<KeyValuePair<string, string>> getMedalCount() {

            List<AgeGroup> ageGroups = AgeGroupDao.getList().ToList();
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"SELECT AgeGroup, Place, COUNT(ukey) AS MedalCount
                    FROM (
	                    SELECT r.AgeGroup, res.Place, (r.AgeGroup || res.Place) AS ukey
		                    FROM Results res
		                    JOIN Registration r ON res.EntryId = r.EntryId
	                    WHERE res.Place IS NOT NULL
                    )
                    GROUP BY ukey", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                String ageGroup = null;
                String summary = "";

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read()) {

                        if(ageGroup == null) {
                            ageGroup = dr["AgeGroup"].ToString();
                        }

                        if(!ageGroup.Equals(dr["AgeGroup"].ToString(), StringComparison.CurrentCultureIgnoreCase)) {
                            ret.Add(new KeyValuePair<string, string>(ageGroup, summary.Substring(0, summary.Length - 1)));
                            summary = "";
                            ageGroup = dr["AgeGroup"].ToString();
                        }
                        summary += string.Format(" Miejsce {0}: {1},", dr["Place"].ToString(), dr["MedalCount"].ToString());
                    }
                    if(ageGroup != null) {
                        ret.Add(new KeyValuePair<string, string>(ageGroup, summary.Substring(0, summary.Length - 1)));
                    }
                }
            }
            return ret;
        }

        public static void createTable() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText =
                    @"CREATE TABLE Results(
                        ResultId INTEGER PRIMARY KEY,
                        EntryId INTEGER NOT NULL REFERENCES Registration(EntryId),
                        AwardId INTEGER NULL REFERENCES SpecialAwards(AwardId),
                        Place INTEGER NULL)";
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_Res_Unique ON Results(EntryId,AwardId,Place)";
                cm.ExecuteNonQuery();
            }
        }
    }
}
