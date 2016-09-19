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
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            (DateTime)dr["TmStamp"],
                            dr.GetInt32(dr.GetOrdinal("ModelId")),
                            dr.GetInt32(dr.GetOrdinal("CategoryId")),
                            dr["CategoryName"].ToString(),
                            dr["AgeGroupName"].ToString()
                        );
                }
            }
        }

        public static IEnumerable<Registration> getList(int modelId) {

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"SELECT r.Id, r.TmStamp, r.ModelId, r.CategoryId, COALESCE(c.Name, r.CategoryName) AS CategoryName, r.AgeGroupName
                    FROM Registration r 
                LEFT JOIN Categories c ON r.CategoryId = c.Id
                WHERE r.ModelId = @Id", cn)) {

                cn.Open();
                cm.Parameters.Add("@Id", DbType.Int32).Value = modelId;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read())
                        yield return new Registration(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            (DateTime)dr["TmStamp"],
                            dr.GetInt32(dr.GetOrdinal("ModelId")),
                            dr.GetInt32(dr.GetOrdinal("CategoryId")),
                            dr["CategoryName"].ToString(),
                            dr["AgeGroupName"].ToString()
                        );
                }
            }
        }

        public static int add(DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName) {
            return add(timeStamp, modelId, categoryId, categoryName, ageGroupName, null);
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

        public static Registration get(int modelId, int categoryId) {
            Registration ret = null;

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName FROM Registration WHERE ModelId = @ModelId AND CategoryId = @CategoryId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@ModelId", DbType.Int32).Value = modelId;
                cm.Parameters.Add("@CategoryId", DbType.Int32).Value = categoryId;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        ret = new Registration(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            (DateTime)dr["TmStamp"],
                            dr.GetInt32(dr.GetOrdinal("ModelId")),
                            dr.GetInt32(dr.GetOrdinal("CategoryId")),
                            dr["CategoryName"].ToString(),
                            dr["AgeGroupName"].ToString()
                        );
                }
            }
            return ret;
        }

        public static Registration get(int registrationId) {
            Registration ret = null;

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName FROM Registration WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = registrationId;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        ret = new Registration(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            (DateTime)dr["TmStamp"],
                            dr.GetInt32(dr.GetOrdinal("ModelId")),
                            dr.GetInt32(dr.GetOrdinal("CategoryId")),
                            dr["CategoryName"].ToString(),
                            dr["AgeGroupName"].ToString()
                        );
                }
            }
            return ret;
        }

        public static void delete(int registrationId) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE RegistrationId = @Id;
                    DELETE FROM Registration WHERE Id = @Id;", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", DbType.Int32).Value = registrationId;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(int modelId, String categoryName) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Registration WHERE ModelId = @ModelId AND CategoryName = @CategoryName", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@ModelId", DbType.Int32).Value = modelId;
                cm.Parameters.Add("@CategoryName", DbType.String).Value = categoryName;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(int modelId, int categoryId) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE RegistrationId IN(SELECT Id FROM Registration WHERE ModelId = @ModelId AND CategoryId = @CategoryId);
                    DELETE FROM Registration WHERE ModelId = @ModelId AND CategoryId = @CategoryId;", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@ModelId", DbType.Int32).Value = modelId;
                cm.Parameters.Add("@CategoryId", DbType.Int32).Value = categoryId;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable() {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"CREATE TABLE Registration(
                        Id INTEGER PRIMARY KEY,
                        TmStamp DATETIME NOT NULL,
                        ModelId INTEGER NOT NULL REFERENCES Models(Id),
                        CategoryId INTEGER NOT NULL DEFAULT -1,
                        CategoryName TEXT NULL,
                        AgeGroupName TEXT NULL REFERENCES AgeGroup(Name));
                    CREATE UNIQUE INDEX Idx_Reg_ModelCat ON Registration(ModelId, CategoryId);", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }
    }
}
