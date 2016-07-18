using System;
using System.IO;

namespace Rejestracja
{
    class LogWriter
    {
        public LogWriter()
        {
            verifyFile();
        }

        private static void verifyFile()
        {
            verifyFile(Resources.ErrorLogPath);
        }

        private static void verifyFile(String filePath)
        {
            String folder = Path.GetDirectoryName(filePath);

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if(!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }

        public static void error(Exception e)
        {
            String logFile = Resources.ErrorLogPath;
            verifyFile(logFile);
            File.WriteAllLines(
                logFile,
                new String[] { String.Format("{0} {1} {2}", DateTime.Now, e.Source, e.Message), e.StackTrace }
            );
        }

        public static void info(String message)
        {
            String logFile = Resources.OutputLogPath;
            verifyFile(logFile);
            File.WriteAllLines(
                logFile,
                new String[] { String.Format("{0} {1}", DateTime.Now, message) }
            );
        }
    }
}
