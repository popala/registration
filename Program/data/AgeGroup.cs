using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja
{
    class AgeGroup
    {
        public long id;
        public String name;
        public int upperAge;
        public int bottomAge;

        public AgeGroup()
        {
        }

        public AgeGroup(long id, String name, int upperAge)
        {
            this.id = id;
            this.name = name;
            this.upperAge = upperAge;
        }

        public AgeGroup(long id, String name, int upperAge, int bottomAge)
        {
            this.id = id;
            this.name = name;
            this.upperAge = upperAge;
            this.bottomAge = bottomAge;
        }

        public static bool exists(int upperAge) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM AgeGroup WHERE Age BETWEEN @Age1 AND @Age2", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Age1", DbType.String).Value = upperAge - 1;
                cm.Parameters.Add("@Age2", DbType.String).Value = upperAge + 1;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static AgeGroup getOlderAgeGroup(String ageGroupName) {
            
            AgeGroup ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT a.Id, a.Name, a.Age, (SELECT MAX(Age) AS Age FROM AgeGroup WHERE Age < a.Age) AS MinAge
	                FROM AgeGroup a
	                JOIN (
		                SELECT MIN(Age) AS Age 
			                FROM AgeGroup 
			                WHERE Age > (SELECT Age FROM AgeGroup WHERE Name = @AgeGroupName)
		                ) m ON a.Age = m.Age", cn))
            {
                cn.Open();
                cm.Parameters.Add("@AgeGroupName", DbType.String).Value = ageGroupName;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        int bottomAge = 0;
                        if (!dr.IsDBNull(dr.GetOrdinal("MinAge"))) {
                            bottomAge = dr.GetInt32(dr.GetOrdinal("MinAge")) + 1;
                        }
                        ret = new AgeGroup(dr.GetInt64(0), dr.GetString(1), dr.GetInt32(2), bottomAge);
                    }
                }
            }
            return ret;
        }

        public static IEnumerable<AgeGroup> getList()
        {
            List<AgeGroup> ret = new List<AgeGroup>();
            int bottomAge = 0;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, Age FROM AgeGroup ORDER BY Age ASC", cn))
            {
                cn.Open();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        AgeGroup ageGroup = new AgeGroup(dr.GetInt64(0), dr.GetString(1), dr.GetInt32(2), bottomAge);
                        bottomAge = ageGroup.upperAge + 1;
                        ret.Add(ageGroup);
                    }
                }
            }
            return ret;
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM AgeGroup ORDER BY Name ASC", cn))
            {
                cn.Open();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return dr.GetString(0);
                }
            }
        }

        public static long add(String name, int age)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO AgeGroup(Name, Age) VALUES(@Name, @Age)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", DbType.String, 64).Value = name;
                cm.Parameters.Add("@Age", DbType.Int32).Value = age;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public static void delete(long id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM AgeGroup WHERE Id = @Id", cn))
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
                @"CREATE TABLE AgeGroup(
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Age INTEGER NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_AgeGroup_Name ON AgeGroup(Name)";
                cm.ExecuteNonQuery();
            }

            add("Młodzik", 12);
            add("Junior", 17);
            add("Senior", 150);
        }
    }
}
