using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Dao {
    class RegistrationDao {

        public static IEnumerable<Registration> getList() {

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName FROM Registration ORDER BY Id", cn)) {
                
                cn.Open();
                
                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read())
                        yield return new Registration(
                            dr.GetInt32(0),
                            dr.GetDateTime(1),
                            dr.GetInt32(2),
                            dr.GetInt32(3),
                            dr.GetString(4),
                            dr.GetString(3)
                        );
                }
            }
        }

        public static int add(DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName) {
            return add(timeStamp, modelId, categoryId, categoryName, null);
        }

        public static int add(Registration registration) {
            return add(DateTime.MinValue, -1, -1, null, null, registration);
        }

        private static int add(DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName, Registration registration) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"INSERT INTO Registration(TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName) VALUES(@TmStamp, @ModelId, @CategoryId, @CategoryName, @AgeGroupName)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                if(registration == null) {
                    cm.Parameters.Add("@TmStamp", DbType.DateTime).Value = timeStamp;
                    cm.Parameters.Add("@ModelId", DbType.Int32).Value = modelId;
                    cm.Parameters.Add("@CategoryId", DbType.Int32).Value = categoryId;
                    cm.Parameters.Add("@CategoryName", DbType.String, Category.MAX_NAME_LENGTH).Value = categoryName;
                    cm.Parameters.Add("@AgeGroupName", DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = ageGroupName;
                }
                else {
                    cm.Parameters.Add("@TmStamp", DbType.DateTime).Value = registration.timeStamp;
                    cm.Parameters.Add("@ModelId", DbType.Int32).Value = registration.modelId;
                    cm.Parameters.Add("@CategoryId", DbType.Int32).Value = registration.categoryId;
                    cm.Parameters.Add("@CategoryName", DbType.String, Category.MAX_NAME_LENGTH).Value = registration.categoryName;
                    cm.Parameters.Add("@AgeGroupName", DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = registration.ageGroupName;
                }
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static Registration get(int id) {
            Registration ret = null;

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName FROM Registration WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = id;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        ret = new Registration(
                            dr.GetInt32(0),
                            dr.GetDateTime(1),
                            dr.GetInt32(2),
                            dr.GetInt32(3),
                            dr.GetString(4),
                            dr.GetString(5)
                        );
                }
            }
            return ret;
        }

        public static void delete(int id) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE RegistrationId = @Id;
                    DELETE FROM Registration WHERE ModelId = @Id;", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", DbType.Int32).Value = id;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable() {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"CREATE TABLE Models(
                        Id INTEGER PRIMARY KEY,
                        Name TEXT,
                        Publisher TEXT,
                        Scale TEXT,
                        ModelerId INTEGER NOT NULL REFERENCES Modelers(Id));
                    CREATE INDEX Idx_Models_MdlrId ON Models(ModelerId);", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }
    }
}
