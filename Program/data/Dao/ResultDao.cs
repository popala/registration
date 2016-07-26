using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

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
                ORDER BY a.Age, DisplayOrder, e.ModelCategory, e.ModelClass, r.Place", cn)) {
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
                    ORDER BY ag.Age ASC, mc.DisplayOrder, e.ModelCategory, e.ModelClass, r.Place", cn)) {
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

        public static int getDiplomaCount() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT COUNT(ResultId) FROM Results", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                return (int)cm.ExecuteScalar();
            }
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
