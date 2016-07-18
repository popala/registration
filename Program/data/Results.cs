using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja {
    class Results {
        public static long addCategoryResult(long entryId, int place) {
            return addResult(entryId, (int?)null, place);
        }

        public static long addAwardWinner(long entryId, long awardId) {
            return addResult(entryId, awardId, (int?)null);
        }

        private static long addResult(long entryId, long? awardId, int? place) {
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

        public static void deleteResult(long resultId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE ResultId = @resultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@resultId", System.Data.DbType.Int64).Value = resultId;
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

        public static void deleteResult(long entryId, int place) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId = @EntryId AND Place = @Place", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@EntryId", System.Data.DbType.Int64).Value = entryId;
                cm.Parameters.Add("@Place", System.Data.DbType.Int32).Value = place;
                cm.ExecuteNonQuery();
            }
        }

        public static bool awardEntryExists(long entryId, long awardId) {
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
                                dr["EntryId"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["Place"].ToString()
                            };
                    }
                }
            }
        }

        public static IEnumerable<RegistrationEntry> getEntriesInCategory(String modelCategory) {
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

        public static IEnumerable<WinningEntry> getCategoryResults() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.ResultId, e.FirstName, e.LastName, e.AgeGroup, e.EntryId, e.ModelName, e.ModelClass, e.ModelCategory, r.Place, ag.Age,
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
                        yield return
                            new WinningEntry(
                                dr.GetInt64(dr.GetOrdinal("ResultId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr.GetInt64(dr.GetOrdinal("EntryId")),
                                dr["ModelName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("Place"))
                            );
                    }
                }
            }
        }

        public static IEnumerable<String[]> getAwardResultList() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.ResultId, e.EntryId, e.ModelName, e.ModelScale, e.ModelPublisher, a.Title, a.Id
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
                                dr["EntryId"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString(),
                                dr["Title"].ToString(),
                            };
                    }
                }
            }
        }

        public static IEnumerable<WinningEntry> getAwardResults() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.ResultId, e.FirstName, e.LastName, e.AgeGroup, e.EntryId, e.ModelName, e.ModelClass, e.ModelCategory, a.Id AS AwardId, a.Title
                    FROM Results r
                    JOIN Registration e ON r.EntryId = e.EntryId
                    JOIN SpecialAwards a ON r.AwardId = a.Id
                WHERE a.Id IS NOT NULL
                    ORDER BY a.DisplayOrder, r.ResultId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new WinningEntry(
                                dr.GetInt64(dr.GetOrdinal("ResultId")),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr.GetInt64(dr.GetOrdinal("EntryId")),
                                dr["ModelName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr.GetInt64(dr.GetOrdinal("AwardId")),
                                dr["Title"].ToString()
                            );
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

        public static List<KeyValuePair<string, string>> getRegistrationStats() {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            int count;
            int i;
            object result;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                // --- POSUMOWANIE ---
                ret.Add(new KeyValuePair<string, string>("GROUP1", "Podsumowanie"));

                //Liczba modelarzy
                cm.CommandText = "SELECT COUNT(*) AS cnt FROM (SELECT DISTINCT LastName, FirstName, AgeGroup FROM Registration) x";
                result = cm.ExecuteScalar();
                count = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modelarzy", count.ToString()));

                //Liczba modeli
                cm.CommandText = "SELECT COUNT(EntryId) FROM Registration";
                result = cm.ExecuteScalar();
                count = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba modeli", count.ToString()));

                //Liczba kategorii
                cm.CommandText = "SELECT COUNT(*) FROM (SELECT DISTINCT ModelCategory FROM Registration) x";
                result = cm.ExecuteScalar();
                count = (result == null ? 0 : int.Parse(result.ToString()));
                ret.Add(new KeyValuePair<string, string>("Liczba kategorii", count.ToString()));

                // --- GRUPY WIEKOWE ---
                ret.Add(new KeyValuePair<string, string>("GROUP2", "Grupy wiekowe"));

                //Liczba modelarzy w każdej grupie wiekowej
                cm.CommandText = "SELECT AgeGroup, COUNT(LastName) AS cnt FROM (SELECT DISTINCT LastName, FirstName, AgeGroup FROM Registration) x GROUP BY AgeGroup";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    i = 0;
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(
                            (i == 0 ? "Liczba modelarzy w groupie " : "") + dr["AgeGroup"].ToString(), dr["cnt"].ToString()));
                        i++;
                    }
                }

                //Liczba modeli w każdej grupie wiekowej
                cm.CommandText = "SELECT AgeGroup, COUNT(ModelName) AS cnt FROM Registration GROUP BY AgeGroup";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    i = 0;
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(
                            (i == 0 ? "Liczba modeli w groupie " : "") + dr["AgeGroup"].ToString(), dr["cnt"].ToString()));
                        i++;
                    }
                }

                // --- KATEGORIE ---
                ret.Add(new KeyValuePair<string, string>("GROUP3", "Modele w klasach"));

                //Liczba modeli w każdej kategorii
                cm.CommandText = "SELECT ModelClass, COUNT(ModelName) AS cnt FROM Registration GROUP BY ModelClass ORDER BY cnt DESC, ModelClass ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelClass"].ToString(), dr["cnt"].ToString()));
                    }
                }

                // --- OPEN ---
                ret.Add(new KeyValuePair<string, string>("GROUP4", "Modele w kategorii 'Waloryzowane (Open)'"));

                //Liczba modeli w każdej kategorii
                cm.CommandText = "SELECT ModelClass, COUNT(ModelName) AS cnt FROM Registration WHERE ModelCategory = LOWER('waloryzowane (open)') GROUP BY ModelClass ORDER BY cnt DESC, ModelClass ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelClass"].ToString(), dr["cnt"].ToString()));
                    }
                }

                // --- Standard ---
                ret.Add(new KeyValuePair<string, string>("GROUP5", "Modele w kategorii 'Standard'"));

                //Liczba modeli w każdej kategorii
                cm.CommandText = "SELECT ModelClass, COUNT(ModelName) AS cnt FROM Registration WHERE ModelCategory = LOWER('standard') GROUP BY ModelClass ORDER BY cnt DESC, ModelClass ASC";
                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        ret.Add(new KeyValuePair<string, string>(dr["ModelClass"].ToString(), dr["cnt"].ToString()));
                    }
                }

                // --- Youngest/Oldest ---
                ret.Add(new KeyValuePair<string, string>("GROUP6", "Najmłodsi modelarze"));

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

                ret.Add(new KeyValuePair<string, string>("GROUP6", "Najstarsi modelarze"));

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
            }

            return ret;
        }
    }
}
