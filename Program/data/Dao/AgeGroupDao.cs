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
using System.Data.Common;

namespace Rejestracja.Data.Dao
{
    class AgeGroupDao
    {
        public bool exists(int upperAge, int classId) {
            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"SELECT Id FROM AgeGroups WHERE Age BETWEEN @Age1 AND @Age2 AND ClassId = @ClassId";

                DataSource.addParam(cm, "@Age1", (upperAge - 1));
                DataSource.addParam(cm, "@Age2", (upperAge + 1));
                DataSource.addParam(cm, "@ClassId", classId);

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public AgeGroup getOlderAgeGroup(String ageGroupName, int classId) {
            
            AgeGroup ret = null;

            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = CommandType.Text;
                cm.CommandText =
                    @"SELECT a.Id, a.Name, a.Age, (SELECT MAX(Age) AS Age FROM AgeGroups WHERE Age < a.Age AND ClassId = @ClassId) AS MinAge
	                FROM AgeGroups a
	                JOIN (
		                SELECT MIN(Age) AS Age 
			                FROM AgeGroups 
			                WHERE Age > (SELECT Age FROM AgeGroups WHERE Name = @AgeGroupName AND ClassId = @ClassId)
		                ) m ON a.Age = m.Age AND a.ClassId = m.ClassId";

                DataSource.addParam(cm, "@AgeGroupName", ageGroupName, AgeGroup.NAME_MAX_LENGTH);
                DataSource.addParam(cm, "@ClassId", classId);

                using (DbDataReader dr = cm.ExecuteReader())
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

        public List<AgeGroup> getList(int classId)
        {
            List<AgeGroup> ret = new List<AgeGroup>();
            int bottomAge = 0;

            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = CommandType.Text;
                cm.CommandText = "SELECT Id, Name, Age FROM AgeGroups WHERE ClassId = @ClassId ORDER BY Age ASC";

                DataSource.addParam(cm, "@ClassId", classId);

                using (DbDataReader dr = cm.ExecuteReader())
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

        public void add(String name, int age, int classId)
        {
            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"INSERT INTO AgeGroups(Classid, Name, Age) VALUES(@ClassId, @Name, @Age)";

                DataSource.addParam(cm, "@Name", name, AgeGroup.NAME_MAX_LENGTH);
                DataSource.addParam(cm, "@Age", age);
                DataSource.addParam(cm, "@ClassId", classId);
                cm.ExecuteNonQuery();
            }
        }

        public void deleteClassAgeGroups(int classId) {
            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"DELETE FROM AgeGroups WHERE ClassId = @Id";

                DataSource.addParam(cm, "@Id", classId);
                cm.ExecuteNonQuery();
            }
        }

        public void delete(int id)
        {
            using (DbConnection cn = DataSource.getConnection())
            using (DbCommand cm = DataSource.getCommand(cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = @"DELETE FROM AgeGroups WHERE Id = @Id";

                DataSource.addParam(cm, "@Id", id);
                cm.ExecuteNonQuery();
            }
        }
    }
}
