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
using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Rejestracja.Data.Dao
{
    class ClassDao
    {
        public static IEnumerable<Class> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name FROM Classes ORDER BY Name ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new Class(
                            dr.GetInt32(0),
                            dr.GetString(1)
                        );
                }
            }
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM Classes ORDER BY Name ASC", cn))
            {
                cn.Open();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return dr.GetString(0);
                }
            }
        }

        public static bool exists(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM Classes WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", DbType.String, Class.MAX_NAME_LENGTH).Value = name;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static int add(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Classes(Name) VALUES(@Name)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String, Class.MAX_NAME_LENGTH).Value = name;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static Class get(int id)
        {
            Class ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name FROM Classes WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        ret = new Class(
                            dr.GetInt32(0),
                            dr.GetString(1)
                        );
                }
            }
            return ret;
        }

        public static void update(int id, String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"UPDATE Classes SET Name = @Name WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.Parameters.Add("@Name", System.Data.DbType.String, Class.MAX_NAME_LENGTH).Value = name;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(int id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Classes WHERE ModelClass = (SELECT Name FROM Classes WHERE Id = @Id)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.ExecuteNonQuery();

                cm.CommandText = @"DELETE FROM Classes WHERE Id = @Id";
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE Classes(
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_MCat_Name ON Classes(Name)";
                cm.ExecuteNonQuery();
            }

            add("Standard");
            add("Waloryzowane (Open)");
        }
    }
}
