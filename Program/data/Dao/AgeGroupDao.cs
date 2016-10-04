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
    class AgeGroupDao
    {
        public static bool exists(int upperAge, int classId) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM AgeGroups WHERE Age BETWEEN @Age1 AND @Age2 AND ClassId = @ClassId", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Age1", DbType.Int32).Value = upperAge - 1;
                cm.Parameters.Add("@Age2", DbType.Int32).Value = upperAge + 1;
                cm.Parameters.Add("@ClassId", DbType.Int32).Value = classId;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static AgeGroup getOlderAgeGroup(String ageGroupName, int classId) {
            
            AgeGroup ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT a.Id, a.Name, a.Age, (SELECT MAX(Age) AS Age FROM AgeGroups WHERE Age < a.Age AND ClassId = @ClassId) AS MinAge
	                FROM AgeGroups a
	                JOIN (
		                SELECT MIN(Age) AS Age 
			                FROM AgeGroups 
			                WHERE Age > (SELECT Age FROM AgeGroups WHERE Name = @AgeGroupName AND ClassId = @ClassId)
		                ) m ON a.Age = m.Age AND a.ClassId = m.ClassId", cn))
            {
                cn.Open();
                cm.Parameters.Add("@AgeGroupName", DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = ageGroupName;
                cm.Parameters.Add("@ClassId", DbType.Int32).Value = classId;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        int bottomAge = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("MinAge"))) {
                            bottomAge = dr.GetInt32(dr.GetOrdinal("MinAge")) + 1;
                        }
                        ret = new AgeGroup(dr.GetInt32(0), dr.GetString(1), dr.GetInt32(2), bottomAge, -1);
                    }
                }
            }
            return ret;
        }

        public static List<AgeGroup> getList(int classId)
        {
            List<AgeGroup> ret = new List<AgeGroup>();
            int bottomAge = 0;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, Age FROM AgeGroups WHERE ClassId = @ClassId ORDER BY Age ASC", cn))
            {
                cn.Open();
                cm.Parameters.Add("@ClassId", DbType.Int32).Value = classId;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        AgeGroup ageGroup = new AgeGroup(dr.GetInt32(0), dr.GetString(1), dr.GetInt32(2), bottomAge, -1);
                        bottomAge = ageGroup.upperAge + 1;
                        ret.Add(ageGroup);
                    }
                }
            }
            return ret;
        }

        public static int add(String name, int age, int classId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO AgeGroups(Classid, Name, Age) VALUES(@ClassId, @Name, @Age)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", DbType.String, AgeGroup.NAME_MAX_LENGTH).Value = name;
                cm.Parameters.Add("@Age", DbType.Int32).Value = age;
                cm.Parameters.Add("@ClassId", DbType.Int32).Value = classId;
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static void deleteForClass(int classId) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM AgeGroups WHERE ClassId = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = classId;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(int id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM AgeGroups WHERE Id = @Id", cn))
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
                @"CREATE TABLE AgeGroups(
                    Id INTEGER PRIMARY KEY,
                    ClassId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Age INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_AgeGroup_Name ON AgeGroups(ClassId, Name)";
                cm.ExecuteNonQuery();
            }

            add("Młodzik", 12, -1);
            add("Junior", 17, -1);
            add("Senior", 150, -1);
        }
    }
}
