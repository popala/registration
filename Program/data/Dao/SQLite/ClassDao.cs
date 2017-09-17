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
        private const String BASE_QUERY = 
            @"SELECT Id, Name, RegistrationTemplate, JudgingFormTemplate, DiplomaTemplate, ScoringCardType, UseCustomAgeGroups,
                ClassificationType, UsePointRange, PointRanges, UseDistinctions FROM Classes ";

        public static List<Class> getList() {
            return getList(false, false);
        }

        public static List<Class> getList(bool includeCategories, bool includeAgeGroups)
        {
            List<Class> ret = new List<Class>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(BASE_QUERY + " ORDER BY Name ASC", cn))
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
                            (Class.ScoringCardType)dr.GetInt32(dr.GetOrdinal("ScoringCardType")),
                            (Class.ClassificationType)dr.GetInt32(dr.GetOrdinal("ClassificationType")),
                            dr.GetBoolean(dr.GetOrdinal("UsePointRange")),
                            dr.GetBoolean(dr.GetOrdinal("UseDistinctions"))
                        );
                        if(c.usePointRange) {
                            String [] pointRanges = dr["PointRanges"].ToString().Split(',');
                            c.pointRanges = new int[pointRanges.Length];
                            for(int i = 0; i < pointRanges.Length; i++) {
                                c.pointRanges[i] = int.Parse(pointRanges[i]);
                            }
                        }
                        ret.Add(c);
                    }
                }

                if(includeCategories) {
                    ret.Add(new Class(-1, Class.IMPORTED_CLASS_TITLE, null, null, null, false, Class.ScoringCardType.SeparateAgeGroupSeparateCategory, Class.ClassificationType.Places, false, false));

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

                if(includeAgeGroups) {
                    cm.CommandText = 
                        @"SELECT ag.Id AS AgeGroupId, c.Id AS ClassId, ag.Name, ag.Age
	                        FROM Classes c
	                        LEFT JOIN AgeGroups ag ON
		                        CASE WHEN c.UseCustomAgeGroups = 0 THEN -1 ELSE c.Id END = ag.ClassId
                        ORDER BY c.Id, ag.Age";
                    using(SQLiteDataReader dr = cm.ExecuteReader()) {
                        
                        int lastClassId = -2;
                        int bottomAge = 0;

                        while(dr.Read()) {
                            int currentClassId = dr.GetInt32(dr.GetOrdinal("ClassId"));
                            if(currentClassId != lastClassId) {
                                bottomAge = 0;
                            }
                            AgeGroup ageGroup = new AgeGroup(
                                dr.GetInt32(dr.GetOrdinal("AgeGroupId")),
                                dr["Name"].ToString(),
                                dr.GetInt32(dr.GetOrdinal("Age")),
                                bottomAge,
                                currentClassId
                            );
                            bottomAge = ageGroup.upperAge + 1;

                            int idx = ret.FindIndex(x => x.id == ageGroup.classId);
                            ret[idx].ageGroups.Add(ageGroup);
                        }
                    }
                }
            }
            return ret;
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

        public static Class getClassForCategory(int id) {

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(BASE_QUERY + " WHERE Name = (SELECT ModelClass FROM Categories WHERE Id = @Id)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = id;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        return new Class(
                            dr.GetInt32(dr.GetOrdinal("Id")),
                            dr["Name"].ToString(),
                            dr["RegistrationTemplate"].ToString(),
                            dr["JudgingFormTemplate"].ToString(),
                            dr["DiplomaTemplate"].ToString(),
                            dr.GetBoolean(dr.GetOrdinal("UseCustomAgeGroups")),
                            (Class.ScoringCardType)dr.GetInt32(dr.GetOrdinal("ScoringCardType")),
                            (Class.ClassificationType)dr.GetInt32(dr.GetOrdinal("ClassificationType")),
                            dr.GetBoolean(dr.GetOrdinal("UsePointRange")),
                            dr.GetBoolean(dr.GetOrdinal("UseDistinctions"))
                        );
                }
            }
            return null;
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
            String query = BASE_QUERY + (id > -1 ? " WHERE Id = @Id" : " WHERE Name = @Name");

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
                            (Class.ScoringCardType)dr.GetInt32(dr.GetOrdinal("ScoringCardType")),
                            (Class.ClassificationType)dr.GetInt32(dr.GetOrdinal("ClassificationType")),
                            dr.GetBoolean(dr.GetOrdinal("UsePointRange")),
                            dr.GetBoolean(dr.GetOrdinal("UseDistinctions"))
                        );
                        if(ret.usePointRange) {
                            String[] pointRanges = dr["PointRanges"].ToString().Split(',');
                            ret.pointRanges = new int[pointRanges.Length];
                            for(int i = 0; i < pointRanges.Length; i++) {
                                ret.pointRanges[i] = int.Parse(pointRanges[i]);
                            }
                        }
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
                ret.ageGroups = new AgeGroupDao().getList(ret.id);
            }
            return ret;
        }

//        public static void update(int id, String name, Class.ScoringCardType scoringCardType, bool useCustomAgeGroups, String registrationTemplate, String judgingFormTemplate, String diplomaTemplate)
//        {
//            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
//            using(SQLiteCommand cm = new SQLiteCommand(
//                @"UPDATE Classes SET
//                    Name = @Name,
//                    RegistrationTemplate = @RegistrationTemplate, JudgingFormTemplate = @JudgingFormTemplate, DiplomaTemplate = @DiplomaTemplate,
//                    ScoringCardType = @ScoringCardType, UseCustomAgeGroups = @UseCustomAgeGroups, ClassificationType = @ClassificationType,
//                    UsePointRange = @UsePointRange, PointRanges = @PointRanges, UseDistinctions = @UseDistinctions
//                WHERE Id = @Id", cn))
//            {
//                cn.Open();
//                cm.CommandType = System.Data.CommandType.Text;

//                cm.Parameters.Add("@Id", System.Data.DbType.Int32).Value = id;
//                cm.Parameters.Add("@Name", System.Data.DbType.String, Class.MAX_NAME_LENGTH).Value = name;
//                cm.Parameters.Add("@RegistrationTemplate", System.Data.DbType.String).Value = registrationTemplate;
//                cm.Parameters.Add("@JudgingFormTemplate", System.Data.DbType.String).Value = judgingFormTemplate;
//                cm.Parameters.Add("@DiplomaTemplate", System.Data.DbType.String).Value = diplomaTemplate;
//                cm.Parameters.Add("@ScoringCardType", System.Data.DbType.Int32).Value = (int)scoringCardType;
//                cm.Parameters.Add("@UseCustomAgeGroups", System.Data.DbType.Boolean).Value = useCustomAgeGroups;
//                cm.Parameters.Add("@ClassificationType", System.Data.DbType.Int32).Value = (int)
//                cm.ExecuteNonQuery();
//            }
//        }

        public static void update(Class modelClass) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Classes SET
                    Name = @Name,
                    RegistrationTemplate = @RegistrationTemplate, JudgingFormTemplate = @JudgingFormTemplate, DiplomaTemplate = @DiplomaTemplate,
                    ScoringCardType = @ScoringCardType, UseCustomAgeGroups = @UseCustomAgeGroups, ClassificationType = @ClassificationType,
                    UsePointRange = @UsePointRange, PointRanges = @PointRanges, UseDistinctions = @UseDistinctions
                WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = modelClass.id;
                cm.Parameters.Add("@Name", DbType.String, Class.MAX_NAME_LENGTH).Value = modelClass.name;
                cm.Parameters.Add("@RegistrationTemplate", DbType.String).Value = modelClass.registrationCardTemplate;
                cm.Parameters.Add("@JudgingFormTemplate", DbType.String).Value = modelClass.judgingFormTemplate;
                cm.Parameters.Add("@DiplomaTemplate", DbType.String).Value = modelClass.diplomaTemplate;
                cm.Parameters.Add("@ScoringCardType", DbType.Int32).Value = (int)modelClass.scoringCardType;
                cm.Parameters.Add("@UseCustomAgeGroups", DbType.Boolean).Value = modelClass.useCustomAgeGroups;
                cm.Parameters.Add("@ClassificationType", DbType.Int32).Value = (int)modelClass.classificationType;
                cm.Parameters.Add("@UsePointRange", DbType.Boolean).Value = modelClass.usePointRange;
                if(modelClass.usePointRange && modelClass.pointRanges != null && modelClass.pointRanges.Length > 0) {
                    cm.Parameters.Add("@PointRanges", DbType.String).Value = String.Join(",", modelClass.pointRanges);
                }
                else {
                    cm.Parameters.Add("@PointRanges", DbType.String).Value = null;
                }
                cm.Parameters.Add("@UseDistinctions", DbType.Boolean).Value = modelClass.useDistinctions;
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
    }
}
