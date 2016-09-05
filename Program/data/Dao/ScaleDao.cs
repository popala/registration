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
    class ScaleDao
    {
        public static IEnumerable<ModelScale> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, DisplayOrder FROM Scales ORDER BY DisplayOrder ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new ModelScale(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetInt32(2)
                        );
                }
            }
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM Scales ORDER BY DisplayOrder ASC", cn))
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
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM Scales WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", DbType.String).Value = name;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static int add(String name, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Scales(Name, DisplayOrder) VALUES(@Name, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String, 128).Value = name;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static int getNextSortFlag()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM Scales", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                object res = cm.ExecuteScalar();
                if (String.IsNullOrEmpty(res.ToString()))
                    return 1;
                else
                    return (int.Parse(res.ToString()) + 1);
            }
        }

        public static void updateDisplayOrder(int id, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"UPDATE Scales SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(int id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Scales WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE Scales(
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    DisplayOrder INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_MS_Name ON Scales(Name)";
                cm.ExecuteNonQuery();
            }

            String[] defaults = new String[] { "1:9", "1:16", "1:25", "1:33", "1:43", "1:48", "1:50", "1:100", "1:150", "1:200", "1:250", "1:300", "1:400", "1:500", "Inna" };
            int displayOrder = 0;
            foreach (String scale in defaults) {
                add(scale, displayOrder);
                displayOrder++; 
            }
        }
    }
}
