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
using System.Text;

namespace Rejestracja.Data.Dao
{
    class RegistrationEntryDao
    {
        private const String BASE_QUERY =
            @"SELECT 
	            r.Id AS RegistrationId, r.TmStamp, r.AgeGroupName,
	            r.CategoryId, COALESCE(c.Name, r.CategoryName) AS CategoryName, c.ModelClass, CASE WHEN c.DisplayOrder IS NULL THEN -1 ELSE c.DisplayOrder END AS DisplayOrder,
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
                            dr.GetInt32(dr.GetOrdinal("RegistrationId")),
                            (DateTime)dr["TmStamp"],
                            dr["AgeGroupName"].ToString(),

                            dr.GetInt32(dr.GetOrdinal("CategoryId")),
                            dr["CategoryName"].ToString(),
                            dr["ModelClass"].ToString(),

                            dr.GetInt32(dr.GetOrdinal("ModelerId")),
                            dr["FirstName"].ToString(),
                            dr["LastName"].ToString(),
                            dr["ClubName"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                            dr["Email"].ToString(),

                            dr.GetInt32(dr.GetOrdinal("ModelId")),
                            dr["ModelName"].ToString(),
                            dr["Publisher"].ToString(),
                            dr["Scale"].ToString()
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
                                dr["ModelName"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ClassName"].ToString(),
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
                query.Append(" ORDER BY DisplayOrder, r.CategoryName, r.AgeGroupName, r.ClassName, r.RegistrationId");

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
                                dr["ModelName"].ToString(),
                                dr["CategoryName"].ToString(),
                                dr["ClassName"].ToString(),
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
                                dr.GetInt32(dr.GetOrdinal("RegistrationId")),
                                (DateTime)dr["TmStamp"],
                                dr["AgeGroupName"].ToString(),

                                dr.GetInt32(dr.GetOrdinal("CategoryId")),
                                dr["CategoryName"].ToString(),
                                dr["ModelClass"].ToString(),

                                dr.GetInt32(dr.GetOrdinal("ModelerId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["ClubName"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("YearOfBirth")),
                                dr["Email"].ToString(),

                                dr.GetInt32(dr.GetOrdinal("ModelId")),
                                dr["ModelName"].ToString(),
                                dr["Publisher"].ToString(),
                                dr["Scale"].ToString()
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

        //TODO: RESTART HERE
        public static void mergeAgeGroupsInCategory(long modelCategoryId, String sourceAgeGroup, String targetAgeGroup) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE EntryId IN(SELECT EntryId FROM Registration WHERE AgeGroup = @SourceAgeGroup AND ModelCategoryId = @ModelCategoryId) AND AwardId IS NULL", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@SourceAgeGroup", System.Data.DbType.String, 64).Value = sourceAgeGroup;
                cm.Parameters.Add("@TargetAgeGroup", System.Data.DbType.String, 64).Value = targetAgeGroup;
                cm.Parameters.Add("@ModelCategoryId", System.Data.DbType.Int64).Value = modelCategoryId;

                cm.ExecuteNonQuery();

                cm.CommandText = "UPDATE Registration SET AgeGroup = @TargetAgeGroup WHERE AgeGroup = @SourceAgeGroup AND ModelCategoryId = @ModelCategoryId";
                cm.ExecuteNonQuery();
            }
        }

        public static void changeCategory(int entryId, int modelCategoryId, String modelCategory, String modelClass) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Registration SET ModelCategory = @ModelCategory, ModelCategoryId = @ModelCategoryId, ModelClass = @ModelClass WHERE EntryId = @EntryId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@ModelCategory", System.Data.DbType.String).Value = modelCategory;
                cm.Parameters.Add("@ModelCategoryId", System.Data.DbType.Int32).Value = modelCategoryId;
                cm.Parameters.Add("@ModelClass", System.Data.DbType.String).Value = modelClass;
                cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;

                cm.ExecuteNonQuery();
            }
        }
        
        public static void add(RegistrationEntry entry)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Registration(TmStamp,Email,FirstName,LastName,ClubName,AgeGroup,ModelName,ModelClass,ModelScale,ModelPublisher,ModelCategory,ModelCategoryId,YearOfBirth)
                  VALUES(@tmStamp,@email,@firstName,@lastName,@clubName,@ageGroup,@modelName,@modelClass,@modelScale,@modelPublisher,@modelCategory,@modelCategoryId,@yearOfBirth)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@tmStamp", System.Data.DbType.DateTime).Value = entry.timeStamp;
                cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = entry.email;
                cm.Parameters.Add("@firstName", System.Data.DbType.String, 64).Value = entry.firstName;
                cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = entry.lastName;
                cm.Parameters.Add("@clubName", System.Data.DbType.String, 128).Value = entry.clubName;
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = entry.ageGroupName;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = entry.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = entry.className;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = entry.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = entry.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = entry.categoryName;
                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int32).Value = entry.categoryId;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = entry.yearOfBirth;
                cm.ExecuteNonQuery();

                entry.registrationId = (int)cn.LastInsertRowId;
            }
        }

        public static void update(RegistrationEntry entry)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Registration SET Email = @email, FirstName = @firstName, LastName = @lastName, ClubName = @clubName,
                        AgeGroup = @ageGroup, ModelName = @modelName, ModelClass = @modelClass, ModelScale = @modelScale, 
                        ModelPublisher = @modelPublisher, ModelCategory = @modelCategory, ModelCategoryId = COALESCE(@modelCategoryId, -1), YearOfBirth = @yearOfBirth
                  WHERE EntryId = @entryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = entry.email;
                cm.Parameters.Add("@firstName", System.Data.DbType.String, 64).Value = entry.firstName;
                cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = entry.lastName;
                cm.Parameters.Add("@clubName", System.Data.DbType.String, 128).Value = entry.clubName;
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = entry.ageGroupName;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = entry.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = entry.className;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = entry.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = entry.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = entry.categoryName;
                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int64).Value = entry.categoryId;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = entry.yearOfBirth;
                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = entry.registrationId;

                cm.ExecuteNonQuery();
            }
        }

        public static bool delete(long entryId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Registration WHERE EntryId = @entryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = entryId;
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
                cm.CommandText = "SELECT COUNT(*) AS cnt FROM (SELECT DISTINCT LastName, FirstName, AgeGroup FROM Registration) x";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modelarzy", categoryTotal.ToString()));

                //Model count
                cm.CommandText = "SELECT COUNT(EntryId) FROM Registration";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modeli", categoryTotal.ToString()));

                //Category count
                cm.CommandText = "SELECT COUNT(*) FROM (SELECT DISTINCT ModelCategory FROM Registration) x";
                result = cm.ExecuteScalar();
                categoryTotal = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba kategorii", categoryTotal.ToString()));

                // --- AGE GROUPS ---
                //ret.Add(new KeyValuePair<string, string>("GROUP2", "Grupy wiekowe"));

                //Modelers by age group
                ret.Add(new KeyValuePair<string, string>("GROUP2", "Liczba modelarzy w grupach wiekowych"));
                cm.CommandText = "SELECT AgeGroup, COUNT(LastName) AS cnt FROM (SELECT DISTINCT LastName, FirstName, AgeGroup FROM Registration) x GROUP BY AgeGroup";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroup"].ToString(), dr["cnt"].ToString()));
                    }
                }

                //Models by age group
                ret.Add(new KeyValuePair<string, string>("GROUP3", "Liczba modeli w grupach wiekowych"));
                cm.CommandText = "SELECT AgeGroup, COUNT(ModelName) AS cnt FROM Registration GROUP BY AgeGroup";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroup"].ToString(), dr["cnt"].ToString()));
                    }
                }

                // --- Youngest/Oldest ---
                ret.Add(new KeyValuePair<string, string>("GROUP4", "Najmłodsi modelarze"));

                cm.CommandText =
                    @"SELECT DISTINCT FirstName, LastName, YearOfBirth FROM Registration 
                        WHERE YearOfBirth IN (
                            SELECT x.YearOfBirth FROM (SELECT DISTINCT YearOfBirth FROM Registration) x ORDER BY x.YearOfBirth DESC LIMIT 3
                        )
                        ORDER BY YearOfBirth DESC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["FirstName"].ToString() + " " + dr["LastName"].ToString(), dr["YearOfBirth"].ToString()));
                    }
                }

                ret.Add(new KeyValuePair<string, string>("GROUP5", "Najstarsi modelarze"));

                cm.CommandText =
                    @"SELECT DISTINCT FirstName, LastName, YearOfBirth FROM Registration 
                        WHERE YearOfBirth IN (
                            SELECT x.YearOfBirth FROM (SELECT DISTINCT YearOfBirth FROM Registration) x ORDER BY x.YearOfBirth ASC LIMIT 3
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
                cm.CommandText = "SELECT ModelClass, COUNT(ModelName) AS cnt FROM Registration GROUP BY ModelClass ORDER BY cnt DESC, ModelClass ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelClass"].ToString(), dr["cnt"].ToString()));
                    }
                }

                // --- Model Category ---
                ret.Add(new KeyValuePair<string, string>("GROUP7", "Modele w kategoriach"));

                //Model count in class
                cm.CommandText =
                    @"SELECT r.ModelClass, r.ModelCategory, r.AgeGroup, COUNT(EntryId) AS cnt, r.ModelCategoryId, ct.CategoryTotal
	                    FROM Registration r
	                    LEFT JOIN ModelCategory mc ON r.ModelCategoryId = mc.Id
	                    JOIN (SELECT ModelCategoryId, COUNT(EntryId) AS CategoryTotal FROM Registration GROUP BY ModelCategoryId) ct ON ct.ModelCategoryId = r.ModelCategoryId
	                    GROUP BY r.ModelClass, r.ModelCategoryId, r.AgeGroup
                    ORDER BY ct.CategoryTotal DESC, mc.DisplayOrder, r.ModelCategory, r.AgeGroup, r.ModelClass";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {

                    int categoryId = -2;

                    while (dr.Read()) {
                        int currentCatId = dr.GetInt32(dr.GetOrdinal("ModelCategoryId"));
                        if (categoryId != currentCatId) {
                            categoryTotal = 0;
                            categoryId = currentCatId;
                            ret.Add(new KeyValuePair<string, string>("*" + dr["ModelCategory"].ToString(), dr["CategoryTotal"].ToString()));
                        }

                        ret.Add(new KeyValuePair<string, string>(dr["AgeGroup"].ToString(), dr["cnt"].ToString()));
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
                        EntryId INTEGER PRIMARY KEY,
                        TmStamp DATETIME NOT NULL,
                        Email TEXT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        ClubName TEXT,
                        AgeGroup TEXT NOT NULL, 
                        ModelName TEXT NOT NULL,
                        ModelCategory TEXT NOT NULL,
                        ModelCategoryId INTEGER NOT NULL DEFAULT -1,
                        ModelScale TEXT NOT NULL,
                        ModelPublisher TEXT,
                        ModelClass TEXT NOT NULL,
                        YearOfBirth INTEGER NOT NULL,
                        SkipErrorValidation INTEGER NOT NULL DEFAULT 0)";
                cm.ExecuteNonQuery();
                cm.CommandText = "CREATE INDEX Idx_Reg_Name ON Registration(LastName, FirstName)";
                cm.ExecuteNonQuery();
                cm.CommandText = "CREATE INDEX Idx_Reg_Email ON Registration(Email)";
                cm.ExecuteNonQuery();
                cm.CommandText = "CREATE INDEX Idx_Reg_ModelName ON Registration(ModelName)";
                cm.ExecuteNonQuery();
                cm.CommandText = "CREATE INDEX Idx_Reg_CatId ON Registration(ModelCategoryId)";
                cm.ExecuteNonQuery();
            }
        }
    }
}
