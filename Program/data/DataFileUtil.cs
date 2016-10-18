﻿using Rejestracja.Utils;
using System;
using System.Data.SQLite;

namespace Rejestracja.Data {
    class DataFileUtil {

        public static void createTables() {
            String query = 
@"CREATE TABLE Classes(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    RegistrationTemplate TEXT NULL,
    JudgingFormTemplate TEXT NULL,
    DiplomaTemplate TEXT NULL,
    ScoringCardType INTEGER NOT NULL DEFAULT 0,
    UseCustomAgeGroups INTEGER NOT NULL DEFAULT 0,
    ClassificationType INTEGER NOT NULL DEFAULT 0,
    UsePointRange INTEGER NOT NULL DEFAULT 0,
    UseDistinctions INTEGERE NOT NULL DEFAULT 0);
CREATE UNIQUE INDEX Idx_Class_Name ON Classes(Name);

CREATE TABLE Classification(
    ClassId INTEGER NOT NULL REFERENCES Classes(Id),
    Place INTEGER NOT NULL DEFAULT 0,
    PointRange INTEGER NOT NULL DEFAULT 0
);
CREATE UNIQUE INDEX Idx_Classif ON Classification(ClassId, Place);

CREATE TABLE Modelers(
    Id INTEGER PRIMARY KEY,
    FirstName TEXT,
    LastName TEXT,
    ClubName TEXT,
    YearOfBirth INTEGER,
    Email TEXT);
CREATE INDEX Idx_Modeler_Name ON Modelers(FirstName, LastName, Email);

CREATE TABLE Models(
    Id INTEGER PRIMARY KEY,
    Name TEXT,
    Publisher TEXT,
    Scale TEXT,
    ModelerId INTEGER NOT NULL REFERENCES Modelers(Id));
CREATE INDEX Idx_Models_MdlrId ON Models(ModelerId);

CREATE TABLE Registration(
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

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }

        public static void convertTo941() {

            String query =
@"CREATE TABLE Classes(
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    RegistrationTemplate TEXT NULL,
    JudgingFormTemplate TEXT NULL,
    DiplomaTemplate TEXT NULL,
    ScoringCardType INTEGER NOT NULL DEFAULT 0,
    UseCustomAgeGroups INTEGER NOT NULL DEFAULT 0);
INSERT INTO Classes
    SELECT Id, Name, NULL, NULL, NULL, 0, 0 FROM ModelClass;
CREATE UNIQUE INDEX Idx_Class_Name ON Classes(Name);

CREATE TABLE Modelers(
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


ALTER TABLE AgeGroup RENAME TO OldAgeGroups;
CREATE TABLE AgeGroups(
    Id INTEGER PRIMARY KEY,
    ClassId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Age INTEGER NOT NULL);
CREATE UNIQUE INDEX Idx_AgeGroups_Name_ClsId ON AgeGroups(ClassId, Name);
INSERT INTO AgeGroups
    SELECT Id, -1, Name, Age FROM OldAgeGroups;


ALTER TABLE ModelClass RENAME TO OldClasses;
ALTER TABLE ModelCategory RENAME TO Categories;
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