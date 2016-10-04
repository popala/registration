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
        public static List<Class> getList() {
            return getList(false);
        }

        public static List<Class> getList(bool includeCategories)
        {
            List<Class> ret = new List<Class>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, RegistrationTemplate, JudgingFormTemplate, DiplomaTemplate, ScoringCardType, UseCustomAgeGroups FROM Classes ORDER BY Name ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while(dr.Read()) {
                        Class c = new Class(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            dr["Name"].ToString(),
                            dr["RegistrationTemplate"].ToString(),
                            dr["JudgingFormTemplate"].ToString(),
                            dr["DiplomaTemplate"].ToString(),
                            dr.GetBoolean(dr.GetOrdinal("UseCustomAgeGroups")),
                            (Class.ScoringCardType)dr.GetInt32(dr.GetOrdinal("ScoringCardType"))
                        );
                        ret.Add(c);
                    }
                }

                if(includeCategories) {
                    ret.Add(new Class(-1, Class.IMPORTED_CLASS_TITLE, null, null, null, false, Class.ScoringCardType.SeparateAgeGroupSeparateCategory));

                    cm.CommandText =
                        @"SELECT Id, Code, Name, ModelClass, DisplayOrder FROM Categories
                        UNION
                        SELECT -1 AS Id, NULL AS Code, CategoryName AS Name, '" + Class.IMPORTED_CLASS_TITLE + @"' AS ModelClass, -1 AS DisplayOrder FROM Registration WHERE CategoryId < 0
                        ORDER BY ModelClass, DisplayOrder";
                    using(SQLiteDataReader dr = cm.ExecuteReader()) {
                        while(dr.Read()) {
                            Category category = new Category(
                                dr.GetInt32(dr.GetOrdinal("Id")),
                                dr["Code"].ToString(),
                                dr["Name"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                            );
                            int idx = ret.FindIndex(x => x.name.Equals(category.className, StringComparison.CurrentCultureIgnoreCase));
                            ret[idx].categories.Add(category);
                        }
                    }
                }
            }
            return ret;
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

        public static Class get(String name) {
            return get(-1, name);
        }

        public static Class get(int id) {
            return get(id, null);
        }

        private static Class get(int id, String name)
        {
            Class ret = null;
            String query;
            if(id > -1) {
                query = "SELECT Id, Name, RegistrationTemplate, JudgingFormTemplate, DiplomaTemplate, ScoringCardType, UseCustomAgeGroups FROM Classes WHERE Id = @Id";
            }
            else {
                query = "SELECT Id, Name, RegistrationTemplate, JudgingFormTemplate, DiplomaTemplate, ScoringCardType, UseCustomAgeGroups FROM Classes WHERE Name = @Name";
            }

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(query, cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                if(id > -1) {
                    cm.Parameters.Add("@Id", DbType.Int32).Value = id;
                }
                else {
                    cm.Parameters.Add("@Name", DbType.String, Class.MAX_NAME_LENGTH).Value = name;
                }

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        ret = new Class(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            dr["Name"].ToString(),
                            dr["RegistrationTemplate"].ToString(),
                            dr["JudgingFormTemplate"].ToString(),
                            dr["DiplomaTemplate"].ToString(),
                            dr.GetBoolean(dr.GetOrdinal("UseCustomAgeGroups")),
                            (Class.ScoringCardType)dr.GetInt32(dr.GetOrdinal("ScoringCardType"))
                        );
                }

                cm.Parameters.Clear();
                cm.CommandText = "SELECT Id, Code, Name, ModelClass, DisplayOrder FROM Categories WHERE ModelClass = @ModelClass";
                cm.Parameters.Add("ModelClass", DbType.String, Class.MAX_NAME_LENGTH).Value = ret.name;
                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read()) {
                        ret.categories.Add(
                            new Category(
                                dr.GetInt32(dr.GetOrdinal("Id")),
                                dr["Code"].ToString(),
                                dr["Name"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("DisplayOrder"))
                        ));
                    }
                }
            }

            if(ret.useCustomAgeGroups) {
                ret.ageGroups = AgeGroupDao.getList(ret.id);
            }
            return ret;
        }

        public static void update(int id, String name, Class.ScoringCardType scoringCardType, bool useCustomAgeGroups, String registrationTemplate, String judgingFormTemplate, String diplomaTemplate)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Classes SET
                    Name = @Name,
                    RegistrationTemplate = @RegistrationTemplate, JudgingFormTemplate = @JudgingFormTemplate, DiplomaTemplate = @DiplomaTemplate,
                    ScoringCardType = @ScoringCardType, UseCustomAgeGroups = @UseCustomAgeGroups
                WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
                cm.Parameters.Add("@Name", System.Data.DbType.String, Class.MAX_NAME_LENGTH).Value = name;
                cm.Parameters.Add("@RegistrationTemplate", System.Data.DbType.String).Value = registrationTemplate;
                cm.Parameters.Add("@JudgingFormTemplate", System.Data.DbType.String).Value = judgingFormTemplate;
                cm.Parameters.Add("@DiplomaTemplate", System.Data.DbType.String).Value = diplomaTemplate;
                cm.Parameters.Add("@ScoringCardType", System.Data.DbType.Int32).Value = (int)scoringCardType;
                cm.Parameters.Add("@UseCustomAgeGroups", System.Data.DbType.Boolean).Value = useCustomAgeGroups;
                cm.ExecuteNonQuery();
            }
        }

        public static void update(Class modelClass) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Classes SET
                    Name = @Name,
                    RegistrationTemplate = @RegistrationTemplate, JudgingFormTemplate = @JudgingFormTemplate, DiplomaTemplate = @DiplomaTemplate,
                    ScoringCardType = @ScoringCardType, UseCustomAgeGroups = @UseCustomAgeGroups
                WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = modelClass.id;
                cm.Parameters.Add("@Name", System.Data.DbType.String, Class.MAX_NAME_LENGTH).Value = modelClass.name;
                cm.Parameters.Add("@RegistrationTemplate", System.Data.DbType.String).Value = modelClass.registrationCardTemplate;
                cm.Parameters.Add("@JudgingFormTemplate", System.Data.DbType.String).Value = modelClass.judgingFormTemplate;
                cm.Parameters.Add("@DiplomaTemplate", System.Data.DbType.String).Value = modelClass.diplomaTemplate;
                cm.Parameters.Add("@ScoringCardType", System.Data.DbType.Int32).Value = (int)modelClass.scoringCardType;
                cm.Parameters.Add("@UseCustomAgeGroups", System.Data.DbType.Boolean).Value = modelClass.useCustomAgeGroups;
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
                    Name TEXT NOT NULL,
                    RegistrationTemplate TEXT NULL,
                    JudgingFormTemplate TEXT NULL,
                    DiplomaTemplate TEXT NULL,
                    ScoringCardType INTEGER NOT NULL DEFAULT 0,
                    UseCustomAgeGroups INTEGER NOT NULL DEFAULT 0)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_MCat_Name ON Classes(Name)";
                cm.ExecuteNonQuery();
            }

            add("Standard");
            add("Waloryzowane (Open)");
            add("Naviga");
        }
    }
}
