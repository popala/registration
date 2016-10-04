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
    class CategoryDao
    {
        public static IEnumerable<Category> getList() {
            return getList(false, null);
        }

        public static IEnumerable<Category> getList(bool includeImported, String modelClass)
        {
            String query = null;

            if(includeImported) {
                query =
                    @"SELECT Id, Code, Name, ModelClass, DisplayOrder FROM Categories
                    UNION
                    SELECT -1 AS Id, NULL AS Code, CategoryName AS Name, NULL AS ModelClass, -1 AS DisplayOrder FROM Registration WHERE CategoryId < 0";
                if(!String.IsNullOrEmpty(modelClass)) {
                    query += " AND ModelClass = @ModelClass";
                }
            }
            else {
                query = "SELECT Id, Code, Name, ModelClass, DisplayOrder FROM Categories";
                if(!String.IsNullOrEmpty(modelClass)) {
                    query += " WHERE ModelClass = @ModelClass";
                }
            }

            query += " ORDER BY DisplayOrder, Name ASC";

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(query, cn))
            {
                cn.Open();

                if(!String.IsNullOrEmpty(modelClass)) {
                    cm.Parameters.Add("@ModelClass", DbType.String, Class.MAX_NAME_LENGTH).Value = modelClass;
                }

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new Category(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            dr["Code"].ToString(),
                            dr["Name"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                        );
                }
            }
        }

        public static bool codeExists(String code)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM Categories WHERE Code = @Code", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Code", DbType.String).Value = code;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static int add(String code, String name, String modelClass, int displayOrder)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Categories(Code, Name, ModelClass, DisplayOrder) VALUES(@Code, @Name, @ModelClass, @DisplayOrder)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Code", System.Data.DbType.String, 16).Value = code;
                cm.Parameters.Add("@Name", System.Data.DbType.String, 128).Value = name;
                cm.Parameters.Add("@ModelClass", System.Data.DbType.String, 128).Value = modelClass;
                cm.Parameters.Add("@DisplayOrder", System.Data.DbType.Int32).Value = displayOrder;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static int getNextSortFlag()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"SELECT MAX(DisplayOrder) AS displayOrder FROM Categories", cn))
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

        public static Category get(long id)
        {
            Category ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Code, Name, ModelClass, DisplayOrder FROM Categories WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int64).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        ret = new Category(
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
            using(SQLiteCommand cm = new SQLiteCommand(@"UPDATE Categories SET DisplayOrder = @DisplayOrder WHERE Id = @Id", cn))
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
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE EntryId IN(SELECT EntryId FROM Registration WHERE ModelCategoryId = @Id) AND Place IS NOT NULL", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                    
                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.ExecuteNonQuery();

                cm.CommandText = "UPDATE Registration SET ModelCategoryId = -1 WHERE ModelCategoryId = @Id";
                cm.ExecuteNonQuery();

                cm.CommandText = "DELETE FROM Categories WHERE Id = @Id";
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE Categories(
                    Id INTEGER PRIMARY KEY,
                    Code TEXT NOT NULL,
                    Name TEXT,
                    ModelClass TEXT,
                    DisplayOrder INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE INDEX Idx_MC_Code ON Categories(Code)";
                cm.ExecuteNonQuery();
                cm.CommandText = "CREATE INDEX Idx_MC_Class ON Categories(ModelClass)";
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
            //Naviga
            add("C-1", "Modele jednostek wiosłowych i żaglowych", "Naviga", i++);
            add("C-2", "Modele jednostek o napędzie mechanicznym", "Naviga", i++);
            add("C-3", "Modele urządzeń, itp.", "Naviga", i++);
            add("C-4", "Modele miniaturowe", "Naviga", i++);
            add("C-5", "Modele z butelkach", "Naviga", i++);
            add("C-6", "Modele plastikowe", "Naviga", i++);
            add("C-7", "Modele kartonowe i papierowe", "Naviga", i++);
        }
    }
}
