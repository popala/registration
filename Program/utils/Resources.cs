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
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rejestracja.Utils {
    class Resources {
        public static String DateFormat = "yyyy-MM-dd HH:mm";
        public static Regex FileNameInvalidChars = new Regex(@"[\\/:*?""<>|]");
        public static String AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Przeciszow", "Rejestracja");
        public static String DataFileFolder = Path.Combine(AppDataFolder, "data");
        public static String DocumentFolder = Path.Combine(AppDataFolder, "dokumenty");
        public static String TemplateFolder = Path.Combine(AppDataFolder, "templates"); 
        public static String ErrorLogPath = Path.Combine(AppDataFolder, "logs", "error.log");
        public static String OutputLogPath = Path.Combine(AppDataFolder, "logs", "output.log");

        public static String ResultsTemplate =
@"<html>
    <head>
        <META HTTP-EQUIV=""Content-type"" CONTENT=""text/html; charset=UTF-8"">
        <title>Wyniki Konkursu</title>
    </head>
    <body>
        [NAGLOWEK]
        [KATEGORIE]
        [NAGRODY]
    </body>
</html>";

        public static String ResultsTemplateV2 =
@"<html>
    <head>
        <META HTTP-EQUIV=""Content-type"" CONTENT=""text/html; charset=UTF-8"">
        <title>Wyniki Konkursu</title>
    </head>
    <body>
        [WYNIKI]
    </body>
</html>";

        public static String SummaryTemplate =
@"<html>
    <head>
        <META HTTP-EQUIV=""Content-type"" CONTENT=""text/html; charset=UTF-8"">
        <title>Wyniki Konkursu</title>
    </head>
    <body>
        [NAGLOWEK]
        [POSUMOWANIE]
    </body>
</html>";

        public static void setDataFile(String dataFilePath) {

            if (dataFilePath.Contains(Path.PathSeparator)) {
                Properties.Settings.Default.DataFile = dataFilePath;
            }
            else {
                Properties.Settings.Default.DataFile = Path.Combine(DataFileFolder, dataFilePath);
            }

            Properties.Settings.Default.Save();
        }

        public static String getDataFilePath() {
            return Properties.Settings.Default.DataFile;
        }

        public static String getConnectionString() {
            return String.Format("Data Source={0}", Properties.Settings.Default.DataFile);
        }

        public static String resolvePath(String appSettingName) {
            String path = ConfigurationManager.AppSettings[appSettingName];

            if (String.IsNullOrWhiteSpace(path)) {
                return null;
            }

            return Path.Combine(AppDataFolder, path);
        }

        public static String removeDiacritics(String s) {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++) {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }
    }
}