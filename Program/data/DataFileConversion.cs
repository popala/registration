using Rejestracja.Utils;
using System;
using System.Data.SQLite;

namespace Rejestracja.Data {
    class DataFileConversion {

        public static void convertTo941() {

            String query =
@"CREATE TABLE Modelers(
    Id INTEGER PRIMARY KEY,
    FirstName TEXT,
    LastName TEXT,
    ClubName TEXT,
    YearOfBirth INTEGER,
    Email TEXT);
CREATE INDEX Idx_Modeler_Name ON Modelers(FirstName, LastName, Email);

INSERT INTO Modelers(FirstName, LastName, ClubName, YearOfBirth, Email)
    SELECT FirstName, LastName, ClubName, YearOfBirth, Email
    FROM Registration
	GROUP BY LastName, FirstName, Email;


CREATE TABLE Models(
    Id INTEGER PRIMARY KEY,
    Name TEXT,
    Publisher TEXT,
    Scale TEXT,
    ModelerId INTEGER NOT NULL REFERENCES Modelers(Id));
CREATE INDEX Idx_Models_MdlrId ON Models(ModelerId);

INSERT INTO Models(Id, Name, Publisher, Scale, ModelerId)
	SELECT r.EntryId, r.ModelName, r.ModelPublisher, r.ModelScale, m.Id
	FROM Registration r
	JOIN Modelers m ON r.FirstName = m.FirstName AND r.LastName = m.LastName AND r.Email = m.Email;

ALTER TABLE Registration RENAME TO OldRegistration;
CREATE TABLE Registration(
    Id INTEGER PRIMARY KEY,
    TmStamp DATETIME NOT NULL,
    ModelId INTEGER NOT NULL REFERENCES Models(Id),
    CategoryId INTEGER NOT NULL DEFAULT -1,
    CategoryName TEXT NULL,
    AgeGroupName TEXT NULL REFERENCES AgeGroup(Name));
CREATE UNIQUE INDEX Idx_Reg_ModelCat ON Registration(ModelId, CategoryId);

INSERT INTO Registration(ModelId, CategoryId, CategoryName, AgeGroupName)
SELECT m.Id, oreg.TmStamp, oreg.ModelCategoryId, CASE WHEN oreg.ModelCategoryId < 0 THEN oreg.ModelCategory ELSE NULL END, ag.Name
	FROM Models m
	JOIN OldRegistration oreg ON m.Id = oreg.EntryId
	LEFT JOIN AgeGroup ag ON ag.Name = oreg.AgeGroup;


ALTER TABLE Results RENAME TO OldResults;
CREATE TABLE Results(
	ResultId INTEGER PRIMARY KEY,
	RegistrationId INTEGER NOT NULL REFERENCES Registration(Id),
	AwardId INTEGER NULL REFERENCES SpecialAwards(Id),
	Place INTEGER NULL);

INSERT INTO Results(RegistrationId, Place, AwardId)
	SELECT r.Id, ores.Place, ores.AwardId
		FROM OldResults ores
		JOIN Registration r ON ores.EntryId = r.ModelId
		LEFT JOIN SpecialAwards aw ON aw.Id = ores.AwardId;


ALTER TABLE AgeGroup RENAME TO AgeGroups;
ALTER TABLE ModelCategory RENAME TO Categories;
ALTER TABLE ModelClass RENAME TO Classes;
ALTER TABLE ModelScale RENAME TO Scales;
ALTER TABLE Publisher RENAME TO Publishers;
ALTER TABLE SpecialAwards RENAME TO Awards;


CREATE TABLE Version(Version REAL NOT NULL);
CREATE UNIQUE INDEX Idx_Ver_Ver ON Version(Version);
INSERT INTO Version(Version) VALUES(.941);
";

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }

        }
    }
}