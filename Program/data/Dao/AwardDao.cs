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
using System.Data.SQLite;

namespace Rejestracja.Data.Dao
{
    class AwardDao
    {
        public Award get(int id)
        {
            Award ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Title, DisplayOrder FROM Awards WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ret = new Award(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            dr["Title"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                        );
                    }
                }
            }
            return ret;
        }

        public int add(String title, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Awards(Title, DisplayOrder) VALUES(@Title, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Title", System.Data.DbType.String, Award.TITLE_MAX_LENGTH).Value = title;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int64).Value = displayOrder;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public void updateDisplayOrder(int id, int displayOrder) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"UPDATE Awards SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();
            }
        }

        public bool delete(int id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Results WHERE AwardId = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.ExecuteNonQuery();

                cm.CommandText = @"DELETE FROM Awards WHERE Id = @Id";
                return (cm.ExecuteNonQuery() == 1);
            }
        }

        public bool exists(String title) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM Awards WHERE UPPER(Title) = UPPER(@Title)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Title", System.Data.DbType.String).Value = title;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public int getNextSortFlag() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM Awards", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                object res = cm.ExecuteScalar();
                if (String.IsNullOrEmpty(res.ToString()))
                    return 0;
                else
                    return (int.Parse(res.ToString()) + 1);
            }
        }

        public IEnumerable<Award> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Title, DisplayOrder FROM Awards ORDER BY DisplayOrder ASC", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        yield return
                            new Award( 
                                dr.GetInt32(dr.GetOrdinal("Id")),
                                dr["Title"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            );
                    }
                }
            }
        }
    }
}
