using Rejestracja.Utils;
using Rejestracja.Data.Objects;
using System;
using MySql.Data.MySqlClient;
using System.Text;

namespace Rejestracja.Data {
    class MySqlUtil {

        public static string testConnection(string server, string database, string user, string password, int port, string additionalOptions) {
            String connectionString = String.Format("server={0};user={1};port={2};password={3};database={4};{5}", server, user, port, password, database, additionalOptions);
            StringBuilder sb = new StringBuilder();

            using (MySqlConnection cn = new MySqlConnection(connectionString))
            using (MySqlCommand cm = new MySqlCommand("SHOW TABLES", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using(MySqlDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        sb.AppendLine(dr[0].ToString());
                    }
                }
            }
            return sb.ToString();
        }

        public static void createDatabase(string server, string database, string user, string password, int port, string additionalOptions) {
            String connectionString = String.Format("server={0};user={1};port={2};password={3};{4}", server, user, port, password, additionalOptions);

            using (MySqlConnection cn = new MySqlConnection(connectionString))
            using (MySqlCommand cm = new MySqlCommand("CREATE DATABASE IF NOT EXISTS @database", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.AddWithValue("@database", database);
                cm.ExecuteNonQuery();
            }
        }

        public static void createTables() {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                String.Format(
                    @"CREATE TABLE Classes(
                        Id INTEGER PRIMARY KEY,
                        Name VARCHAR({0}) NOT NULL,
                        RegistrationTemplate VARCHAR({1}) NULL,
                        JudgingFormTemplate VARCHAR({1}) NULL,
                        DiplomaTemplate VARCHAR({1}) NULL,
                        ScoringCardType TINYINT NOT NULL DEFAULT 0,
                        UseCustomAgeGroups TINYINT NOT NULL DEFAULT 0,
                        ClassificationType TINYINT NOT NULL DEFAULT 0,
                        UsePointRange TINYINT NOT NULL DEFAULT 0,
                        PointRanges VARCHAR({2}) NULL,
                        UseDistinctions TINYINT NOT NULL DEFAULT 0);
                    CREATE UNIQUE INDEX Idx_Class_Name ON Classes(Name);",
                    Class.MAX_NAME_LENGTH,
                    1024,
                    256
                )
            );

            sb.Append(
                String.Format(
                    @"CREATE TABLE Modelers(
                        Id INTEGER PRIMARY KEY,
                        FirstName VARCHAR({0}) NOT NULL,
                        LastName VARCHAR({0}) NOT NULL,
                        ClubName VARCHAR({0}) NULL,
                        YearOfBirth SMALLINT NOT NULL DEFAULT 0,
                        Email VARCHAR({1}));
                    CREATE INDEX Idx_Modeler_Name ON Modelers(FirstName, LastName, Email);",
                    Modeler.MAX_NAME_LENGTH,
                    Modeler.MAX_EMAIL_LENGTH
                )
            );

            sb.Append(
                String.Format(
                    @"CREATE TABLE Models(
                        Id INTEGER PRIMARY KEY,
                        Name VARCHAR({0}),
                        Publisher VARCHAR({1}),
                        Scale VARCHAR({2}),
                        ModelerId INTEGER NOT NULL,
                        FOREIGN KEY fk_modeler_id (ModelerId)
                        REFERENCES Modelers(Id)
                        ON DELETE cascade
                        ON UPDATE cascade);
                    CREATE INDEX Idx_Models_MdlrId ON Models(ModelerId);",
                    Model.MAX_NAME_LENGTH,
                    Publisher.MAX_NAME_LENGTH,
                    Scale.MAX_NAME_LENGTH
                )
            );

            String query = 
@"CREATE TABLE Registration(
    Id INTEGER PRIMARY KEY,
    TmStamp DATETIME NOT NULL,
    ModelId INTEGER NOT NULL REFERENCES Models(Id),
    CategoryId INTEGER NOT NULL DEFAULT -1,
    CategoryName TEXT NULL,
    AgeGroupName TEXT NULL REFERENCES AgeGroup(Name));
CREATE UNIQUE INDEX Idx_Reg_ModelCat ON Registration(ModelId, CategoryId);

CREATE TABLE Results(
	ResultId INTEGER PRIMARY KEY,
	RegistrationId INTEGER NOT NULL REFERENCES Registration(Id),
	AwardId INTEGER NULL REFERENCES SpecialAwards(Id),
	Place INTEGER NULL);

CREATE TABLE AgeGroups(
    Id INTEGER PRIMARY KEY,
    ClassId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Age INTEGER NOT NULL);
CREATE UNIQUE INDEX Idx_AgeGroups_Name_ClsId ON AgeGroups(ClassId, Name);

CREATE TABLE Categories(
    Id INTEGER PRIMARY KEY,
    Code TEXT NOT NULL,
    Name TEXT,
    ModelClass TEXT,
    DisplayOrder INTEGER NOT NULL);
CREATE INDEX Idx_MC_Code ON Categories(Code);
CREATE INDEX Idx_MC_Class ON Categories(ModelClass);

CREATE TABLE Scales(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL);
CREATE UNIQUE INDEX Idx_Scales_Name ON Scales(Name);

CREATE TABLE Publishers(
    Id INTEGER  PRIMARY KEY,
    Name TEXT NOT NULL);
CREATE INDEX Idx_Pub_Name ON Publishers(Name);

CREATE TABLE Awards(
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    DisplayOrder INTEGER NOT NULL);
CREATE INDEX Idx_Aw_Title ON Awards(Title);

CREATE TABLE Options(
    Name TEXT NOT NULL PRIMARY KEY,
    Value TEXT NOT NULL);
CREATE UNIQUE INDEX Idx_Opt_Name ON Options(Name);

CREATE TABLE Version(Version REAL NOT NULL);
CREATE UNIQUE INDEX Idx_Ver_Ver ON Version(Version);
INSERT INTO Version(Version) VALUES(.941);";

            using (MySqlConnection cn = new MySqlConnection(Resources.getConnectionString()))
            using (MySqlCommand cm = new MySqlCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }
    }
}