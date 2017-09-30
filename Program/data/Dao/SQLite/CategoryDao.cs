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
    class CategoryDao : ICategoryDao
    {
        public IEnumerable<Category> getList() {
            return getList(false, null);
        }

        public IEnumerable<Category> getList(bool includeImported, String modelClass)
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

        public bool codeExists(String code)
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

        public int add(String code, String name, String modelClass, int displayOrder)
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

        public int getNextSortFlag()
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

        public Category get(long id)
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

        public void updateDisplayOrder(long id, int displayOrder)
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

        public void delete(long id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("DELETE FROM Results WHERE RegistrationId IN(SELECT Id FROM Registration WHERE CategoryId = @Id) AND Place IS NOT NULL", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                    
                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.ExecuteNonQuery();

                cm.CommandText = "UPDATE Registration SET CategoryId = -1 WHERE CategoryId = @Id";
                cm.ExecuteNonQuery();

                cm.CommandText = "DELETE FROM Categories WHERE Id = @Id";
                cm.ExecuteNonQuery();
            }
        }
    }
}
