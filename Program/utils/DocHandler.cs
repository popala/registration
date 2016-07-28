﻿using Novacode;
using Rejestracja.Data.Dao;
using Rejestracja.Data.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rejestracja.Utils
{
    class DocHandler
    {
        public static void generateDiploma(String templateFile, String outFile, Result result)
        {
            String documentHeader = Options.get("DocumentHeader") ?? "";
            String documentFooter = Options.get("DocumentFooter") ?? "";

            using (DocX template = DocX.Load(templateFile))
            {
                template.ReplaceText("[Naglowek]", documentHeader, 
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nagłówek]", documentHeader,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);


                template.ReplaceText("[Imie]", result.entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imię]", result.entry.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nazwisko]", result.entry.lastName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.ReplaceText("[GrupaWiekowa]", result.entry.ageGroup,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NazwaModelu]", result.entry.modelName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlasaModelu]", result.entry.modelClass,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", result.entry.modelCategory,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.ReplaceText("[Stopka]", documentFooter,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Stopka!]", documentFooter.ToUpper(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                if (result.place > 0)
                {
                    template.ReplaceText("[Miejsce]", result.place.ToString(),
                        false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                }
                if (result.award != null)
                {
                    template.ReplaceText("[Nagroda]", result.award.title,
                        false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                }

                template.SaveAs(outFile);
            }
        }

        public static void generateRegistrationCard(String templateFile, String outFile, RegistrationEntry entry)
        {
            String documentFooter = Options.get("DocumentFooter");

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
                template.ReplaceText("[KlasaModelu]", entry.modelClass,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Wydawnictwo]", entry.modelPublisher,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", entry.modelCategory,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[RokUrodzenia]", entry.yearOfBirth.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Stopka]", documentFooter,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Stopka!]", documentFooter.ToUpper(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.SaveAs(outFile);
            }
        }

        public static void generateJudgingForms(String template, String outFolder, frmMain parentForm)
        {
            List<RegistrationEntry> entries = RegistrationEntryDao.getListForJudging();
            List<ModelCategory> categories = ModelCategoryDao.getList().ToList();
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
                        category = categories.Where(x => x.fullName.ToLower().Equals(entry.modelCategory.ToLower())).ToArray();

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

        public static void generateHtmlResults(String template, String outputFileName)
        {
            String htmlTemplate;
            String docHeader = Options.get("DocumentHeader").Replace("\r\n", "<br/>");

            String[] headers = "L.p.,Imię i Nazwisko,Nr i Nazwa Modelu,MIEJSCE".Split(',');

            String ageGroup = null;
            String modelClass = null;
            String modelCategory = null;

            //Prep header or use a template with header
            if (String.IsNullOrWhiteSpace(template))
            {
                htmlTemplate = Resources.ResultsTemplate;
            }
            else
            {
                using(StreamReader sr = File.OpenText(template)) {
                    htmlTemplate = sr.ReadToEnd();
                }
            }

            //Header
            htmlTemplate = htmlTemplate.Replace("[NAGLOWEK]", String.Format("<h1>{0}</h1>", docHeader));
            htmlTemplate = htmlTemplate.Replace("[NAGŁÓWEK]", String.Format("<h1>{0}</h1>", docHeader));

            //Category winners
            IEnumerable<Result> results = ResultDao.getCategoryResults();
            StringBuilder resultHtml = new StringBuilder();
            int lpCounter = 1;

            foreach (Result result in results)
            {
                if (ageGroup != result.entry.ageGroup || modelCategory != result.entry.modelCategory)
                {
                    if(ageGroup != null)
                    {
                        resultHtml.AppendLine(@"</table></div>");
                    }
                    ageGroup = result.entry.ageGroup;
                    modelCategory = result.entry.modelCategory;
                    modelClass = "";

                    resultHtml.AppendFormat(@"<div class=""tbl""><h2>Grupa Wiekowa <span class=""ageGroup"">{0}</span> - <span class=""modelCategory"">{1}</span></h2>",
                        result.entry.ageGroup.ToUpper(), result.entry.modelCategory).AppendLine();
                }
                if (modelClass != result.entry.modelClass)
                {
                    lpCounter = 1;

                    if(!String.IsNullOrWhiteSpace(modelClass))
                    {
                        resultHtml.AppendLine(@"</table></div>");
                    }

                    modelClass = result.entry.modelClass;
                    resultHtml.AppendFormat(@"<h3>{0}</h3>", result.entry.modelClass).AppendLine();
                    resultHtml.AppendLine(@"<table class=""category"">");
                    //Insert headers
                    resultHtml.AppendFormat(@"<tr><th class=""lp"">{0}</th><th class=""name"">{1}</th><th class=""modelName"" colspan=""2"">{2}</th><th class=""place"">{3}</th></tr>", headers).AppendLine();
                }
                resultHtml.AppendFormat(@"<tr><td class=""lp"">{0}</td><td class=""name"">{1} {2}</td><td class=""modelNumber"">{3}</td><td class=""modelName"">{4}</td><td class=""place"">{5}</td></tr>",
                    lpCounter, result.entry.firstName, result.entry.lastName, result.entry.entryId, result.entry.modelName, result.place).AppendLine();
                lpCounter++;
            }
            if(ageGroup != null)
            {
                resultHtml.AppendLine(@"</table></div>");
            }

            htmlTemplate = htmlTemplate.Replace("[KATEGORIE]", resultHtml.ToString());
            resultHtml.Clear();


            //SPECIAL AWARDS
            long awardId = -1;
            results = ResultDao.getAwardResults();

            resultHtml.AppendLine(@"<h2>Nagrody Specjalne</h2>");

            foreach (Result result in results)
            {
                if (awardId != result.award.id)
                {
                    if(awardId > -1)
                    {
                        resultHtml.AppendLine(@"</table><br/><br/>");
                    }
                    resultHtml.AppendFormat(@"<table class=""award""><tr><th colspan=""3"">{0}</th></tr>", result.award.title).AppendLine();
                }
                resultHtml.AppendFormat(@"<tr><td class=""name"">{0} {1}</td><td class=""modelNumber"">{2}</td><td class=""modelName"">{3}</td></tr>", 
                    result.entry.firstName, result.entry.lastName, result.entry.entryId, result.entry.modelName).AppendLine();
                awardId = result.award.id;
            }
            if (awardId > -1)
            {
                resultHtml.AppendLine(@"</table>");
            }
            htmlTemplate = htmlTemplate.Replace("[NAGRODY]", resultHtml.ToString());
            
            File.WriteAllText(outputFileName, htmlTemplate);
        }

        public static void generateHtmlSummary(String template, String outputFileName) {
            String htmlTemplate;
            String docHeader = (Options.get("DocumentHeader") ?? "").Replace("\r\n", "<br/>");
            String docFooter = (Options.get("DocumentFooter") ?? "").Replace("\r\n", "<br/>");
            StringBuilder sb = new StringBuilder();

            //Prep header or use a template with header
            if (String.IsNullOrWhiteSpace(template)) {
                htmlTemplate = Resources.SummaryTemplate;
            }
            else {
                using (StreamReader sr = File.OpenText(template)) {
                    htmlTemplate = sr.ReadToEnd();
                }
            }

            //Header
            htmlTemplate = htmlTemplate.Replace("[NAGLOWEK]", String.Format("{0}", docHeader));
            htmlTemplate = htmlTemplate.Replace("[NAGŁÓWEK]", String.Format("{0}", docHeader));

            //Summary stats
            List<KeyValuePair<String, String>> stats = RegistrationEntryDao.getRegistrationStats();
            sb.AppendLine(@"<div class=""stats"">").AppendLine();

            foreach (KeyValuePair<String, String> stat in stats) {
                if (stat.Key.StartsWith("GROUP")) {
                    if (!stat.Key.Equals("GROUP1")) {
                        sb.AppendLine("</table>");
                    }
                    sb.AppendLine("<table>").AppendFormat(@"<tr><th colspan=""2"">{0}</th></tr>", stat.Value).AppendLine();
                }
                else {
                    if (stat.Key.StartsWith("*")) {
                        sb.AppendFormat(@"<tr><td class=""lc b"">{0}</td><td class=""rc b"">{1}</td></tr>", stat.Key.Substring(1), stat.Value).AppendLine();
                    }
                    else {
                        sb.AppendFormat(@"<tr><td class=""lc"">{0}</td><td class=""rc"">{1}</td></tr>", stat.Key, stat.Value).AppendLine();
                    }
                }
            }
            sb.AppendLine("</table>").AppendLine("</div>");

            htmlTemplate = htmlTemplate.Replace("[PODSUMOWANIE]", sb.ToString());
            htmlTemplate = htmlTemplate.Replace("[STOPKA]", docFooter);
            File.WriteAllText(outputFileName, htmlTemplate);
        }

        public static void PrintHtmlDoc(String filePath) {
            // Create a WebBrowser instance. 
            WebBrowser webBrowserForPrinting = new WebBrowser();

            // Add an event handler that prints the document after it loads.
            webBrowserForPrinting.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);

            // Set the Url property to load the document.
            webBrowserForPrinting.Url = new Uri(filePath);
        }

        private static void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e) {
            // Print the document now that it is fully loaded.
            ((WebBrowser)sender).Print();

            // Dispose the WebBrowser now that the task is complete. 
            ((WebBrowser)sender).Dispose();
        }

        public static void printWordDoc(String filePath)
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
