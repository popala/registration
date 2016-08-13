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
using System.IO;

namespace Rejestracja.Utils
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
                new String[] { String.Format("{0} error_source=\"{1}\" error_message=\"{2}\"", DateTime.Now, e.Source, e.Message), e.StackTrace }
            );
        }

        public static void error(String message, Exception e) {
            String logFile = Resources.ErrorLogPath;
            verifyFile(logFile);
            File.WriteAllLines(
                logFile,
                new String[] { String.Format("{0} message=\"{1}\" error_source=\"{2}\" error_message=\"{3}\"", DateTime.Now, message, e.Source, e.Message), e.StackTrace }
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
