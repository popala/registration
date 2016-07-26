using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Rejestracja.Data.Dao
{
    class ModelClassDao
    {
        public static IEnumerable<ModelClass> getList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name FROM ModelClass ORDER BY Name ASC", cn))
            {
                cn.Open();

                using(SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new ModelClass(
                            dr.GetInt64(0),
                            dr.GetString(1)
                        );
                }
            }
        }

        public static IEnumerable<String> getSimpleList()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Name FROM ModelClass ORDER BY Name ASC", cn))
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
            using (SQLiteCommand cm = new SQLiteCommand(@"SELECT Id FROM ModelClass WHERE Name = @Name", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Name", DbType.String, ModelClass.MAX_NAME_LENGTH).Value = name;

                object res = cm.ExecuteScalar();
                return (res != null);
            }
        }

        public static long add(String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO ModelClass(Name) VALUES(@Name)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Name", System.Data.DbType.String, ModelClass.MAX_NAME_LENGTH).Value = name;
                cm.ExecuteNonQuery();

                return cn.LastInsertRowId;
            }
        }

        public static ModelClass get(long id)
        {
            ModelClass ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name FROM ModelClass WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int64).Value = id;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                        ret = new ModelClass(
                            dr.GetInt64(0),
                            dr.GetString(1)
                        );
                }
            }
            return ret;
        }

        public static void update(long id, String name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"UPDATE ModelClass SET Name = @Name WHERE Id = @Id", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", System.Data.DbType.Int64).Value = id;
                cm.Parameters.Add("@Name", System.Data.DbType.String, ModelClass.MAX_NAME_LENGTH).Value = name;
                cm.ExecuteNonQuery();
            }
        }

        public static void delete(long id)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM ModelClass WHERE Id = @Id", cn))
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
                @"CREATE TABLE ModelClass(
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();

                cm.CommandText = "CREATE UNIQUE INDEX Idx_MCat_Name ON ModelClass(Name)";
                cm.ExecuteNonQuery();
            }

            String[] defaults = new String[] { "Standard", "Waloryzowane (Open)" };
            foreach (String modelClass in defaults) {
                add(modelClass);
            }
        }
    }
}
