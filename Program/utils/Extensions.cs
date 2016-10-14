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
using System.Text;

namespace Rejestracja.Utils {
    public static class Extensions {
        public static string Replace(this string s, string oldValue, string newValue, StringComparison comparisonType) {
            if(s == null)
                return null;

            if(String.IsNullOrEmpty(oldValue))
                return s;

            StringBuilder result = new StringBuilder(Math.Min(4096, s.Length));
            int pos = 0;

            while(true) {
                int i = s.IndexOf(oldValue, pos, comparisonType);
                if(i < 0)
                    break;

                result.Append(s, pos, i - pos);
                result.Append(newValue);

                pos = i + oldValue.Length;
            }
            result.Append(s, pos, s.Length - pos);

            return result.ToString();
        }

        public static string Replace(this string s, string oldValue, string newValue) {
            return Replace(s, oldValue, newValue, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
