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
using Rejestracja.Data;
using Rejestracja.Data.Dao;
using Rejestracja.Utils;
using System;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Rejestracja {
    class DataSource {
        private String _connectionString;

        public DataSource() {
            this._connectionString = Resources.getConnectionString();
        }

        public DataSource(String newFilePath) {
            Resources.setDataFile(newFilePath);
            this._connectionString = Resources.getConnectionString();
            initDataFile();
        }

        private void initDataFile() {
            String fileName = Resources.getDataFilePath();

            if (fileName == null) {
                throw new FileNotFoundException("Data file path is empty");
            }

            if (File.Exists(fileName)) {
                return;
            }

            String directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            SQLiteConnection.CreateFile(fileName);
            DataFileUtil.createTables();
            insertDefaults();
            
            Options.set("RegistrationView", "groupped");
            Options.set("RegistrationSortColumn", "0");
            Options.set("RegistrationSortOrder", "0");
            Options.set("ValidateAgeGroup", "true");
        }

        private void insertDefaults() {
            AgeGroupDao ageGroupDao = new AgeGroupDao();
            CategoryDao categoryDao = new CategoryDao();
            ClassDao classDao = new ClassDao();
            PublisherDao publisherDao = new PublisherDao();
            ScaleDao scaleDao = new ScaleDao();

            //Standard age groups
            ageGroupDao.add("Młodzik", 12, -1);
            ageGroupDao.add("Junior", 17, -1);
            ageGroupDao.add("Senior", 150, -1);

            //Standard classes
            classDao.add("Standard");
            classDao.add("Waloryzowane (Open)");

            //Naviga
            int classId = classDao.add("Naviga");
            ageGroupDao.add("Młodzik", 13, classId);
            ageGroupDao.add("Senior", 150, classId);

            //Categories
            int i = 0;
            //Standard
            categoryDao.add("MSO", "Statki i Okręty", "Standard", i++);
            categoryDao.add("MOP", "Okręty podwodne", "Standard", i++);
            categoryDao.add("MŻ", "Żaglowce", "Standard", i++);
            categoryDao.add("S1A", "Samoloty jednosilnikowe śmigłowe do 1930 roku", "Standard", i++);
            categoryDao.add("S1B", "Samoloty jednosilnikowe śmigłowe po 1930 roku", "Standard", i++);
            categoryDao.add("S2", "Samoloty wielosilnikowe śmigłowe", "Standard", i++);
            categoryDao.add("S3", "Samoloty odrzutowe", "Standard", i++);
            categoryDao.add("S4", "Samoloty w skali 1:48; 1:50", "Standard", i++);
            categoryDao.add("S5", "Szybowce", "Standard", i++);
            categoryDao.add("S6", "Śmigłowce", "Standard", i++);
            categoryDao.add("MPKC", "Pojazdy kołowe cywilne", "Standard", i++);
            categoryDao.add("MPKW", "Pojazdy kołowe wojskowe", "Standard", i++);
            categoryDao.add("MPSZ", "Pojazdy szynowe", "Standard", i++);
            categoryDao.add("MPG", "Pojazdy gąsienicowe", "Standard", i++);
            categoryDao.add("MB", "Budowle", "Standard", i++);
            categoryDao.add("MF", "Figurki", "Standard", i++);
            categoryDao.add("MR", "Rakiety, statki kosmiczne, S-F", "Standard", i++);
            categoryDao.add("MA", "Artyleria", "Standard", i++);
            //Open
            categoryDao.add("WSO", "Statki i okręty", "Waloryzowane (Open)", i++);
            categoryDao.add("WŻ", "Żaglowce", "Waloryzowane (Open)", i++);
            categoryDao.add("WOP", "Okręty podwodne", "Waloryzowane (Open)", i++);
            categoryDao.add("WKC", "Pojazdy kołowe cywilne", "Waloryzowane (Open)", i++);
            categoryDao.add("WKW", "Pojazdy kołowe wojskowe", "Waloryzowane (Open)", i++);
            categoryDao.add("WSZ", "Pojazdy szynowe", "Waloryzowane (Open)", i++);
            categoryDao.add("WG", "Pojazdy gąsienicowe", "Waloryzowane (Open)", i++);
            categoryDao.add("WS1", "Samoloty śmigłowe + śmigłowce (bez podziału na napęd)", "Waloryzowane (Open)", i++);
            categoryDao.add("WS2", "Samoloty odrzutowe", "Waloryzowane (Open)", i++);
            categoryDao.add("WPP", "Modele projektowane od podstaw", "Waloryzowane (Open)", i++);
            categoryDao.add("WD", "Dioramy i makiety", "Waloryzowane (Open)", i++);
            //Naviga
            categoryDao.add("C-1", "Modele jednostek wiosłowych i żaglowych", "Naviga", i++);
            categoryDao.add("C-2", "Modele jednostek o napędzie mechanicznym", "Naviga", i++);
            categoryDao.add("C-3", "Modele urządzeń, itp.", "Naviga", i++);
            categoryDao.add("C-4", "Modele miniaturowe", "Naviga", i++);
            categoryDao.add("C-5", "Modele z butelkach", "Naviga", i++);
            categoryDao.add("C-6", "Modele plastikowe", "Naviga", i++);
            categoryDao.add("C-7", "Modele kartonowe i papierowe", "Naviga", i++);

            //Publishers
            String[] defaults = new String[] { "GPM", "Answer", "WAK", "Kartonowa Kolekcja", "Mały Modelarz", "JSC", "Modelik", "Orlik MK", "Haliński", "Model-Kom", "Orel", "WMC", "Dom Bumagi", "Draf Model", "Szkutnik", "Wektor", "Sklej Model", "Hobby Model", "AJ Model", "Model Hobby", "Quest", "Internet", "Świat z Kartonu", "Inne" };
            Array.Sort(defaults, StringComparer.InvariantCulture);
            foreach(String publisher in defaults) {
                publisherDao.add(publisher);
            }

            //Scales
            defaults = new String[] { "1:9", "1:16", "1:25", "1:33", "1:43", "1:48", "1:50", "1:100", "1:150", "1:200", "1:250", "1:300", "1:400", "1:500", "Inna" };
            i = 0;
            foreach(String scale in defaults) {
                scaleDao.add(scale);
            }
        }

        public void export(String outputFile, bool use2003Format) {
            StringBuilder output = new StringBuilder();
            output.AppendLine(@"""Nr Modelu"",""Dodane"",""Email"",""Imię"",""Nazwisko"",""Klub"",""Grupa Wiekowa"",""Nazwa Modelu"",""Klasa Modelu"",""Skala"",""Wydawnictwo"",""Kategoria"",""Rok Ur.""");

            using (SQLiteConnection cn = new SQLiteConnection(_connectionString))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText =
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(YearOfBirth,0) AS YearOfBirth 
                    FROM Registration ORDER BY EntryId";

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    if (use2003Format) {
                        while (dr.Read()) {
                            output.AppendLine(
                                String.Join(",",
                                    new string[] 
                                {
                                    wrapCsvValue2003(dr["EntryId"].ToString()),
                                    wrapCsvValue2003(dr.GetDateTime(dr.GetOrdinal("TmStamp")).ToString(Resources.DateFormat)),
                                    wrapCsvValue2003(dr["Email"].ToString()),
                                    wrapCsvValue2003(dr["FirstName"].ToString()),
                                    wrapCsvValue2003(dr["LastName"].ToString()),
                                    wrapCsvValue2003(dr["ClubName"].ToString()),
                                    wrapCsvValue2003(dr["AgeGroup"].ToString()),
                                    wrapCsvValue2003(dr["ModelName"].ToString()),
                                    wrapCsvValue2003(dr["ModelClass"].ToString()),
                                    wrapCsvValue2003(dr["ModelScale"].ToString()),
                                    wrapCsvValue2003(dr["ModelPublisher"].ToString()),
                                    wrapCsvValue2003(dr["ModelCategory"].ToString()),
                                    wrapCsvValue2003(dr["YearOfBirth"].ToString())
                                }));
                        }
                    }
                    else {
                        while (dr.Read()) {
                            output.AppendLine(
                                String.Join(",",
                                    new string[] 
                                {
                                    wrapCsvValue2013(dr["EntryId"].ToString()),
                                    wrapCsvValue2013(dr.GetDateTime(dr.GetOrdinal("TmStamp")).ToString(Resources.DateFormat)),
                                    wrapCsvValue2013(dr["Email"].ToString()),
                                    wrapCsvValue2013(dr["FirstName"].ToString()),
                                    wrapCsvValue2013(dr["LastName"].ToString()),
                                    wrapCsvValue2013(dr["ClubName"].ToString()),
                                    wrapCsvValue2013(dr["AgeGroup"].ToString()),
                                    wrapCsvValue2013(dr["ModelName"].ToString()),
                                    wrapCsvValue2013(dr["ModelClass"].ToString()),
                                    wrapCsvValue2013(dr["ModelScale"].ToString()),
                                    wrapCsvValue2013(dr["ModelPublisher"].ToString()),
                                    wrapCsvValue2013(dr["ModelCategory"].ToString()),
                                    wrapCsvValue2013(dr["YearOfBirth"].ToString())
                                }));
                        }
                    }
                }
            }

            File.WriteAllText(outputFile, output.ToString(), Encoding.UTF8);
        }

        private string wrapCsvValue2013(string value) {
            return String.Format("\"=\"\"{0}\"\"\"", value);
        }

        private string wrapCsvValue2003(string value) {
            return String.Format("\"{0}\"", value);
        }

        public void dropRegistrationRecords() {
            using (SQLiteConnection cn = new SQLiteConnection(_connectionString))
            using(SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Results; DELETE FROM Registration; DELETE FROM Models; DELETE FROM Modelers;", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }

        public float getFileVersion() {
            using(SQLiteConnection cn = new SQLiteConnection(_connectionString))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT COUNT(name) AS cnt FROM sqlite_master WHERE type='table' AND name='Version'", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                int cnt = (int)cm.ExecuteScalar();
                if(cnt == 0) {
                    return 0.931F;
                }

                cm.CommandText = "SELECT MAX(Version) FROM Version";
                return (float)cm.ExecuteScalar();
            }
        }

        public void upgradeDataFile() {

            float ver = getFileVersion();
            
            if(ver < .941) {

                //Generate backup file name
                string dataFileName = Resources.getDataFilePath();
                string backupFileName = null;

                if(!File.Exists(dataFileName.Replace(".sqlite", ".BACKUP.sqlite"))) {
                    backupFileName = dataFileName.Replace(".sqlite", ".BACKUP.sqlite");
                }
                else {
                    int i = 1;
                    do {
                        backupFileName = dataFileName.Replace(".sqlite", string.Format(".BACKUP{0}.sqlite", i));
                    } while(File.Exists(backupFileName));
                }

                //Create backup file
                File.Copy(dataFileName, backupFileName, false);

                //Populate it
                DataFileUtil.convertTo941();
            }
        }       
    }
}
