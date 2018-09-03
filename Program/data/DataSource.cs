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
using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Rejestracja {
    class DataSource {

        public DataSource() {
            //this._connectionString = Resources.getConnectionString();
        }

        public DataSource(String newFilePath) {
            if (String.IsNullOrEmpty(newFilePath)) {
                throw new FileNotFoundException("Data file path is empty");
            }
            Resources.setConnectionString(newFilePath, null);
            DataFileUtil.initDataFile();

            insertDefaults();

            Options.set("RegistrationView", "groupped");
            Options.set("RegistrationSortColumn", "0");
            Options.set("RegistrationSortOrder", "0");
            Options.set("ValidateAgeGroup", "true");
        }

        public DataSource(String server, String user, String password, String database, String additionalParams) {
            String connectionString = 
                String.Format("server={0};uid={1};pwd={2};{3}",
                server,
                user,
                password,
                additionalParams
            );

            using(MySqlConnection cn = new MySqlConnection(connectionString))
            using (MySqlCommand cm = new MySqlCommand("", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = String.Format("CREATE DATABASE {0} IF NOT EXISTS", database);
            }

            insertDefaults();

            Options.set("RegistrationView", "groupped");
            Options.set("RegistrationSortColumn", "0");
            Options.set("RegistrationSortOrder", "0");
            Options.set("ValidateAgeGroup", "true");
        }

        public static DbConnection getConnection() {
            String dataFile = Properties.Settings.Default.DataFile;
            if (!String.IsNullOrWhiteSpace(dataFile) && File.Exists(dataFile)) {
                return new SQLiteConnection(dataFile);
            }

            String mySqlConnectionString = Properties.Settings.Default.MySqlConnection;
            if (!String.IsNullOrWhiteSpace(mySqlConnectionString)) {
                return new MySqlConnection(mySqlConnectionString);
            }

            throw new ApplicationException("Missing or invalid database configuration");
        }

        public static DbCommand getCommand(DbConnection cn) {
            if (cn is SQLiteConnection) {
                return new SQLiteCommand((SQLiteConnection)cn);
            } else {
                return new MySqlCommand("", (MySqlConnection)cn);
            }
        }

        public static void addParam(DbCommand cm, String parameterName, String parameterValue) {
            addParam(cm, parameterName, -1);
        }

        public static void addParam(DbCommand cm, String parameterName, String parameterValue, int parameterSize) {
            DbParameter parm = cm.CreateParameter();
            parm.ParameterName = parameterName;
            parm.Value = parameterValue;
            parm.DbType = DbType.String;
            if (parameterSize > 0) {
                parm.Size = parameterSize;
            }
            cm.Parameters.Add(parm);
        }

        public static void addParam(DbCommand cm, String parameterName, int parameterValue) {
            DbParameter parm = cm.CreateParameter();
            parm.ParameterName = parameterName;
            parm.Value = parameterValue;
            parm.DbType = DbType.Int32;
            cm.Parameters.Add(parm);
        }

        public static void addParam(DbCommand cm, String parameterName, long parameterValue) {
            DbParameter parm = cm.CreateParameter();
            parm.ParameterName = parameterName;
            parm.Value = parameterValue;
            parm.DbType = DbType.Int64;
            cm.Parameters.Add(parm);
        }

        public static void addParam(DbCommand cm, String parameterName, bool parameterValue) {
            DbParameter parm = cm.CreateParameter();
            parm.ParameterName = parameterName;
            parm.Value = parameterValue;
            parm.DbType = DbType.Boolean;
            cm.Parameters.Add(parm);
        }

        public static void dropRegistrationRecords() {
            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"DELETE FROM Results; DELETE FROM Registration; DELETE FROM Models; DELETE FROM Modelers;";
                cm.ExecuteNonQuery();
            }
        }

        public static bool hasValidDataSource() {
            String dataFile = Properties.Settings.Default.DataFile;
            if (!String.IsNullOrWhiteSpace(dataFile) && File.Exists(dataFile)) {
                return true;
            }
            String mySqlConnectionString = Properties.Settings.Default.MySqlConnection;
            if (!String.IsNullOrWhiteSpace(mySqlConnectionString)) {
                return true;
            }
            return false;
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

            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText =
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(YearOfBirth,0) AS YearOfBirth 
                    FROM Registration ORDER BY EntryId";

                using (DbDataReader dr = cm.ExecuteReader()) {
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
    }
}
