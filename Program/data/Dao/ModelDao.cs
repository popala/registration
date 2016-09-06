﻿using Rejestracja.Data.Objects;
using Rejestracja.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Dao {
    class ModelDao {
        public static IEnumerable<Model> getList() {
            return getList(null);
        }

        public static IEnumerable<Model> search(String value) {
            return getList(value);
        }

        private static IEnumerable<Model> getList(String searchValue) {

            StringBuilder query = new StringBuilder("SELECT Id, Name, Publisher, Scale, ModelerId FROM Models ");
            if(!String.IsNullOrWhiteSpace(searchValue)) {
                query.Append(" WHERE Name LIKE @SearchValue ");
            }
            query.Append(" ORDER BY Id ");

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(query.ToString(), cn)) {
                cn.Open();

                if(!String.IsNullOrWhiteSpace(searchValue)) {
                    cm.Parameters.Add("@SearchValue", DbType.String).Value = "%" + searchValue + "%";
                }

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    while(dr.Read())
                        yield return new Model(
                            dr.GetInt32(0),
                            dr.GetString(1),
                            dr.GetString(2),
                            dr.GetString(3),
                            dr.GetInt32(4)
                        );
                }
            }
        }

        public static int add(String name, String publisher, String scale, int modelerId) {
            return add(name, publisher, scale, modelerId, null);
        }

        public static int add(Model model) {
            return add(null, null, null, 0, model);
        }

        private static int add(String name, String publisher, String scale, int modelerId, Model model) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(@"INSERT INTO Models(Name, Publisher, Scale, ModelerId) VALUES(@Name, @Publisher, @Scale, @ModelerId)", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                if(model == null) {
                    cm.Parameters.Add("@Name", System.Data.DbType.String, Model.MAX_NAME_LENGTH).Value = name;
                    cm.Parameters.Add("@Publisher", System.Data.DbType.String, Publisher.MAX_NAME_LENGTH).Value = publisher;
                    cm.Parameters.Add("@Scale", System.Data.DbType.String, Scale.MAX_NAME_LENGTH).Value = scale;
                    cm.Parameters.Add("@ModelerId", System.Data.DbType.Int32).Value = modelerId;
                }
                else {
                    cm.Parameters.Add("@Name", System.Data.DbType.String, Modeler.MAX_NAME_LENGTH).Value = model.name;
                    cm.Parameters.Add("@Publisher", System.Data.DbType.String, Publisher.MAX_NAME_LENGTH).Value = model.publisher;
                    cm.Parameters.Add("@Scale", System.Data.DbType.String, Scale.MAX_NAME_LENGTH).Value = model.scale;
                    cm.Parameters.Add("@ModelerId", System.Data.DbType.Int32).Value = model.modelerId;
                }
                cm.ExecuteNonQuery();

                return (int)cn.LastInsertRowId;
            }
        }

        public static Model get(int id) {
            Model ret = null;

            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand("SELECT Id, Name, Publisher, Scale, ModelerId FROM Models WHERE Id = @Id", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@Id", DbType.Int32).Value = id;

                using(SQLiteDataReader dr = cm.ExecuteReader()) {
                    if(dr.Read())
                        ret = new Model(
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

        public static void delete(int id) {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"DELETE FROM Results WHERE RegistrationId IN(SELECT RegistrationId FROM Registration WHERE ModelId = @Id);
                    DELETE FROM Registration WHERE ModelId = @Id;
                    DELETE FROM Models WHERE ModelId = @Id;", cn)) {

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@Id", DbType.Int32).Value = id;
                cm.ExecuteNonQuery();
            }
        }

        public static void createTable() {
            using(SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using(SQLiteCommand cm = new SQLiteCommand(
                    @"CREATE TABLE Models(
                        Id INTEGER PRIMARY KEY,
                        Name TEXT,
                        Publisher TEXT,
                        Scale TEXT,
                        ModelerId INTEGER NOT NULL REFERENCES Modelers(Id));
                    CREATE INDEX Idx_Models_MdlrId ON Models(ModelerId);", cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.ExecuteNonQuery();
            }
        }
    }
}