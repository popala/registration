﻿/*
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
using System.Text;

namespace Rejestracja.Data.Dao
{
    class RegistrationEntryDao
    {
        private const String BASE_QUERY =
            @"SELECT 
	            r.Id AS RegistrationId, r.TmStamp, r.AgeGroupName,
	            r.CategoryId, c.Code, COALESCE(c.Name, r.CategoryName) AS CategoryName, c.ModelClass, CASE WHEN c.DisplayOrder IS NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder,
	            mr.Id AS ModelerId, mr.FirstName, mr.LastName, mr.ClubName, mr.YearOfBirth, mr.Email,
	            ml.Id AS ModelId, ml.Name AS ModelName, ml.Publisher, ml.Scale
            FROM Registration r
	            JOIN Models ml ON r.ModelId = ml.Id
	            JOIN Modelers mr ON ml.ModelerId = mr.Id
	            LEFT JOIN Categories c ON r.CategoryId = c.Id ";

        public static RegistrationEntry get(int entryId)
        {
            RegistrationEntry ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(BASE_QUERY + " WHERE r.Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = entryId;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ret = new RegistrationEntry(
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
                    }
                }
            }
            return ret;
        }

        public static IEnumerable<String[]> getList(String searchValue, int sortField, bool sortAscending) {

            string sortFieldName;

            switch (sortField) { 
                case 0: sortFieldName = "RegistrationId"; break;
                case 1: sortFieldName = "TmStamp"; break;
                case 2: sortFieldName = "Email"; break;
                case 3: sortFieldName = "FirstName"; break;
                case 4: sortFieldName = "LastName"; break;
                case 5: sortFieldName = "YearOfBirth"; break;
                case 6: sortFieldName = "ClubName"; break;
                case 7: sortFieldName = "AgeGroupName"; break;
                case 8: sortFieldName = "ModelName"; break;
                case 9: sortFieldName = "CategoryName"; break;
                case 10: sortFieldName = "ModelClass"; break;
                case 11: sortFieldName = "Scale"; break;
                case 12: sortFieldName = "Publisher"; break;
                default: sortFieldName = "RegistrationId"; break;
            }
            return getList(searchValue, sortFieldName, sortAscending);
        }

        private static IEnumerable<String[]> getList(String searchValue, String sortField, bool sortAscending)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn))
            {
                StringBuilder query = new StringBuilder(BASE_QUERY);

                if (!String.IsNullOrEmpty(searchValue))
                {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n))
                    {
                        query.Append(" r.Id = @Id AND ");
                        cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = n;
                    }
                    query.Append(" mr.Email LIKE @SearchValue OR mr.FirstName LIKE @SearchValue OR mr.LastName LIKE @SearchValue OR ml.Name LIKE @SearchValue ");
                    cm.Parameters.Add("@SearchValue", System.Data.DbType.String, 64).Value = "%" + searchValue + "%";
                }
                query.Append(" ORDER BY ").Append(sortField).Append(sortAscending ? " ASC" : " DESC");

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = query.ToString();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int yearOfBirth = 0;
                        int.TryParse(dr["YearOfBirth"].ToString(), out yearOfBirth);

                        yield return
                            new String[] 
                            {
                                dr["RegistrationId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroupName"].ToString(),
                                dr["Code"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["Scale"].ToString(),
                                dr["Publisher"].ToString()
                            };
                    }
                }
            }
        }

        public static IEnumerable<String[]> getGrouppedList() {
            return getGrouppedList(null);
        }
        
        public static IEnumerable<String[]> getGrouppedList(String searchValue) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                StringBuilder query = new StringBuilder(BASE_QUERY);

                if (!String.IsNullOrEmpty(searchValue)) {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n)) {
                        query.Append(" r.RegistrationId = @Id AND ");
                        cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = n;
                    }

                    query.Append(" mr.Email LIKE @SearchValue OR mr.FirstName LIKE @SearchValue OR mr.LastName LIKE @SearchValue OR ml.Name LIKE @SearchValue ");
                    cm.Parameters.Add("@SearchValue", System.Data.DbType.String, 64).Value = "%" + searchValue + "%";
                }
                query.Append(" ORDER BY DisplayOrder, CategoryName, AgeGroupName, ModelClass, RegistrationId");

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = query.ToString();

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        int yearOfBirth = 0;
                        int.TryParse(dr["YearOfBirth"].ToString(), out yearOfBirth);

                        yield return
                            new String[] 
                            {
                                dr["RegistrationId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroupName"].ToString(),
                                dr["Code"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["Scale"].ToString(),
                                dr["Publisher"].ToString()
                            };
                    }
                }
            }
        }
        
        public static List<RegistrationEntry> getListForJudging()
        {
            List<RegistrationEntry> ret = new List<RegistrationEntry>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                BASE_QUERY + 
                " WHERE r.CategoryId > -1 " +
                " ORDER BY CategoryName, AgeGroupName, ClassName, RegistrationId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ret.Add(
                            new RegistrationEntry(
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
                        ));
                    }
                    return ret;
                }
            }
        }

        public static IEnumerable<String[]> getListForMergingCategories(int maxEntryCount) {

            String query =
                @"SELECT r.AgeGroupName, c.ModelClass, c.Id AS CategoryId, c.Name AS CategoryName, COUNT(r.Id) AS EntryCount, CASE c.DisplayOrder WHEN NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder
                    FROM Registration r
	                    LEFT JOIN Categories c ON c.Id = r.CategoryId
	                    LEFT JOIN AgeGroups ag ON r.AgeGroupName = ag.Name
	                    JOIN (
		                    SELECT DISTINCT CategoryId
		                    FROM (
			                    SELECT CategoryId, AgeGroupName, COUNT(Id) AS EntryCount 
				                    FROM Registration 
				                    WHERE CategoryId > -1
				                    GROUP BY CategoryId, AgeGroupName
				                    HAVING EntryCount < @MaxCount)
	                    ) sm ON r.CategoryId = sm.CategoryId
                    GROUP BY r.AgeGroupName, c.ModelClass, r.CategoryId
                    ORDER BY c.DisplayOrder, c.Name, ag.Age, c.ModelClass";

            if (maxEntryCount < 1) {
                yield break;
            }
            
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@MaxCount", System.Data.DbType.Int32).Value = maxEntryCount;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return new String[] {
                            dr["AgeGroupName"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["CategoryName"].ToString(),
                            dr["CategoryId"].ToString(),
                            dr["EntryCount"].ToString()
                        };
                    }
                }
            }
        }

        public static void mergeAgeGroupsInCategory(long modelCategoryId, String sourceAgeGroup, String targetAgeGroup) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE RegistrationId IN(SELECT Id FROM Registration WHERE AgeGroup = @SourceAgeGroup AND CategoryId = @CategoryId) AND AwardId IS NULL", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@SourceAgeGroup", System.Data.DbType.String, 64).Value = sourceAgeGroup;
                cm.Parameters.Add("@TargetAgeGroup", System.Data.DbType.String, 64).Value = targetAgeGroup;
                cm.Parameters.Add("@CategoryId", System.Data.DbType.Int64).Value = modelCategoryId;

                cm.ExecuteNonQuery();

                cm.CommandText = "UPDATE Registration SET AgeGroupName = @TargetAgeGroup WHERE AgeGroupName = @SourceAgeGroup AND CategoryId = @CategoryId";
                cm.ExecuteNonQuery();
            }
        }

        public static void changeCategory(int registrationId, int categoryId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Registration SET CategoryName = NULL, CategoryId = @CategoryId WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@CategoryId", System.Data.DbType.Int32).Value = categoryId;
                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = registrationId;

                cm.ExecuteNonQuery();
            }
        }
        
        public static int add(DateTime timeStamp, int modelId, int categoryId, String ageGroupName)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Registration(TmStamp, ModelId, CategoryId, AgeGroupName)
                  VALUES(@TmStamp, @ModelId, @CategoryId, @AgeGroupName)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@TmStamp", System.Data.DbType.DateTime).Value = timeStamp;
                cm.Parameters.Add("@ModelId", System.Data.DbType.Int32).Value = modelId;
                cm.Parameters.Add("@CategoryId", System.Data.DbType.Int32).Value = categoryId;
                cm.Parameters.Add("@AgeGroupName", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = ageGroupName;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

//        public static void update(RegistrationEntry entry)
//        {
//            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
//            using (SQLiteCommand cm = new SQLiteCommand(
//                @"UPDATE Registration SET Email = @email, FirstName = @firstName, LastName = @lastName, ClubName = @clubName,
//                        AgeGroup = @ageGroup, ModelName = @modelName, ModelClass = @modelClass, ModelScale = @modelScale, 
//                        ModelPublisher = @modelPublisher, ModelCategory = @modelCategory, ModelCategoryId = COALESCE(@modelCategoryId, -1), YearOfBirth = @yearOfBirth
//                  WHERE EntryId = @entryId", cn))
//            {
//                cn.Open();
//                cm.CommandType = System.Data.CommandType.Text;

//                cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = entry.email;
//                cm.Parameters.Add("@firstName", System.Data.DbType.String, 64).Value = entry.firstName;
//                cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = entry.lastName;
//                cm.Parameters.Add("@clubName", System.Data.DbType.String, 128).Value = entry.clubName;
//                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = entry.ageGroupName;
//                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = entry.modelName;
//                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = entry.className;
//                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = entry.modelScale;
//                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = entry.modelPublisher;
//                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = entry.categoryName;
//                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int64).Value = entry.categoryId;
//                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = entry.yearOfBirth;
//                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = entry.registrationId;

//                cm.ExecuteNonQuery();
//            }
//        }

        public static bool delete(long registrationId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Registration WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = registrationId;
                return ((int)cm.ExecuteNonQuery() == 1);
            }
        }

        public static List<KeyValuePair<string, string>> getRegistrationStats() {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            int categoryTotal;
            //int i;
            object result;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                // --- Summary section ---
                ret.Add(new KeyValuePair<string, string>("GROUP1", "Podsumowanie"));

                //Modeler count
                cm.CommandText = "SELECT COUNT(Id) AS cnt FROM Modelers";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modelarzy", categoryTotal.ToString()));

                //Model count
                cm.CommandText = "SELECT COUNT(Id) FROM Models";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modeli", categoryTotal.ToString()));

                //Category count
                cm.CommandText = "SELECT COUNT(Id) FROM Categories";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba kategorii", categoryTotal.ToString()));

                // --- AGE GROUPS ---
                //ret.Add(new KeyValuePair<string, string>("GROUP2", "Grupy wiekowe"));

                //Modelers by age group
                ret.Add(new KeyValuePair<string, string>("GROUP2", "Liczba modelarzy w grupach wiekowych"));
                cm.CommandText = 
                    @"SELECT ag.AgeGroupName AS AgeGroup, COUNT(m.Id) AS [Count]
	                    FROM (SELECT Id, (strftime('%Y', 'now') - YearOfBirth) AS Age FROM Modelers) m
	                    LEFT JOIN (
		                    SELECT ag.Id AS AgeGroupId, ag.Name AS AgeGroupName, ag.Age AS MaxAge, x.MinAge
			                    FROM AgeGroups ag
			                    JOIN (
				                    SELECT x.Id, MAX(x.MinAge) AS MinAge FROM(
					                    SELECT a1.Id, a1.Name, a1.Age AS MaxAge, COALESCE(a2.Age + 1, 0) AS MinAge
						                    FROM AgeGroups a1 
						                    LEFT JOIN AgeGroups a2 ON a1.Age > a2.Age
				                    ) x GROUP BY x.Id
			                    ) x ON ag.Id = x.Id
		                    ) ag ON (m.Age BETWEEN ag.MinAge AND ag.MaxAge)
	                    GROUP BY AgeGroupName";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroup"].ToString(), dr["Count"].ToString()));
                    }
                }

                //Models by age group
                ret.Add(new KeyValuePair<string, string>("GROUP3", "Liczba modeli w grupach wiekowych"));
                cm.CommandText =
                    @"SELECT r.AgeGroupName AS AgeGroup, COUNT(r.Id) AS [Count]
	                    FROM Registration r
	                    JOIN AgeGroups ag ON r.AgeGroupName = ag.Name
	                    WHERE CategoryId > -1
                    GROUP BY r.AgeGroupName
                    ORDER BY ag.Age";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroup"].ToString(), dr["Count"].ToString()));
                    }
                }

                // --- Youngest/Oldest ---
                ret.Add(new KeyValuePair<string, string>("GROUP4", "Najmłodsi modelarze"));

                cm.CommandText =
                    @"SELECT DISTINCT FirstName, LastName, YearOfBirth FROM Modelers 
                        WHERE YearOfBirth IN (
                            SELECT x.YearOfBirth FROM (SELECT DISTINCT YearOfBirth FROM Modelers) x ORDER BY x.YearOfBirth DESC LIMIT 3
                        )
                        ORDER BY YearOfBirth DESC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["FirstName"].ToString() + " " + dr["LastName"].ToString(), dr["YearOfBirth"].ToString()));
                    }
                }

                ret.Add(new KeyValuePair<string, string>("GROUP5", "Najstarsi modelarze"));

                cm.CommandText =
                    @"SELECT DISTINCT FirstName, LastName, YearOfBirth FROM Modelers
                        WHERE YearOfBirth IN (
                            SELECT x.YearOfBirth FROM (SELECT DISTINCT YearOfBirth FROM Modelers) x ORDER BY x.YearOfBirth ASC LIMIT 3
                        )
                        ORDER BY YearOfBirth ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["FirstName"].ToString() + " " + dr["LastName"].ToString(), dr["YearOfBirth"].ToString()));
                    }
                }

                // --- Model Class ---
                ret.Add(new KeyValuePair<string, string>("GROUP6", "Modele w klasach"));

                //Model count in class
                cm.CommandText =
                    @"SELECT c.ModelClass, COUNT(r.ModelId) AS [Count]
	                    FROM Registration r
	                    JOIN Categories c ON r.CategoryId = c.Id
                    GROUP BY ModelClass 
                    ORDER BY [Count] DESC, ModelClass ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelClass"].ToString(), dr["Count"].ToString()));
                    }
                }

                // --- Model Category ---
                ret.Add(new KeyValuePair<string, string>("GROUP7", "Modele w kategoriach"));

                //Model count in class
                cm.CommandText =
                    @"SELECT c.ModelClass, COALESCE(c.Name, r.CategoryName) AS ModelCategory, r.AgeGroupName, COUNT(r.Id) AS [Count], r.CategoryId, ct.CategoryTotal
	                    FROM Registration r
	                    LEFT JOIN Categories c ON r.CategoryId = c.Id
	                    JOIN (SELECT CategoryId, COUNT(Id) AS CategoryTotal FROM Registration GROUP BY CategoryId) ct ON ct.CategoryId = r.CategoryId
	                    GROUP BY c.ModelClass, r.CategoryId, r.AgeGroupName
                    ORDER BY CategoryTotal DESC, DisplayOrder, ModelCategory, AgeGroupName, ModelClass";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {

                    int categoryId = -2;

                    while (dr.Read()) {
                        int currentCatId = dr.GetInt32(dr.GetOrdinal("CategoryId"));
                        if (categoryId != currentCatId) {
                            categoryTotal = 0;
                            categoryId = currentCatId;
                            ret.Add(new KeyValuePair<string, string>("*" + dr["ModelCategory"].ToString(), dr["CategoryTotal"].ToString()));
                        }

                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroupName"].ToString(), dr["Count"].ToString()));
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
                    @"CREATE TABLE Registration(
                        Id INTEGER PRIMARY KEY,
                        TmStamp DATETIME NOT NULL,
                        ModelId INTEGER NOT NULL REFERENCES Models(Id),
                        CategoryId INTEGER NOT NULL DEFAULT -1,
                        CategoryName TEXT NULL,
                        AgeGroupName TEXT NULL REFERENCES AgeGroup(Name));
                    CREATE UNIQUE INDEX Idx_Reg_ModelCat ON Registration(ModelId, CategoryId);";
                cm.ExecuteNonQuery();
            }
        }
    }
}
