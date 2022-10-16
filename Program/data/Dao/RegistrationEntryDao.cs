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
        public static RegistrationEntry get(long entryId)
        {
            return get(entryId, null, null);
        }

        private static RegistrationEntry get(long? entryId, String email, String lastName)
        {
            RegistrationEntry ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn))
            {
                cn.Open();

                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText =
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup, 
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, 
                        COALESCE(ModelCategoryId, -1) AS ModelCategoryId,
                        COALESCE(YearOfBirth,0) AS YearOfBirth, COALESCE(SkipErrorValidation,0) AS SkipErrorValidation
                     FROM Registration ";

                if (entryId.HasValue)
                {
                    cm.CommandText += "WHERE EntryId = @EntryId";
                    cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;
                }
                else if (email != null)
                {
                    cm.CommandText += "WHERE email = @email";
                    cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = email;
                }
                else if (lastName != null)
                {
                    cm.CommandText += "WHERE lastName = @lastName";
                    cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = lastName;
                }
                else
                {
                    throw new ArgumentNullException("EntryId, Email or LastName have to be provided!");
                }

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ret = new RegistrationEntry(
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
                    }
                }
            }
            return ret;
        }

        public static IEnumerable<String[]> getList(String searchValue, int sortField, bool sortAscending) {

            string sortFieldName;

            switch (sortField) { 
                case 0: sortFieldName = "EntryId"; break;
                case 1: sortFieldName = "TmStamp"; break;
                case 2: sortFieldName = "Email"; break;
                case 3: sortFieldName = "FirstName"; break;
                case 4: sortFieldName = "LastName"; break;
                case 5: sortFieldName = "YearOfBirth"; break;
                case 6: sortFieldName = "ClubName"; break;
                case 7: sortFieldName = "AgeGroup"; break;
                case 8: sortFieldName = "ModelName"; break;
                case 9: sortFieldName = "ModelCategory"; break;
                case 10: sortFieldName = "ModelClass"; break;
                case 11: sortFieldName = "ModelScale"; break;
                case 12: sortFieldName = "ModelPublisher"; break;
                default: sortFieldName = "EntryId"; break;
            }

            return getList(searchValue, sortFieldName, sortAscending);
        }

        private static IEnumerable<String[]> getList(String searchValue, String sortField, bool sortAscending)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn))
            {
                StringBuilder query = new StringBuilder(
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                    ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(YearOfBirth,0) AS YearOfBirth 
                 FROM Registration ");

                if (!String.IsNullOrEmpty(searchValue))
                {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n))
                    {
                        query.Append(" EntryId = @EntryId OR ");
                        cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = n;
                    }

                    query.Append(" Email LIKE @SearchValue OR FirstName LIKE @SearchValue OR LastName LIKE @SearchValue OR ModelName LIKE @SearchValue OR ClubName LIKE @SearchValue ");
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
                                dr["EntryId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString()
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
                StringBuilder query = new StringBuilder(
                    @"SELECT r.EntryId, r.TmStamp, r.Email, r.FirstName, r.LastName, r.ClubName, r.AgeGroup,
                        r.ModelName, r.ModelClass, r.ModelScale, r.ModelPublisher, r.ModelCategory, COALESCE(r.YearOfBirth,0) AS YearOfBirth,
                        CASE WHEN m.DisplayOrder IS NULL THEN -1 ELSE m.DisplayOrder END AS DisplayOrder
                    FROM Registration r
                        LEFT JOIN ModelCategory m ON m.Id = r.ModelCategoryId");

                if (!String.IsNullOrEmpty(searchValue)) {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n)) {
                        query.Append(" EntryId = @EntryId OR ");
                        cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = n;
                    }

                    query.Append(" Email LIKE @SearchValue OR FirstName LIKE @SearchValue OR LastName LIKE @SearchValue OR ModelName LIKE @SearchValue OR ClubName LIKE @SearchValue ");
                    cm.Parameters.Add("@SearchValue", System.Data.DbType.String, 64).Value = "%" + searchValue + "%";
                }
                query.Append(" ORDER BY DisplayOrder, r.ModelCategory, r.AgeGroup, r.ModelClass, r.EntryId");

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
                                dr["EntryId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString()
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
                @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(ModelCategoryId, -1) AS ModelCategoryId,
                        COALESCE(YearOfBirth,0) AS YearOfBirth, COALESCE(SkipErrorValidation,0) AS SkipErrorValidation
                    FROM Registration 
                    ORDER BY ModelCategory, AgeGroup, ModelClass, EntryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ret.Add(
                            new RegistrationEntry(
                                dr.GetInt64(dr.GetOrdinal("EntryId")),
                                dr.GetDateTime(dr.GetOrdinal("TmStamp")),
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
                        ));
                    }
                    return ret;
                }
            }
        }

        public static IEnumerable<String[]> getListForMergingCategories(long maxEntryCount) {

            String query =
                @"SELECT r.AgeGroup, r.ModelClass, r.ModelCategory, COALESCE(r.ModelCategoryId, -1) AS ModelCategoryId, COUNT(r.EntryId) AS EntryCount, CASE m.DisplayOrder WHEN NULL THEN 0 ELSE m.DisplayOrder END AS DisplayOrder
                    FROM Registration r
	                    LEFT JOIN ModelCategory m ON m.Id = r.ModelCategoryId
	                    LEFT JOIN AgeGroup ag ON r.AgeGroup = ag.Name
	                    JOIN (
		                    SELECT DISTINCT ModelCategoryId
			                    FROM (
				                    SELECT ModelCategoryId, AgeGroup, COUNT(EntryId) AS EntryCount 
					                    FROM Registration 
					                    WHERE ModelCategoryId > -1
					                    GROUP BY ModelCategoryId, AgeGroup" +
                                        (maxEntryCount > 0 ? " HAVING EntryCount < @MaxCount " : "") +
                              @")
                        ) sm ON r.ModelCategoryId = sm.ModelCategoryId
                    GROUP BY r.AgeGroup, r.ModelClass, r.ModelCategoryId
                    ORDER BY m.DisplayOrder, r.ModelCategory, ag.Age, r.ModelClass";

            //if (maxEntryCount < 1) {
            //    yield break;
            //}
            
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@MaxCount", System.Data.DbType.Int32).Value = maxEntryCount;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return new String[] {
                            dr["AgeGroup"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["ModelCategory"].ToString(),
                            dr["ModelCategoryId"].ToString(),
                            dr["EntryCount"].ToString()
                        };
                    }
                }
            }
        }

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
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = entry.ageGroup;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = entry.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = entry.modelClass;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = entry.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = entry.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = entry.modelCategory;
                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int32).Value = entry.modelCategoryId;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = entry.yearOfBirth;
                cm.ExecuteNonQuery();

                entry.entryId = cn.LastInsertRowId;
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
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = entry.ageGroup;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = entry.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = entry.modelClass;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = entry.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = entry.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = entry.modelCategory;
                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int64).Value = entry.modelCategoryId;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = entry.yearOfBirth;
                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = entry.entryId;

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

                //Model count by publisher
                ret.Add(new KeyValuePair<string, string>("GROUP8", "Wydawcy"));
                cm.CommandText =
                    @"SELECT COALESCE(ModelPublisher, 'N/A') AS ModelPublisher, COUNT(EntryId) AS cnt
                        FROM Registration
                        GROUP BY ModelPublisher
                    ORDER BY cnt DESC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelPublisher"].ToString(), dr["cnt"].ToString()));
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
