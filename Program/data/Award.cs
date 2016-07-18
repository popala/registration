﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Rejestracja
{
    class Award
    {
        public long id;
        public String title;
        public long displayOrder;

        public const int TITLE_MAX_LENGTH = 256;

        public Award(String title, long displayOrder)
        {
            this.title = title;
            this.displayOrder = displayOrder;
        }

        public Award(long id, String title, long displayOrder)
        {
            this.id = id;
            this.title = title;
            this.displayOrder = displayOrder;
        }

        public Award(long id)
        {
            Award award = get(id);
            if (award == null)
                return;

            this.id = award.id;
            this.title = award.title;
            this.displayOrder = award.displayOrder;
        }

        public string[] toArray()
        {
            return new string[] {
                this.id.ToString(),
                this.title,
                this.displayOrder.ToString()
            };
        }

        public static Award get(long id)
        {
            Award ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Title, DisplayOrder FROM SpecialAwards WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ret = new Award(
                            dr.GetInt64(dr.GetOrdinal("Id")),
                            dr["Title"].ToString(),
                            dr.GetInt64(dr.GetOrdinal("DisplayOrder"))
                        );
                    }
                }
            }
            return ret;
        }

        public static long add(String title, long displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO SpecialAwards(Title, DisplayOrder) VALUES(@Title, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Title", System.Data.DbType.String, TITLE_MAX_LENGTH).Value = title;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int64).Value = displayOrder;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public void update()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"UPDATE SpecialAwards SET Title = @Title, DisplayOrder = @DisplayOrder WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Title", System.Data.DbType.String, TITLE_MAX_LENGTH).Value = this.title;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int64).Value = this.displayOrder;
                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = this.id;

                cm.ExecuteNonQuery();
            }
        }

        public static void updateDisplayOrder(long id, int displayOrder) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"UPDATE SpecialAwards SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();
            }
        }

        public static bool delete(long id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Results WHERE AwardId = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.ExecuteNonQuery();

                cm.CommandText = @"DELETE FROM SpecialAwards WHERE Id = @Id";
                return (cm.ExecuteNonQuery() == 1);
            }
        }

        public static bool exists(String title) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM SpecialAwards WHERE UPPER(Title) = UPPER(@Title)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Title", System.Data.DbType.String).Value = title;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static int getNextSortFlag() {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM SpecialAwards", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                object res = cm.ExecuteScalar();
                if (String.IsNullOrEmpty(res.ToString()))
                    return 0;
                else
                    return (int.Parse(res.ToString()) + 1);
            }
        }

        public static IEnumerable<Award> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Title, DisplayOrder FROM SpecialAwards ORDER BY DisplayOrder ASC", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        yield return
                            new Award( 
                                dr.GetInt64(dr.GetOrdinal("Id")),
                                dr["Title"].ToString(),
                                dr.GetInt64(dr.GetOrdinal("DisplayOrder"))
                            );
                    }
                }
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE SpecialAwards(
                    Id INTEGER PRIMARY KEY,
                    Title TEXT NOT NULL,
                    DisplayOrder INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE INDEX Idx_Aw_Title ON SpecialAwards(Title)";
                cm.ExecuteNonQuery();
            }
        }
    }
}
