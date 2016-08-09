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
using Microsoft.VisualBasic.FileIO;
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
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
            createTables(true);
            
            Options.set("RegistrationView", "groupped");
            Options.set("RegistrationSortColumn", "0");
            Options.set("RegistrationSortOrder", "0");
            Options.set("ValidateAgeGroup", "true");
        }

        private void createTables(bool createAllTables) {
            RegistrationEntryDao.createTable();
            if (createAllTables) {
                AgeGroupDao.createTable();
                AwardDao.createTable();
                ModelCategoryDao.createTable();
                ModelClassDao.createTable();
                ModelScaleDao.createTable();
                PublisherDao.createTable();
                Options.createTable();                    
            }
            ResultDao.createTable();
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

        private List<RegistrationEntry> parseCSVFile(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile, out int badRecordCount) {

            List<RegistrationEntry> ret = new List<RegistrationEntry>();

            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;
            List<String> publishers = PublisherDao.getSimpleList().ToList<String>();
            List<ModelCategory> modelCategories = ModelCategoryDao.getList().ToList<ModelCategory>();
            List<AgeGroup> ageGroups = AgeGroupDao.getList().ToList<AgeGroup>();
            List<String> modelClasses = ModelClassDao.getSimpleList().ToList<String>();

            badRecordCount = 0;

            using (TextFieldParser parser = new TextFieldParser(filePath)) {
                parser.CommentTokens = new String[] { "#" };
                parser.SetDelimiters(new String[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;
                StringBuilder badEntries = new StringBuilder();

                //Skip headers
                if(hasHeaders)
                    parser.ReadLine();

                while (!parser.EndOfData) {
                    
                    RegistrationEntry newRegistration = new RegistrationEntry();
                    int age = 0;
                    int yearOfBirth = 0;

                    String[] parsedEntry = parser.ReadFields();

                    try {
                        //Timestamp
                        if (fieldMap.TimeStamp > -1) {
                            if (!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp], out newRegistration.timeStamp)) {
                                if (!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp].Substring(0, parsedEntry[fieldMap.TimeStamp].LastIndexOf(' ') + 1), out newRegistration.timeStamp)) {
                                    newRegistration.timeStamp = DateTime.Now;
                                }
                            }
                        }
                        else {
                            newRegistration.timeStamp = DateTime.Now;
                        }

                        //Email
                        if (fieldMap.Email > -1) {
                            newRegistration.email = parsedEntry[fieldMap.Email].Trim();
                        }

                        //FirstName
                        newRegistration.firstName = textInfo.ToTitleCase(parsedEntry[fieldMap.FirstName].Trim());

                        //LastName
                        newRegistration.lastName = textInfo.ToTitleCase(parsedEntry[fieldMap.LastName].Trim());

                        //ClubName
                        if (fieldMap.ClubName > -1) {
                            newRegistration.clubName = parsedEntry[fieldMap.ClubName].Trim();
                        }

                        //YearOfBirth AND AgeGroup
                        if (int.TryParse(parsedEntry[fieldMap.YearOfBirth], out yearOfBirth)) {
                            newRegistration.yearOfBirth = yearOfBirth;
                            if (fieldMap.CalculateAgeGroup) {
                                age = DateTime.Now.Year - yearOfBirth;

                                AgeGroup[] ag = ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray<AgeGroup>();
                                if (ag.Length == 1) {
                                    newRegistration.ageGroup = ag[0].name;
                                }
                            }
                            else if (fieldMap.AgeGroup > -1) {
                                newRegistration.ageGroup = parsedEntry[fieldMap.AgeGroup];
                            }
                        }

                        //ModelName
                        newRegistration.modelName = parsedEntry[fieldMap.ModelName].Trim();

                        //ModelCategory
                        //Pick the first field with value
                        String enteredModelCategory = null;
                        foreach (int i in fieldMap.ModelCategory) {
                            if (!String.IsNullOrWhiteSpace(parsedEntry[i])) {
                                enteredModelCategory = parsedEntry[i];
                                break;
                            }
                        }

                        ModelCategory[] matchedModelCategory = null;
                        if (enteredModelCategory != null) {
                            //Try to match model category
                            matchedModelCategory = modelCategories.Where(x => x.fullName.ToLower().Equals(enteredModelCategory.ToLower())).ToArray();
                            if (matchedModelCategory.Length == 0) {
                                matchedModelCategory = modelCategories.Where(x => enteredModelCategory.ToLower().Contains("(" + x.code + ")")).ToArray();
                            }
                            if (matchedModelCategory.Length > 0) {
                                newRegistration.modelCategory = matchedModelCategory[0].fullName;
                                newRegistration.modelCategoryId = matchedModelCategory[0].id;
                                //Matched model category and so set the category if it should be derived from class
                                if (fieldMap.DeriveClassFromCategory) {
                                    newRegistration.modelClass = matchedModelCategory[0].modelClass;
                                }
                            }
                            else {
                                //Did not match model class so see if we have category field mapped as well
                                newRegistration.modelCategory = enteredModelCategory;
                                if (fieldMap.ModelClass > -1) {
                                    newRegistration.modelClass = parsedEntry[fieldMap.ModelClass];
                                }
                                else {
                                    newRegistration.modelClass = modelClasses[0];
                                }
                            }
                        }

                        //ModelScale
                        newRegistration.modelScale = parsedEntry[fieldMap.ModelScale].Trim();

                        //ModelClass
                        //Only populate if it should not be derived from model category
                        if (!fieldMap.DeriveClassFromCategory) {
                            String[] cat = modelClasses.Where(x => x.ToLower().Equals(parsedEntry[fieldMap.ModelClass].ToLower())).ToArray<String>();
                            if (cat.Length == 1) {
                                newRegistration.modelClass = cat[0];
                            }
                            else {
                                newRegistration.modelClass = parsedEntry[fieldMap.ModelClass].Trim();
                            }
                        }

                        //Publisher
                        if (fieldMap.ModelPublisher > -1) {
                            String[] pub = publishers.Where(x => x.ToLower().Equals(parsedEntry[fieldMap.ModelPublisher].ToLower())).ToArray<String>();
                            //halinski vs. haliński
                            if (pub.Length == 0)
                                pub = publishers.Where(x => removeAccent(x.ToLower()).Equals(removeAccent(parsedEntry[fieldMap.ModelPublisher].ToLower()))).ToArray<String>();
                            if (pub.Length > 0) {
                                newRegistration.modelPublisher = pub[0].Trim();
                            }
                        }
                    }
                    catch (Exception) {
                        newRegistration = null;
                        badRecordCount++;
                        badEntries.AppendLine(String.Join(",", parsedEntry));
                    }

                    if (newRegistration != null) {
                        ret.Add(
                            new RegistrationEntry(
                                newRegistration.timeStamp,
                                newRegistration.email,
                                newRegistration.firstName,
                                newRegistration.lastName,
                                newRegistration.clubName,
                                newRegistration.ageGroup,
                                newRegistration.modelName,
                                newRegistration.modelClass,
                                newRegistration.modelScale,
                                newRegistration.modelPublisher,
                                newRegistration.modelCategory,
                                newRegistration.modelCategoryId,
                                yearOfBirth
                            )
                        );
                    }
                    if (badRecordCount > 0 && badRecordFile != null) {
                        File.WriteAllText(badRecordFile, badEntries.ToString());
                    }
                }
            }
            return ret;
        }

        public static List<String[]> getFileSample(String filePath, String commentToken, String delimiter, bool valuesInQuotes, int lineCount) {

            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;
            List<String[]> ret = new List<String[]>();

            if(lineCount < 1) {
                return ret;
            }

            using (TextFieldParser parser = new TextFieldParser(filePath)) {
                parser.CommentTokens = new String[] { commentToken };
                parser.SetDelimiters(new String[] { delimiter });
                parser.HasFieldsEnclosedInQuotes = valuesInQuotes;

                int i = 0;
                while (!parser.EndOfData && i < lineCount) {
                    ret.Add(parser.ReadFields());
                    i++;
                }

                return ret;
            }
        }

        public int bulkLoadRegistration(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile) {

            int badRecordCountParsing = 0;
            int badRecordCountLoading = 0;
            IEnumerable<RegistrationEntry> entries = parseCSVFile(filePath, fieldMap, hasHeaders, badRecordFile, out badRecordCountParsing);
            ModelCategory[] categories = ModelCategoryDao.getList().ToArray();
            ModelCategory[] matchedCategory = null;
            StringBuilder sb = new StringBuilder();

            using (SQLiteConnection cn = new SQLiteConnection(_connectionString))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Registration(TmStamp, Email, FirstName, LastName, ClubName, AgeGroup, ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, ModelCategoryId, YearOfBirth)
                    VALUES(@TmStamp, @Email, @FirstName, @LastName, @ClubName, @AgeGroup, @ModelName, @ModelClass, @ModelScale, @ModelPublisher, @ModelCategory, @ModelCategoryId, @YearOfBirth)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add(new SQLiteParameter("@TmStamp", System.Data.DbType.DateTime));
                cm.Parameters.Add(new SQLiteParameter("@Email", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@FirstName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@LastName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ClubName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@AgeGroup", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelClass", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelScale", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelPublisher", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelCategory", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelCategoryId", System.Data.DbType.Int32));
                cm.Parameters.Add(new SQLiteParameter("@YearOfBirth", System.Data.DbType.Int32));

                using (SQLiteTransaction t = cn.BeginTransaction()) {
                    foreach (RegistrationEntry entry in entries) {

                        matchedCategory = categories.Where(x => x.fullName.ToLower().Equals(entry.modelCategory.ToLower())).ToArray();
                        if (matchedCategory.Length > 0) {
                            cm.Parameters["@ModelCategoryId"].Value = matchedCategory[0].id;
                        }
                        else {
                            cm.Parameters["@ModelCategoryId"].Value = -1;
                        }

                        cm.Parameters["@TmStamp"].Value = entry.timeStamp;
                        cm.Parameters["@Email"].Value = entry.email;
                        cm.Parameters["@FirstName"].Value = entry.firstName;
                        cm.Parameters["@LastName"].Value = entry.lastName;
                        cm.Parameters["@ClubName"].Value = entry.clubName;
                        cm.Parameters["@AgeGroup"].Value = entry.ageGroup;
                        cm.Parameters["@ModelName"].Value = entry.modelName;
                        cm.Parameters["@ModelClass"].Value = entry.modelClass;
                        cm.Parameters["@ModelScale"].Value = entry.modelScale;
                        cm.Parameters["@ModelPublisher"].Value = entry.modelPublisher;
                        cm.Parameters["@ModelCategory"].Value = entry.modelCategory;
                        cm.Parameters["@YearOfBirth"].Value = entry.yearOfBirth;
                        try {
                            cm.ExecuteNonQuery();
                        }
                        catch (Exception e) {
                            LogWriter.error(e);
                            badRecordCountLoading++;
                            sb.AppendLine(entry.ToCsvString());
                        }
                    }
                    t.Commit();
                }
            }
            if (badRecordCountLoading > 0) {
                if (badRecordCountParsing > 0) {
                    sb.AppendLine();
                }
                File.AppendAllText(badRecordFile, sb.ToString());
            }
            return badRecordCountParsing + badRecordCountLoading;
        }

        private string removeAccent(String input) {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(input);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public void dropRegistrationRecords() {
            using (SQLiteConnection cn = new SQLiteConnection(_connectionString))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Results", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = @"DROP TABLE Results";
                cm.ExecuteNonQuery();

                cm.CommandText = @"DROP TABLE Registration";
                cm.ExecuteNonQuery();
            }

            createTables(false);
        }
    }
}
