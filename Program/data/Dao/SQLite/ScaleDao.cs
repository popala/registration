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
using System.Linq;

namespace Rejestracja.Data.Dao
{
    class ScaleDao
    {
        public static List<Scale> getList()
        {
            List<Scale> ret = new List<Scale>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name FROM Scales", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while(dr.Read()) {
                        ret.Add(
                            new Scale(dr.GetInt32(dr.GetOrdinal("Id")), dr["Name"].ToString())
                        );
                    }
                }
            }
            if(ret.Count > 0) {
                ret.Sort();
            }

            return ret;
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

        public static int add(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Scales(Name) VALUES(@Name)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String, 128).Value = name;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
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
    }
}
