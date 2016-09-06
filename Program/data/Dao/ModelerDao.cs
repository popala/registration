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
    class ModelerDao {

        public static IEnumerable<Modeler> getList() {
            return getList(null);
        }

        public static IEnumerable<Modeler> search(String value) {
            return getList(value);
        }

        private static IEnumerable<Modeler> getList(String searchValue) {

            StringBuilder query = new StringBuilder("SELECT Id, FirstName, LastName, ClubName, YearOfBirth, Email FROM Modelers ");
            if(!String.IsNullOrWhiteSpace(searchValue)) {
                query.Append(" WHERE LastName LIKE @SearchValue OR FirstName LIKE @SearchValue OR Email LIKE @SearchValue ");
            }
            query.Append(" ORDER BY LastName, FirstName, Email ");

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(query.ToString(), cn)) {
                cn.Open();

                if(!String.IsNullOrWhiteSpace(searchValue)) {
                    cm.Parameters.Add("@SearchValue", DbType.String).Value = "%" + searchValue + "%";
                }

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read())
                        yield return new Modeler(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetInt32(4),
                            dr.GetString(5)
                        );
                }
            }
        }

        public static int add(String firstName, String lastName, String clubName, int yearOfBirth, String email) {
            return add(firstName, lastName, clubName, yearOfBirth, email, null);
        }

        public static int add(Modeler modeler) {
            return add(null, null, null, 0, null, modeler);
        }

        private static int add(String firstName, String lastName, String clubName, int yearOfBirth, String email, Modeler modeler) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                        @"INSERT INTO Modelers(FirstName, LastName, ClubName, YearOfBirth, Email) VALUES(@FirstName, @LastName, @ClubName, @YearOfBirth, @Email)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                if(modeler == null) {
                    cm.Parameters.Add("@FirstName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = firstName;
                    cm.Parameters.Add("@LastName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = lastName;
                    cm.Parameters.Add("@ClubName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = clubName;
                    cm.Parameters.Add("@YearOfBirth", System.Data.DbType.Int32).Value = yearOfBirth;
                    cm.Parameters.Add("@Email", System.Data.DbType.String, Modeler.MAX_EMAIL_LENGTH).Value = email;
                }
                else {
                    cm.Parameters.Add("@FirstName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = modeler.firstName;
                    cm.Parameters.Add("@LastName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = modeler.lastName;
                    cm.Parameters.Add("@ClubName", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = modeler.clubName;
                    cm.Parameters.Add("@YearOfBirth", System.Data.DbType.Int32).Value = modeler.yearOfBirth;
                    cm.Parameters.Add("@Email", System.Data.DbType.String, Modeler.MAX_EMAIL_LENGTH).Value = modeler.email;
                }
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static Modeler get(int id) {
            Modeler ret = null;

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, FirstName, LastName, ClubName, YearOfBirth, Email FROM Modelers WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = id;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        ret = new Modeler(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetInt32(4),
                            dr.GetString(3)
                        );
                }
            }
            return ret;
        }

        public static void delete(int id) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results 
                    WHERE RegistrationId IN(
                        SELECT RegistrationId FROM Registration WHERE ModelId IN(
                            SELECT ModelId FROM Models WHERE ModelerId = @Id
                        )
                    );
                    DELETE FROM Registration WHERE ModelId IN(
                        SELECT ModelId FROM Models WHERE ModelerId = @Id
                    );
                    DELETE FROM Models WHERE ModelerId = @Id;
                    DELETE FROM Modelers WHERE Id = @Id", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", DbType.Int32).Value = id;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable() {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"CREATE TABLE Modelers(
                        Id INTEGER PRIMARY KEY,
                        FirstName TEXT,
                        LastName TEXT,
                        ClubName TEXT,
                        YearOfBirth INTEGER,
                        Email TEXT);
                    CREATE INDEX Idx_Modeler_Name ON Modelers(FirstName, LastName, Email);", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }
    }
}
