using Rejestracja.Utils;
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
using System.Text.RegularExpressions;

namespace Rejestracja.Data.Objects {
    class Scale {
        public int id;
        public String name;
        public int displayOrder;

        public const int MAX_NAME_LENGTH = 32;

        public Scale(int id, String name, int displayOrder) {
            this.id = id;
            this.name = name;
            this.displayOrder = displayOrder;
        }

        public static String parse(String newScale) {
            String tmp = newScale.Replace(" ", "").Replace("\t", "");
            if (Regex.IsMatch(tmp, "[0-9]:[0-9]{1,}")) {
                string [] parts = tmp.Split(':');
                try {
                    return String.Format("{0}:{1}", int.Parse(parts[0]), int.Parse(parts[1]));
                }
                catch (Exception err) {
                    LogWriter.error(String.Format("Error parsing scale: \"{0}\"", newScale), err);
                    return newScale;
                }
            }
            else {
                return newScale;
            }
        }
    }
}
