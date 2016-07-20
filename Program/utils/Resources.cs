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