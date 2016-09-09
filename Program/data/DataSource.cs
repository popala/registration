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
using Rejestracja.Data;
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
using System.Windows.Forms;

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
                CategoryDao.createTable();
                ClassDao.createTable();
                ScaleDao.createTable();
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

        private List<RegistrationEntry> parseCSVFile(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile, bool addScale, bool addPublisher, out int badRecordCount) {

            List<RegistrationEntry> ret = new List<RegistrationEntry>();

            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;
            List<String> publishers = PublisherDao.getSimpleList().ToList();
            List<Category> modelCategories = CategoryDao.getList().ToList();
            List<AgeGroup> ageGroups = AgeGroupDao.getList().ToList();
            List<String> modelClasses = ClassDao.getSimpleList().ToList();
            List<String> modelScales = ScaleDao.getSimpleList().ToList();

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
                    List<Category> modelCategory = new List<Category>();
                    int age = 0;
                    int yearOfBirth = 0;

                    String[] parsedEntry = parser.ReadFields();

                    try {
                        //Timestamp
                        if (fieldMap.TimeStamp > -1) {
                            if (!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp], out newRegistration.registration.timeStamp)) {
                                if (!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp].Substring(0, parsedEntry[fieldMap.TimeStamp].LastIndexOf(' ') + 1), out newRegistration.registration.timeStamp)) {
                                    newRegistration.registration.timeStamp = DateTime.Now;
                                }
                            }
                        }
                        else {
                            newRegistration.registration.timeStamp = DateTime.Now;
                        }

                        //Email
                        if (fieldMap.Email > -1) {
                            newRegistration.modeler.email = parsedEntry[fieldMap.Email].Trim().ToLower();
                        }

                        //FirstName
                        newRegistration.modeler.firstName = textInfo.ToTitleCase(parsedEntry[fieldMap.FirstName].Trim());

                        //LastName
                        newRegistration.modeler.lastName = textInfo.ToTitleCase(parsedEntry[fieldMap.LastName].Trim());

                        //ClubName
                        if (fieldMap.ClubName > -1) {
                            newRegistration.modeler.clubName = parsedEntry[fieldMap.ClubName].Trim();
                        }

                        //YearOfBirth AND AgeGroup
                        if (int.TryParse(parsedEntry[fieldMap.YearOfBirth], out yearOfBirth)) {
                            newRegistration.modeler.yearOfBirth = yearOfBirth;
                            if (fieldMap.CalculateAgeGroup) {
                                age = DateTime.Now.Year - yearOfBirth;

                                AgeGroup[] ag = ageGroups.Where(x => x.bottomAge <= age && x.upperAge >= age).ToArray<AgeGroup>();
                                if (ag.Length == 1) {
                                    newRegistration.registration.ageGroupName = ag[0].name;
                                }
                            }
                            else if (fieldMap.AgeGroup > -1) {
                                newRegistration.registration.ageGroupName = parsedEntry[fieldMap.AgeGroup];
                            }
                        }

                        //ModelName
                        newRegistration.model.name = parsedEntry[fieldMap.ModelName].Trim();

                        ////ModelCategory
                        ////Pick the first field with value
                        //String enteredModelCategory = null;
                        //foreach (int i in fieldMap.ModelCategory) {
                        //    if (!String.IsNullOrWhiteSpace(parsedEntry[i])) {
                        //        enteredModelCategory = parsedEntry[i];
                        //        break;
                        //    }
                        //}

                        //Category[] matchedModelCategory = null;
                        //if (enteredModelCategory != null) {
                        //    //Try to match model category
                        //    matchedModelCategory = modelCategories.Where(x => x.fullName.ToLower().Equals(enteredModelCategory.ToLower())).ToArray();
                        //    if (matchedModelCategory.Length == 0) {
                        //        matchedModelCategory = modelCategories.Where(
                        //            x => enteredModelCategory.ToLower().Contains("(" + x.code.ToLower() + ")") ||
                        //                enteredModelCategory.ToLower().StartsWith(x.code.ToLower() + " ") ||
                        //                enteredModelCategory.ToLower().EndsWith(" " + x.code.ToLower()) ||
                        //                enteredModelCategory.ToLower().Equals(x.code.ToLower())
                        //                ).ToArray();
                        //    }
                        //    if (matchedModelCategory.Length > 0) {
                        //        newRegistration.category.name = matchedModelCategory[0].fullName;
                        //        newRegistration.category.id = matchedModelCategory[0].id;
                        //        //Matched model category and so set the category if it should be derived from class
                        //        if (fieldMap.DeriveClassFromCategory) {
                        //            newRegistration.category.className = matchedModelCategory[0].className;
                        //        }
                        //    }
                        //    else {
                        //        //Did not match model class so see if we have category field mapped as well
                        //        newRegistration.category.name = enteredModelCategory;
                        //        if (fieldMap.ModelClass > -1) {
                        //            newRegistration.category.className = parsedEntry[fieldMap.ModelClass];
                        //        }
                        //        else {
                        //            newRegistration.category.className = modelClasses[0];
                        //        }
                        //    }
                        //}

                        //ModelCategory: first will be a list fields to pick the first value from, possibly after that fields
                        //  that if present require cloning the registration entry assigned to that second category
                        foreach(int[] fieldList in fieldMap.ModelCategory) {

                            //Pick the first field with value
                            String enteredModelCategory = null;
                            foreach(int i in fieldList) {
                                if(!String.IsNullOrWhiteSpace(parsedEntry[i])) {
                                    enteredModelCategory = parsedEntry[i];
                                    break;
                                }
                            }

                            Category[] matchedModelCategory = null;
                            if(enteredModelCategory != null) {
                                //Try to match model category
                                matchedModelCategory = modelCategories.Where(x => x.fullName.ToLower().Equals(enteredModelCategory.ToLower())).ToArray();
                                if(matchedModelCategory.Length == 0) {
                                    matchedModelCategory = modelCategories.Where(
                                        x => enteredModelCategory.ToLower().Contains("(" + x.code.ToLower() + ")") ||
                                            enteredModelCategory.ToLower().StartsWith(x.code.ToLower() + " ") ||
                                            enteredModelCategory.ToLower().EndsWith(" " + x.code.ToLower()) ||
                                            enteredModelCategory.ToLower().Equals(x.code.ToLower())
                                            ).ToArray();
                                }
                                //Category found so populate list
                                if(matchedModelCategory.Length > 0) {
                                    modelCategory.Add(matchedModelCategory[0]);
                                }
                                else {
                                    //Did not match model category so add it with emtpy class
                                    modelCategory.Add(new Category() { name = enteredModelCategory });
                                }
                            }
                        }

                        //ModelScale
                        newRegistration.model.scale = Scale.parse(parsedEntry[fieldMap.ModelScale].Trim());
                        if(String.IsNullOrWhiteSpace(newRegistration.model.scale)) {
                            newRegistration.model.scale = "Inna";
                        }
                        else if (addScale) {
                            String[] scale = modelScales.Where(x => x.ToLower().Equals(newRegistration.model.scale.ToLower())).ToArray<String>();
                            if (scale.Length == 0) {
                                ScaleDao.add(newRegistration.model.scale, ScaleDao.getNextSortFlag());
                                modelScales = ScaleDao.getSimpleList().ToList();
                            }
                        }

                        //ModelClass
                        //Only populate if it should not be derived from model category
                        if (!fieldMap.DeriveClassFromCategory) {
                            String[] cat = modelClasses.Where(x => x.ToLower().Equals(parsedEntry[fieldMap.ModelClass].ToLower())).ToArray<String>();
                            if (cat.Length == 1) {
                                newRegistration.category.className = cat[0];
                            }
                            else {
                                newRegistration.category.className = parsedEntry[fieldMap.ModelClass].Trim();
                            }
                        }

                        //Publisher
                        if (fieldMap.ModelPublisher > -1) {
                            String parsedPublisher = parsedEntry[fieldMap.ModelPublisher].Trim();
                            String[] pub = publishers.Where(x => x.Equals(parsedPublisher, StringComparison.CurrentCultureIgnoreCase)).ToArray<String>();
                            
                            //halinski vs. haliński
                            if(pub.Length == 0) {
                                pub = publishers.Where(x => removeAccent(x).Equals(removeAccent(parsedPublisher), StringComparison.CurrentCultureIgnoreCase)).ToArray<String>();
                            }
                            
                            if(pub.Length > 0) {
                                newRegistration.model.publisher = pub[0];
                            }
                            else if(addPublisher) {
                                PublisherDao.add(parsedPublisher);
                                publishers = PublisherDao.getSimpleList().ToList<String>();
                            }
                        }
                    }
                    catch (Exception) {
                        newRegistration = null;
                        badRecordCount++;
                        badEntries.AppendLine(String.Join(",", parsedEntry));
                    }

                    if (newRegistration != null) {
                        MessageBox.Show("TODO: Fix Import!");
                        //ret.Add(
                        //    new RegistrationEntry(
                        //        newRegistration.timeStamp,
                        //        newRegistration.email,
                        //        newRegistration.firstName,
                        //        newRegistration.lastName,
                        //        newRegistration.clubName,
                        //        newRegistration.ageGroupName,
                        //        newRegistration.modelName,
                        //        newRegistration.className,
                        //        newRegistration.modelScale,
                        //        newRegistration.modelPublisher,
                        //        newRegistration.categoryName,
                        //        newRegistration.categoryId,
                        //        yearOfBirth
                        //    )
                        //);
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

        public int bulkLoadRegistration(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile, bool addScale, bool addPublisher) {

            int badRecordCountParsing = 0;
            int badRecordCountLoading = 0;
            IEnumerable<RegistrationEntry> entries = parseCSVFile(filePath, fieldMap, hasHeaders, badRecordFile, addScale, addPublisher, out badRecordCountParsing);
            Category[] categories = CategoryDao.getList().ToArray();
            Category[] matchedCategory = null;
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

                        matchedCategory = categories.Where(x => x.fullName.ToLower().Equals(entry.category.name.ToLower())).ToArray();
                        if (matchedCategory.Length > 0) {
                            cm.Parameters["@ModelCategoryId"].Value = matchedCategory[0].id;
                        }
                        else {
                            cm.Parameters["@ModelCategoryId"].Value = -1;
                        }

                        //cm.Parameters["@TmStamp"].Value = entry.timeStamp;
                        //cm.Parameters["@Email"].Value = entry.email;
                        //cm.Parameters["@FirstName"].Value = entry.firstName;
                        //cm.Parameters["@LastName"].Value = entry.lastName;
                        //cm.Parameters["@ClubName"].Value = entry.clubName;
                        //cm.Parameters["@AgeGroup"].Value = entry.ageGroupName;
                        //cm.Parameters["@ModelName"].Value = entry.modelName;
                        //cm.Parameters["@ModelClass"].Value = entry.className;
                        //cm.Parameters["@ModelScale"].Value = entry.modelScale;
                        //cm.Parameters["@ModelPublisher"].Value = entry.modelPublisher;
                        //cm.Parameters["@ModelCategory"].Value = entry.category.name;
                        //cm.Parameters["@YearOfBirth"].Value = entry.yearOfBirth;
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
                DataFileConversion.convertTo941();
            }
        }       
    }
}
