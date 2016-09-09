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
using Novacode;
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


                template.ReplaceText("[Imie]", result.entry.modeler.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imię]", result.entry.modeler.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nazwisko]", result.entry.modeler.lastName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);

                template.ReplaceText("[GrupaWiekowa]", result.entry.registration.ageGroupName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NazwaModelu]", result.entry.model.name,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlasaModelu]", result.entry.category.className,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", result.entry.category.name,
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
                template.ReplaceText("[DataRejestracji]", entry.registration.timeStamp.ToString(Resources.DateFormat),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NumerStartowy]", entry.registration.id.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NS]", entry.registration.id.ToString(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Email]", entry.modeler.email,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imie]", entry.modeler.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Imię]", entry.modeler.firstName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Nazwisko]", entry.modeler.lastName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[L]", entry.modeler.lastName.Substring(0, 1).ToUpper(),
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlubModelarski]", entry.modeler.clubName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[GrupaWiekowa]", entry.registration.ageGroupName,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[NazwaModelu]", entry.model.name,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[SkalaModelu]", entry.model.scale,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KlasaModelu]", entry.category.className,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[Wydawnictwo]", entry.model.publisher,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[KategoriaModelu]", entry.category.name,
                    false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                template.ReplaceText("[RokUrodzenia]", entry.modeler.yearOfBirth.ToString(),
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
            List<Category> categories = CategoryDao.getList().ToList();
            Dictionary<String, Char> authors = new Dictionary<String, Char>();
            
            String authorKey;
            Char authorMarker = 'A';
            String documentHeader = Options.get("DocumentHeader");

            String ageGroup = null;
            String modelClass = null;
            String modelCategory = null;
            Category [] category = null;

            String outputFileName = null;
            DocX doc = null;
            Table tbl = null;
            Row row = null;

            parentForm.resetProgressBar(entries.Count);

            try
            {
                foreach (RegistrationEntry entry in entries)
                {
                    authorKey = String.Format("{0}_{1}_{2}", entry.modeler.firstName, entry.modeler.lastName, entry.modeler.yearOfBirth);

                    if(!ageGroup.Equals(entry.registration.ageGroupName, StringComparison.CurrentCultureIgnoreCase) ||
                        !modelClass.Equals(entry.category.className, StringComparison.CurrentCultureIgnoreCase) ||
                        !modelCategory.Equals(entry.category.name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (doc != null)
                        {
                            doc.Save();
                            doc.Dispose();
                            doc = null;
                        }

                        //Match existing category
                        category = categories.Where(x => x.fullName.Equals(entry.category.name, StringComparison.CurrentCultureIgnoreCase)).ToArray();

                        outputFileName = 
                            String.Format("{0}\\{1}_{2}_{3}.docx", 
                            outFolder,
                            Resources.FileNameInvalidChars.Replace(entry.registration.ageGroupName, "-").ToUpper(),
                            Resources.FileNameInvalidChars.Replace(entry.category.className, "-").ToUpper(), 
                            Resources.FileNameInvalidChars.Replace(entry.category.name, "-"));

                        File.Copy(template, outputFileName);

                        doc = DocX.Load(outputFileName);
                        doc.ReplaceText("[Naglowek]", documentHeader, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Naglowek!]", documentHeader.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[GrupaWiekowa]", entry.registration.ageGroupName, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[GrupaWiekowa!]", entry.registration.ageGroupName.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Klasa]", entry.category.className, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Klasa!]", entry.category.className.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Kategoria]", entry.category.name, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        doc.ReplaceText("[Kategoria!]", entry.category.name, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        if (category.Length > 0) {
                            doc.ReplaceText("[KodKategorii]", category[0].code, false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                            doc.ReplaceText("[KodKategorii!]", category[0].code.ToUpper(), false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        }
                        else {
                            doc.ReplaceText("[KodKategorii]", "---", false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                            doc.ReplaceText("[KodKategorii!]", "---", false, RegexOptions.IgnoreCase & RegexOptions.Singleline, null, null, MatchFormattingOptions.ExactMatch);
                        }

                        tbl = doc.Tables[1];

                        ageGroup = entry.registration.ageGroupName;
                        modelClass = entry.category.className;
                        modelCategory = entry.category.name;

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

                    row.Cells[0].Paragraphs[0].InsertText(entry.registration.id.ToString());
                    row.Cells[1].Paragraphs[0].InsertText(authors[authorKey].ToString());
                    row.Cells[2].Paragraphs[0].InsertText(entry.model.name);

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
                if (ageGroup != result.entry.registration.ageGroupName || modelCategory != result.entry.category.name)
                {
                    if(ageGroup != null)
                    {
                        resultHtml.AppendLine(@"</table></div>");
                    }
                    ageGroup = result.entry.registration.ageGroupName;
                    modelCategory = result.entry.category.name;
                    modelClass = "";

                    resultHtml.AppendFormat(@"<div class=""tbl""><h2>Grupa Wiekowa <span class=""ageGroup"">{0}</span> - <span class=""modelCategory"">{1}</span></h2>",
                        result.entry.registration.ageGroupName.ToUpper(), result.entry.category.name).AppendLine();
                }
                if(modelClass != result.entry.category.className)
                {
                    lpCounter = 1;

                    if(!String.IsNullOrWhiteSpace(modelClass))
                    {
                        resultHtml.AppendLine(@"</table></div>");
                    }

                    modelClass = result.entry.category.className;
                    resultHtml.AppendFormat(@"<h3>{0}</h3>", result.entry.category.className).AppendLine();
                    resultHtml.AppendLine(@"<table class=""category"">");
                    //Insert headers
                    resultHtml.AppendFormat(@"<tr><th class=""lp"">{0}</th><th class=""name"">{1}</th><th class=""modelName"" colspan=""2"">{2}</th><th class=""place"">{3}</th></tr>", headers).AppendLine();
                }
                resultHtml.AppendFormat(@"<tr><td class=""lp"">{0}</td><td class=""name"">{1} {2}</td><td class=""modelNumber"">{3}</td><td class=""modelName"">{4}</td><td class=""place"">{5}</td></tr>",
                    lpCounter, result.entry.modeler.firstName, result.entry.modeler.lastName, result.entry.registration.id, result.entry.model.name, result.place).AppendLine();
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
                    result.entry.modeler.firstName, result.entry.modeler.lastName, result.entry.registration.id, result.entry.model.name).AppendLine();
                awardId = result.award.id;
            }
            if (awardId > -1)
            {
                resultHtml.AppendLine(@"</table>");
            }
            htmlTemplate = htmlTemplate.Replace("[NAGRODY]", resultHtml.ToString());
            
            File.WriteAllText(outputFileName, htmlTemplate);
        }

        public static void generateHtmlResultsV2(String template, String outputFileName) {
            String htmlTemplate;
            String docHeader = String.Format("<h1>{0}</h1>", Options.get("DocumentHeader").Replace("\r\n", "<br/>"));
            String docFooter = String.Format(@"<div class=""footer"">{0}</div>", Options.get("DocumentFooter").Replace("\r\n", "<br/>"));

            String categoryTableHeader =
@"<table class=""category"">
    <tr><td colspan=""3"" class=""title""><span class=""lt"">{0}</span><span class=""rt"">{1}</span></td></tr>
    <tr><th class=""place"">Miejsce</th><th class=""name"">Imię i Nazwisko</th><th class=""model"">Nr i Nazwa Modelu</th></tr>";
            String categoryTableRow = @"    <tr><td>{0}</td><td>{1} {2}</td><td>{3} {4}</td></tr>";

            String awardTableHeader =
@"<table class=""category"">
    <tr><td colspan=""2"" class=""title""><span class=""lt"">{0}</span></td></tr>
    <tr><th class=""name"">Imię i Nazwisko</th><th class=""model"">Nr i Nazwa Modelu</th></tr>";
            String awardTableRow = @"    <tr><td>{0} {1}</td><td>{2} {3}</td></tr>";

            String ageGroup = null;
            String modelClass = null;
            String modelCategory = null;

            //Prep header or use a template with header
            if (String.IsNullOrWhiteSpace(template)) {
                htmlTemplate = Resources.ResultsTemplate;
            }
            else {
                using (StreamReader sr = File.OpenText(template)) {
                    htmlTemplate = sr.ReadToEnd();
                }
            }

            //Category winners
            IEnumerable<Result> results = ResultDao.getCategoryResults();
            StringBuilder sb = new StringBuilder();

            foreach (Result result in results) {
                
                //Category, class or age group changed so close the table if age group is not empty
                if(ageGroup != result.entry.registration.ageGroupName || modelClass != result.entry.category.className || modelCategory != result.entry.category.name) {
                    if (ageGroup != null) {
                        sb.AppendLine("</table>");
                    }
                
                    //If age group changed we need H1, H2
                    if (ageGroup == null || !ageGroup.Equals(result.entry.registration.ageGroupName)) {
                        if (ageGroup != null) {
                            sb.AppendLine(docFooter);
                        }
                        sb.AppendLine(docHeader);
                        sb.AppendFormat("<h2>Grupa Wiekowa {0}</h2>", result.entry.registration.ageGroupName).AppendLine();
                        ageGroup = result.entry.registration.ageGroupName;
                    }

                    //Start a new table for diff class and/or category
                    modelClass = result.entry.category.className;
                    modelCategory = result.entry.category.name;
                    sb.AppendFormat(categoryTableHeader, result.entry.category.name, result.entry.category.className).AppendLine();
                }
                sb.AppendFormat(categoryTableRow, result.place, result.entry.modeler.firstName, result.entry.modeler.lastName, result.entry.registration.id, result.entry.model.name).AppendLine();
            }
            if (ageGroup != null) {
                sb.AppendLine("</table>");
                sb.AppendLine(docFooter);
            }

            //SPECIAL AWARDS
            long awardId = -1;
            results = ResultDao.getAwardResults();

            sb.AppendLine(docHeader);
            sb.AppendLine("<h2>Nagrody Specjalne</h2>");

            foreach (Result result in results) {
                if (awardId != result.award.id) {
                    if (awardId > -1) {
                        sb.AppendLine(@"</table>");
                    }
                    sb.AppendFormat(awardTableHeader, result.award.title).AppendLine();
                }
                sb.AppendFormat(awardTableRow, result.entry.modeler.firstName, result.entry.modeler.lastName, result.entry.registration.id, result.entry.model.name).AppendLine();
                awardId = result.award.id;
            }
            if (awardId > -1) {
                sb.AppendLine(@"</table>");
                sb.AppendLine(docFooter);
            }

            htmlTemplate = htmlTemplate.Replace("[WYNIKI]", sb.ToString());
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

        private static String sanitizeFileName(String fileName) {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(fileName, "_");
        }
    }
}
