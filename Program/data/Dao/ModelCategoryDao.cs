using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Rejestracja.Data.Dao
{
    class ModelCategoryDao
    {
        public static IEnumerable<ModelCategory> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Code, Name, ModelClass, DisplayOrder FROM ModelCategory ORDER BY DisplayOrder ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new ModelCategory(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetInt32(4)
                        );
                }
            }
        }

        public static bool codeExists(String code)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM ModelCategory WHERE Code = @Code", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Code", DbType.String).Value = code;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static long add(String code, String name, String modelClass, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO ModelCategory(Code, Name, ModelClass, DisplayOrder) VALUES(@Code, @Name, @ModelClass, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Code", System.Data.DbType.String, 16).Value = code;
                cm.Parameters.Add("@Name", System.Data.DbType.String, 128).Value = name;
                cm.Parameters.Add("@ModelClass", System.Data.DbType.String, 128).Value = modelClass;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public static int getNextSortFlag()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM ModelCategory", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                object res = cm.ExecuteScalar();
                if (String.IsNullOrEmpty(res.ToString()))
                    return 0;
                else
                    return (int.Parse(res.ToString()) + 1);
            }
        }

        public static ModelCategory get(long id)
        {
            ModelCategory ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Code, Name, ModelClass, DisplayOrder FROM ModelCategory WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int64).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        ret = new ModelCategory(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetInt32(4)
                        );
                }
            }
            return ret;
        }

        public static void updateDisplayOrder(long id, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"UPDATE ModelCategory SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn))
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
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM ModelCategory WHERE Id = @Id", cn))
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
                @"CREATE TABLE ModelCategory(
                    Id INTEGER PRIMARY KEY,
                    Code TEXT NOT NULL,
                    Name TEXT,
                    ModelClass TEXT,
                    DisplayOrder INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE INDEX Idx_MC_Code ON ModelCategory(Code)";
                cm.ExecuteNonQuery();
            }

            int i = 0;
            //Standard
            add("MSO", "Statki i Okręty", "Standard", i++);
            add("MOP", "Okręty podwodne", "Standard", i++);
            add("MŻ", "Żaglowce", "Standard", i++);
            add("S1A", "Samoloty jednosilnikowe śmigłowe do 1930 roku", "Standard", i++);
            add("S1B", "Samoloty jednosilnikowe śmigłowe po 1930 roku", "Standard", i++);
            add("S2", "Samoloty wielosilnikowe śmigłowe", "Standard", i++);
            add("S3", "Samoloty odrzutowe", "Standard", i++);
            add("S4", "Samoloty w skali 1:48; 1:50", "Standard", i++);
            add("S5", "Szybowce", "Standard", i++);
            add("S6", "Śmigłowce", "Standard", i++);
            add("MPKC", "Pojazdy kołowe cywilne", "Standard", i++);
            add("MPKW", "Pojazdy kołowe wojskowe", "Standard", i++);
            add("MPSZ", "Pojazdy szynowe", "Standard", i++);
            add("MPG", "Pojazdy gąsienicowe", "Standard", i++);
            add("MB", "Budowle", "Standard", i++);
            add("MF", "Figurki", "Standard", i++);
            add("MR", "Rakiety, statki kosmiczne, S-F", "Standard", i++);
            add("MA", "Artyleria", "Standard", i++);
            //Open
            add("WSO", "Statki i okręty", "Waloryzowane (Open)", i++);
            add("WŻ", "Żaglowce", "Waloryzowane (Open)", i++);
            add("WOP", "Okręty podwodne", "Waloryzowane (Open)", i++);
            add("WKC", "Pojazdy kołowe cywilne", "Waloryzowane (Open)", i++);
            add("WKW", "Pojazdy kołowe wojskowe", "Waloryzowane (Open)", i++);
            add("WSZ", "Pojazdy szynowe", "Waloryzowane (Open)", i++);
            add("WG", "Pojazdy gąsienicowe", "Waloryzowane (Open)", i++);
            add("WS1", "Samoloty śmigłowe + śmigłowce (bez podziału na napęd)", "Waloryzowane (Open)", i++);
            add("WS2", "Samoloty odrzutowe", "Waloryzowane (Open)", i++);
            add("WPP", "Modele projektowane od podstaw", "Waloryzowane (Open)", i++);
            add("WD", "Dioramy i makiety", "Waloryzowane (Open)", i++);
        }
    }
}
