using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja
{
    class Publisher
    {
        public long publisherId;
        public String name;

        public Publisher(long publisherId, String name)
        {
            this.publisherId = publisherId;
            this.name = name;
        }

        public static IEnumerable<Publisher> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT PublisherId, Name FROM Publisher ORDER BY Name ASC", cn))
            {
                cn.Open();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new Publisher(dr.GetInt64(0), dr.GetString(1));
                }
            }
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM Publisher ORDER BY Name ASC", cn))
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
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT PublisherId FROM Publisher WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", DbType.String, 64).Value = name;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static long add(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Publisher(Name) VALUES(@Name)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", DbType.String, 64).Value = name;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public static void delete(long publisherId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Publisher WHERE PublisherId = @PublisherId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@PublisherId", System.Data.DbType.Int64).Value = publisherId;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"CREATE TABLE Publisher(
                    PublisherId INTEGER  PRIMARY KEY,
                    Name TEXT NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE INDEX Idx_Pub_Name ON Publisher(Name)";
                cm.ExecuteNonQuery();
            }

            String[] defaults = new String[] { "GPM", "Answer", "WAK", "Kartonowa Kolekcja", "Mały Modelarz", "JSC" ,"Modelik", "Orlik MK", "Haliński", "Model-Kom", "Orel", "WMC", "Dom Bumagi", "Draf Model", "Szkutnik", "Wektor", "Sklej Model", "Hobby Model", "AJ Model", "Model Hobby", "Quest", "Internet", "Inne" };
            Array.Sort(defaults, StringComparer.InvariantCulture);
            foreach (String publisher in defaults) {
                add(publisher);
            }
        }
    }
}
