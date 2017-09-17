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
using System.Threading.Tasks;

namespace Rejestracja.Data {
    class ImportUtil {

        private static List<RegistrationEntry> parseCSVFile(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile, bool addScale, bool addPublisher, out int badRecordCount) {

            List<RegistrationEntry> ret = new List<RegistrationEntry>();

            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;
            List<String> publishers = PublisherDao.getSimpleList().ToList();
            List<Category> modelCategories = CategoryDao.getList().ToList();
            List<AgeGroup> standardAgeGroups = new AgeGroupDao().getList(-1);
            //List<String> modelClasses = ClassDao.getSimpleList().ToList();
            List<String> modelScales = ScaleDao.getList().Select(x => x.name).ToList();
            List<Class> modelClasses = ClassDao.getList(true, true);

            badRecordCount = 0;

            using(TextFieldParser parser = new TextFieldParser(filePath)) {
                parser.CommentTokens = new String[] { "#" };
                parser.SetDelimiters(new String[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;
                StringBuilder badEntries = new StringBuilder();

                //Skip headers
                if(hasHeaders)
                    parser.ReadLine();

                while(!parser.EndOfData) {

                    RegistrationEntry newRegistration = new RegistrationEntry();
                    int age = 0;
                    int yearOfBirth = 0;

                    String[] parsedEntry = parser.ReadFields();

                    try {
                        //Timestamp
                        if(fieldMap.TimeStamp > -1) {
                            if(!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp], out newRegistration.registration.timeStamp)) {
                                if(!DateTime.TryParse(parsedEntry[fieldMap.TimeStamp].Substring(0, parsedEntry[fieldMap.TimeStamp].LastIndexOf(' ') + 1), out newRegistration.registration.timeStamp)) {
                                    newRegistration.registration.timeStamp = DateTime.Now;
                                }
                            }
                        }
                        else {
                            newRegistration.registration.timeStamp = DateTime.Now;
                        }

                        //Email
                        if(fieldMap.Email > -1) {
                            newRegistration.modeler.email = parsedEntry[fieldMap.Email].Trim().ToLower();
                        }

                        //FirstName
                        newRegistration.modeler.firstName = textInfo.ToTitleCase(parsedEntry[fieldMap.FirstName].Trim());

                        //LastName
                        newRegistration.modeler.lastName = textInfo.ToTitleCase(parsedEntry[fieldMap.LastName].Trim());

                        //ClubName
                        if(fieldMap.ClubName > -1) {
                            newRegistration.modeler.clubName = parsedEntry[fieldMap.ClubName].Trim();
                        }

                        //YearOfBirth AND AgeGroup
                        if(int.TryParse(parsedEntry[fieldMap.YearOfBirth], out yearOfBirth)) {
                            newRegistration.modeler.yearOfBirth = yearOfBirth;
                            age = DateTime.Now.Year - yearOfBirth;
                        }

                        //ModelName
                        newRegistration.model.name = parsedEntry[fieldMap.ModelName].Trim();

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
                            
                            if(enteredModelCategory != null) {
                                //Try to match model category
                                Category[] matchedModelCategory = null;
                                matchedModelCategory = modelCategories.Where(x => x.fullName.Equals(enteredModelCategory, StringComparison.CurrentCultureIgnoreCase)).ToArray();
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
                                    Class modelClass = modelClasses.First(x => x.name.Equals(matchedModelCategory[0].className, StringComparison.CurrentCultureIgnoreCase));
                                    String ageGroupName = "Nieznana";
                                    if(age > 0) {
                                        AgeGroup [] matchedAgeGroup = modelClass.ageGroups.Where(x => age >= x.bottomAge && age <= x.upperAge).ToArray();
                                        if(matchedAgeGroup.Length > 0) {
                                            ageGroupName = matchedAgeGroup[0].name;
                                        }
                                    }
                                    newRegistration.importedRegistration.Add(new Registration(newRegistration.registration.timeStamp, newRegistration.model.id, matchedModelCategory[0].id, null, ageGroupName));
                                }
                                else {
                                    String ageGroupName = "Nieznana";
                                    if(age > 0) {
                                        AgeGroup[] matchedAgeGroup = standardAgeGroups.Where(x => age >= x.bottomAge && age <= x.upperAge).ToArray();
                                        if(matchedAgeGroup.Length > 0) {
                                            ageGroupName = matchedAgeGroup[0].name;
                                        }
                                    }
                                    //Did not match model category so add it with emtpy class
                                    newRegistration.importedRegistration.Add(new Registration(newRegistration.registration.timeStamp, newRegistration.model.id, -1, enteredModelCategory, ageGroupName));
                                }
                            }
                        }

                        //ModelScale
                        newRegistration.model.scale = Scale.parse(parsedEntry[fieldMap.ModelScale].Trim());
                        if(String.IsNullOrWhiteSpace(newRegistration.model.scale)) {
                            newRegistration.model.scale = "Inna";
                        }
                        else if(addScale) {
                            String[] scale = modelScales.Where(x => x.ToLower().Equals(newRegistration.model.scale.ToLower())).ToArray<String>();
                            if(scale.Length == 0) {
                                ScaleDao.add(newRegistration.model.scale);
                                modelScales = ScaleDao.getList().Select(x => x.name).ToList();
                            }
                        }

                        //Publisher
                        if(fieldMap.ModelPublisher > -1) {
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
                    catch(Exception) {
                        newRegistration = null;
                        badRecordCount++;
                        badEntries.AppendLine(String.Join(",", parsedEntry));
                    }

                    if(String.IsNullOrEmpty(newRegistration.modeler.lastName) && string.IsNullOrEmpty(newRegistration.modeler.email)) {
                        newRegistration = null;
                        badRecordCount++;
                        badEntries.AppendLine(String.Join(",", parsedEntry));
                    }

                    if(newRegistration != null) {
                        ret.Add(newRegistration);
                    }   
                }
                if(badRecordCount > 0 && badRecordFile != null) {
                    File.WriteAllText(badRecordFile, badEntries.ToString());
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

            using(TextFieldParser parser = new TextFieldParser(filePath)) {
                parser.CommentTokens = new String[] { commentToken };
                parser.SetDelimiters(new String[] { delimiter });
                parser.HasFieldsEnclosedInQuotes = valuesInQuotes;

                int i = 0;
                while(!parser.EndOfData && i < lineCount) {
                    ret.Add(parser.ReadFields());
                    i++;
                }

                return ret;
            }
        }

        private static void insertModelers(List<Modeler> modelers) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Modelers(FirstName, LastName, ClubName, YearOfBirth, Email)
                    VALUES(@FirstName, @LastName, @ClubName, @YearOfBirth, @Email)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add(new SQLiteParameter("@Email", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@FirstName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@LastName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ClubName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@YearOfBirth", System.Data.DbType.Int32));

                using(SQLiteTransaction t = cn.BeginTransaction()) {
                    foreach(Modeler modeler in modelers) {
                        
                        cm.Parameters["@Email"].Value = modeler.email;
                        cm.Parameters["@FirstName"].Value = modeler.firstName;
                        cm.Parameters["@LastName"].Value = modeler.lastName;
                        cm.Parameters["@ClubName"].Value = modeler.clubName;
                        cm.Parameters["@YearOfBirth"].Value = modeler.yearOfBirth;
                        try {
                            cm.ExecuteNonQuery();
                            modeler.id = (int)cn.LastInsertRowId;
                        }
                        catch(Exception e) {
                            LogWriter.error("Error importing a modeler", e);
                        }
                    }
                    t.Commit();
                }
            }
        }

        private static void insertModels(List<Model> models) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Models(Name, Publisher, Scale, ModelerId)
                    VALUES(@Name, @Publisher, @Scale, @ModelerId)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add(new SQLiteParameter("@Name", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@Publisher", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@Scale", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@ModelerId", System.Data.DbType.Int32));

                using(SQLiteTransaction t = cn.BeginTransaction()) {
                    foreach(Model model in models) {

                        cm.Parameters["@Name"].Value = model.name;
                        cm.Parameters["@Publisher"].Value = model.publisher;
                        cm.Parameters["@Scale"].Value = model.scale;
                        cm.Parameters["@ModelerId"].Value = model.modelerId;
                        try {
                            cm.ExecuteNonQuery();
                            model.id = (int)cn.LastInsertRowId;
                        }
                        catch(Exception e) {
                            LogWriter.error("Error importing a modeler", e);
                        }
                    }
                    t.Commit();
                }
            }
        }

        private static void insertRegistrationEntries(List<Registration> registrations) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Registration(TmStamp, ModelId, CategoryId, CategoryName, AgeGroupName)
                    VALUES(@TmStamp, @ModelId, @CategoryId, @CategoryName, @AgeGroupName)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add(new SQLiteParameter("@TmStamp", System.Data.DbType.DateTime));
                cm.Parameters.Add(new SQLiteParameter("@ModelId", System.Data.DbType.Int32));
                cm.Parameters.Add(new SQLiteParameter("@CategoryId", System.Data.DbType.Int32));
                cm.Parameters.Add(new SQLiteParameter("@CategoryName", System.Data.DbType.String));
                cm.Parameters.Add(new SQLiteParameter("@AgeGroupName", System.Data.DbType.String));
                

                using(SQLiteTransaction t = cn.BeginTransaction()) {
                    foreach(Registration registration in registrations) {

                        cm.Parameters["@TmStamp"].Value = registration.timeStamp;
                        cm.Parameters["@ModelId"].Value = registration.modelId;
                        cm.Parameters["@CategoryId"].Value = registration.categoryId;
                        cm.Parameters["@CategoryName"].Value = registration.categoryName;
                        cm.Parameters["@AgeGroupName"].Value = registration.ageGroupName;
                        try {
                            cm.ExecuteNonQuery();
                            registration.id = (int)cn.LastInsertRowId;
                        }
                        catch(Exception e) {
                            LogWriter.error("Error adding registration entry", e);
                        }
                    }
                    t.Commit();
                }
            }
        }

        public static int bulkLoadRegistration(String filePath, FileImportFieldMap fieldMap, bool hasHeaders, String badRecordFile, bool addScale, bool addPublisher) {

            StringBuilder badRecords = new StringBuilder();
            int badRecordCountParsing = 0;
            int badRecordCountLoading = 0;
            
            List<RegistrationEntry> entries = parseCSVFile(filePath, fieldMap, hasHeaders, badRecordFile, addScale, addPublisher, out badRecordCountParsing);
            List<Modeler> modelers = entries.Select(x => x.modeler).Distinct(new ModelerComparer()).ToList();
            
            //Insert deduped modelers to obtain modeler ID
            insertModelers(modelers);

            //Match inserted modelers to registration entries and update modeler ID
            foreach(RegistrationEntry entry in entries) {
                Modeler match = modelers.First(x => x.Matches(entry.modeler));
                entry.modeler.id = match.id;
                entry.model.modelerId = match.id;
            }

            //Insert models
            insertModels(entries.Select(x => x.model).ToList());

            foreach(RegistrationEntry entry in entries) {
                foreach(Registration registration in entry.importedRegistration) {
                    registration.modelId = entry.model.id;
                }
            }

            //Insert registration records for each category/class
            insertRegistrationEntries(entries.SelectMany(x => x.importedRegistration).ToList());

            if(badRecordCountLoading > 0) {
                if(badRecordCountParsing > 0) {
                    badRecords.AppendLine();
                }
                File.AppendAllText(badRecordFile, badRecords.ToString());
            }
            return badRecordCountParsing + badRecordCountLoading;
        }

        private static string removeAccent(String input) {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(input);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
