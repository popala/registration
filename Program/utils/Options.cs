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
using System.Data.SQLite;

namespace Rejestracja.Utils
{
    class Options
    {
        public static void delete(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Options WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String).Value = name;
                cm.ExecuteNonQuery();
            }
        }

        public static void set(String name, String value)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Options WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", System.Data.DbType.String).Value = name;
                cm.ExecuteNonQuery();

                cm.CommandText = @"INSERT INTO Options(Name, Value) VALUES(@Name, @Value)";
                cm.Parameters.Add("@Value", System.Data.DbType.String).Value = value;
                cm.ExecuteNonQuery();
            }
        }

        public static String get(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Value FROM Options WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.Parameters.Add("@Name", System.Data.DbType.String).Value = name;
                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        return dr.GetString(0);
                    else
                        return null;
                }
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                    @"CREATE TABLE Options(
                        Name TEXT NOT NULL PRIMARY KEY,
                        Value TEXT NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_Opt_Name ON Options(Name)";
                cm.ExecuteNonQuery();
            }
        }
    }
}
