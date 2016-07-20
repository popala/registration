﻿using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Rejestracja.Data.Dao
{
    class ModelScaleDao
    {
        public static IEnumerable<ModelScale> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, DisplayOrder FROM ModelScale ORDER BY DisplayOrder ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new ModelScale(
                            dr.GetInt64(0),
                            dr.GetString(1),
                            dr.GetInt32(2)
                        );
                }
            }
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM ModelScale ORDER BY DisplayOrder ASC", cn))
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
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM ModelScale WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", DbType.String).Value = name;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static long add(String name, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO ModelScale(Name, DisplayOrder) VALUES(@Name, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String, 128).Value = name;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public static int getNextSortFlag()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM ModelScale", cn))
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

        public static void updateDisplayOrder(long id, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"UPDATE ModelScale SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(long id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM ModelScale WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE ModelScale(
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    DisplayOrder INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_MS_Name ON ModelScale(Name)";
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
