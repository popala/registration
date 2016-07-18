using System;
using System.Text.RegularExpressions;
using Novacode;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace Rejestracja
{
    class DocHandler
    {
        public void generateDiploma(String templateFile, String outFile, WinningEntry entry)
        {
            String documentHeader = Options.get("DocumentHeader");

            using (DocX template = DocX.Load(templateFile))
            {
                template.ReplaceText("[Naglowek]", documentHeader, 
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Naglowek!]", documentHeader.ToUpper(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.ReplaceText("[Imie]", entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imię]", entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nazwisko]", entry.lastName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.ReplaceText("[GrupaWiekowa]", entry.ageGroup,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NazwaModelu]", entry.modelName,

                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlasaModelu]", entry.modelClass,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", entry.modelCategory,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                if (entry.place.HasValue)
                {
                    template.ReplaceText("[Miejsce]", entry.place.ToString(),
                        false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                }
                if (entry.awardId.HasValue)
                {
                    template.ReplaceText("[Nagroda]", entry.awardTitle,
                        false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                }

                template.SaveAs(outFile);
            }
        }

        public void generateRegistrationCard(String templateFile, String outFile, RegistrationEntry entry)
        {
            using (DocX template = DocX.Load(templateFile))
            {
                template.ReplaceText("[DataRejestracji]", entry.timeStamp.ToString(Resources.DateFormat),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NumerStartowy]", entry.entryId.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NS]", entry.entryId.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Email]", entry.email,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imie]", entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imię]", entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nazwisko]", entry.lastName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[L]", entry.lastName.Substring(0, 1).ToUpper(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlubModelarski]", entry.clubName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[GrupaWiekowa]", entry.ageGroup,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NazwaModelu]", entry.modelName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[SkalaModelu]", entry.modelScale,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlasaModelu]", entry.modelCategory,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Wydawnictwo]", entry.modelPublisher,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", entry.modelClass,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[RokUrodzenia]", entry.yearOfBirth.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.SaveAs(outFile);
            }
        }

        public void generateJudgingForms(String template, String outFolder, frmMain parentForm)
        {
            List<RegistrationEntry> entries = RegistrationEntry.getListForJudging();
            List<ModelCategory> categories = ModelCategory.getList().ToList();
            Dictionary<String, Char> authors = new Dictionary<String, Char>();
            
            String authorKey;
            Char authorMarker = 'A';
            String documentHeader = Options.get("DocumentHeader");

            String ageGroup = null;
            String modelClass = null;
            String modelCategory = null;
            ModelCategory [] category = null;

            String outputFileName = null;
            DocX doc = null;
            Table tbl = null;
            Row row = null;

            parentForm.resetProgressBar(entries.Count);

            try
            {
                foreach (RegistrationEntry entry in entries)
                {
                    authorKey = String.Format("{0}_{1}_{2}", entry.firstName, entry.lastName, entry.yearOfBirth);

                    if (ageGroup != entry.ageGroup || modelClass != entry.modelCategory || modelCategory != entry.modelClass)
                    {
                        if (doc != null)
                        {
                            doc.Save();
                            doc.Dispose();
                            doc = null;
                        }

                        //Match existing category
                        category = categories.Where(x => x.getFullName().ToLower().Equals(entry.modelCategory.ToLower())).ToArray();

                        outputFileName = 
                            String.Format("{0}\\{1}_{2}_{3}.docx", 
                            outFolder, 
                            entry.ageGroup.ToUpper(),
                            Resources.FileNameInvalidChars.Replace(entry.modelClass, "").ToUpper(), 
                            Resources.FileNameInvalidChars.Replace(entry.modelCategory, ""));

                        File.Copy(template, outputFileName);

                        doc = DocX.Load(outputFileName);
                        doc.ReplaceText("[Naglowek]", documentHeader, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Naglowek!]", documentHeader.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[GrupaWiekowa]", entry.ageGroup, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[GrupaWiekowa!]", entry.ageGroup.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Klasa]", entry.modelClass, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Klasa!]", entry.modelClass.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Kategoria]", entry.modelCategory, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Kategoria!]", entry.modelCategory, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        if (category.Length > 0) {
                            doc.ReplaceText("[KodKategorii]", category[0].code, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                            doc.ReplaceText("[KodKategorii!]", category[0].code.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        }
                        else {
                            doc.ReplaceText("[KodKategorii]", "---", false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                            doc.ReplaceText("[KodKategorii!]", "---", false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        }

                        tbl = doc.Tables[1];

                        ageGroup = entry.ageGroup;
                        modelClass = entry.modelCategory;
                        modelCategory = entry.modelClass;

                        row = tbl.Rows[1];

                        authors.Clear();
                        authorMarker = 'A';
                    }
                    else
                    {
                        row = tbl.InsertRow();
                    }

                    if(!authors.ContainsKey(authorKey))
                    {
                        authors.Add(authorKey, authorMarker);
                        authorMarker++;
                    }

                    row.Cells[0].Paragraphs[0].InsertText(entry.entryId.ToString());
                    row.Cells[1].Paragraphs[0].InsertText(authors[authorKey].ToString());
                    row.Cells[2].Paragraphs[0].InsertText(entry.modelName);

                    parentForm.incrementProgressBar();
                }
                if (doc != null)
                {
                    doc.Save();
                    ((IDisposable)doc).Dispose();
                    doc = null;
                }
            }
            finally
            {
                if (doc != null)
                {
                    ((IDisposable)doc).Dispose();
                }
            }
            Process.Start(outFolder);
        }

        public void generateHtmlResults(String template, String outputFileName)
        {
            //DataSource ds = new DataSource();
            String htmlTemplate;

            String[] headers = "L.p.,Imię i Nazwisko,Nr i Nazwa Modelu,MIEJSCE".Split(',');

            String ageGroup = null;
            String modelClass = null;
            String modelCategory = null;

            //Prep header or use a template with header
            if (String.IsNullOrWhiteSpace(template))
            {
                htmlTemplate = 
@"<html>
    <head>
        <META HTTP-EQUIV=""Content-type"" CONTENT=""text/html; charset=UTF-8"">
        <title>Wyniki Konkursu</title>
    </head>
    <body>
        [KATEGORIE]
        <br/>
        [NAGRODY]
    </body>
</html>";
            }
            else
            {
                htmlTemplate = File.OpenText(template).ReadToEnd();
            }

            //Category winners
            IEnumerable<WinningEntry> entries = Results.getCategoryResults();
            StringBuilder results = new StringBuilder();
            int lpCounter = 1;

            foreach (WinningEntry entry in entries)
            {
                if (ageGroup != entry.ageGroup || modelCategory != entry.modelCategory)
                {
                    if(ageGroup != null)
                    {
                        results.AppendLine(@"</table>");
                    }
                    ageGroup = entry.ageGroup;
                    modelCategory = entry.modelCategory;
                    modelClass = "";

                    results.AppendFormat(@"<h1>Grupa Wiekowa <span class=""ageGroup"">{0}</span> - <span class=""modelCategory"">{1}</span></h1>", entry.ageGroup.ToUpper(), entry.modelCategory).AppendLine();
                }
                if (modelClass != entry.modelClass)
                {
                    lpCounter = 1;

                    if(!String.IsNullOrWhiteSpace(modelClass))
                    {
                        results.AppendLine(@"</table>");
                    }

                    modelClass = entry.modelClass;
                    results.AppendFormat(@"<h2>{0}</h2>", entry.modelClass).AppendLine();
                    results.AppendLine(@"<table class=""category"">");
                    //Insert headers
                    results.AppendFormat(@"<tr><th class=""lp"">{0}</th><th class=""name"">{1}</th><th class=""modelName"" colspan=""2"">{2}</th><th class=""place"">{3}</th></tr>", headers).AppendLine();
                }
                results.AppendFormat(@"<tr><td class=""lp"">{0}</td><td class=""name"">{1} {2}</td><td>{3}</td><td class=""modelName"">{4}</td><td class=""place"">{5}</td></tr>", lpCounter, entry.firstName, entry.lastName, entry.entryId, entry.modelName, entry.place).AppendLine();
                lpCounter++;
            }
            if(ageGroup != null)
            {
                results.AppendLine(@"</table>");
            }

            htmlTemplate = htmlTemplate.Replace("[KATEGORIE]", results.ToString());
            results.Clear();


            //SPECIAL AWARDS
            long awardId = -1;
            entries = Results.getAwardResults();

            results.AppendLine(@"<h1>Nagrody Specjalne</h1>");

            foreach (WinningEntry entry in entries)
            {
                if (awardId != entry.awardId)
                {
                    if(awardId > -1)
                    {
                        results.AppendLine(@"</table><br/><br/>");
                    }
                    results.AppendFormat(@"<table class=""award""><tr><th colspan=""3"">{0}</th></tr>", entry.awardTitle).AppendLine();
                }
                results.AppendFormat(@"<tr><td class=""name"">{0} {1}</td><td>{2}</td><td class=""modelName"">{3}</td></tr>", entry.firstName, entry.lastName, entry.entryId, entry.modelName).AppendLine();
                awardId = entry.awardId.Value;
            }
            if (awardId > -1)
            {
                results.AppendLine(@"</table>");
            }
            htmlTemplate = htmlTemplate.Replace("[NAGRODY]", results.ToString());
            
            File.WriteAllText(outputFileName, htmlTemplate);
        }

        public void printWordDoc(String filePath)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                Verb = "Print",
                WindowStyle = ProcessWindowStyle.Minimized,
                FileName = filePath
            };
            Process process = Process.Start(psi);
            process.WaitForExit();
        }
    }
}
