using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Rejestracja
{
    class RegistrationEntry
    {
        public long entryId;
        public DateTime timeStamp;
        public String email;
        public String firstName;
        public String lastName;
        public String clubName;
        public String ageGroup;
        public String modelName;
        public String modelCategory;
        public long modelCategoryId;
        public String modelScale;
        public String modelPublisher;
        public String modelClass;
        public int yearOfBirth;
        public int place;

        public RegistrationEntry() {

        }

        public RegistrationEntry(long entryId, DateTime timeStamp, String email, String firstName, String lastName, String clubName, String ageGroup,
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId, int yearOfBirth)
        {
            this.entryId = entryId;
            this.timeStamp = timeStamp;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(long entryId, String email, String firstName, String lastName, String clubName, String ageGroup, 
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId, int yearOfBirth)
        {
            this.entryId = entryId;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(DateTime timeStamp, String email, String firstName, String lastName, String clubName, String ageGroup,
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId, int yearOfBirth)
        {
            this.timeStamp = timeStamp;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(long entryId, String ageGroup, String modelName, String modelClass, String modelScale, String modelPublisher, int place) {
            this.entryId = entryId;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelClass = modelClass;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.place = place;
        }

        public static RegistrationEntry get(long entryId)
        {
            return get(entryId, null, null);
        }

        private static RegistrationEntry get(long? entryId, String email, String lastName)
        {
            RegistrationEntry ret = null;

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn))
            {
                cn.Open();

                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText =
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(ModelCategoryId, -1) AS ModelCategoryId, COALESCE(YearOfBirth,0) AS YearOfBirth 
                     FROM Registration ";

                if (entryId.HasValue)
                {
                    cm.CommandText += "WHERE EntryId = @EntryId";
                    cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = entryId;
                }
                else if (email != null)
                {
                    cm.CommandText += "WHERE email = @email";
                    cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = email;
                }
                else if (lastName != null)
                {
                    cm.CommandText += "WHERE lastName = @lastName";
                    cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = lastName;
                }
                else
                {
                    throw new ArgumentNullException("EntryId, Email or LastName have to be provided!");
                }

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ret = new RegistrationEntry(
                            dr.GetInt64(dr.GetOrdinal("EntryId")),
                            (DateTime)dr["TmStamp"],
                            dr["Email"].ToString(),
                            dr["FirstName"].ToString(),
                            dr["LastName"].ToString(),
                            dr["ClubName"].ToString(),
                            dr["AgeGroup"].ToString(),
                            dr["ModelName"].ToString(),
                            dr["ModelClass"].ToString(),
                            dr["ModelScale"].ToString(),
                            dr["ModelPublisher"].ToString(),
                            dr["ModelCategory"].ToString(),
                            dr.GetInt64(dr.GetOrdinal("ModelCategoryId")),
                            dr.GetInt32(dr.GetOrdinal("YearOfBirth"))
                        );
                    }
                }
            }
            return ret;
        }

        public static RegistrationEntry get(String email, String lastName)
        {
            return get((int?)null, email, lastName);
        }

        public static IEnumerable<String[]> getList(String searchValue, int sortField, bool sortAscending) {

            string sortFieldName;

            switch (sortField) { 
                case 0: sortFieldName = "EntryId"; break;
                case 1: sortFieldName = "TmStamp"; break;
                case 2: sortFieldName = "Email"; break;
                case 3: sortFieldName = "FirstName"; break;
                case 4: sortFieldName = "LastName"; break;
                case 5: sortFieldName = "YearOfBirth"; break;
                case 6: sortFieldName = "ClubName"; break;
                case 7: sortFieldName = "AgeGroup"; break;
                case 8: sortFieldName = "ModelName"; break;
                case 9: sortFieldName = "ModelCategory"; break;
                case 10: sortFieldName = "ModelClass"; break;
                case 11: sortFieldName = "ModelScale"; break;
                case 12: sortFieldName = "ModelPublisher"; break;
                default: sortFieldName = "EntryId"; break;
            }

            return getList(searchValue, sortFieldName, sortAscending);
        }

        private static IEnumerable<String[]> getList(String searchValue, String sortField, bool sortAscending)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn))
            {
                StringBuilder query = new StringBuilder(
                    @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                    ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(YearOfBirth,0) AS YearOfBirth 
                 FROM Registration ");

                if (!String.IsNullOrEmpty(searchValue))
                {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n))
                    {
                        query.Append(" EntryId = @EntryId AND ");
                        cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = n;
                    }

                    query.Append(" Email LIKE @SearchValue OR FirstName LIKE @SearchValue OR LastName LIKE @SearchValue OR ModelName LIKE @SearchValue ");
                    cm.Parameters.Add("@SearchValue", System.Data.DbType.String, 64).Value = "%" + searchValue + "%";
                }
                query.Append(" ORDER BY ").Append(sortField).Append(sortAscending ? " ASC" : " DESC");

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = query.ToString();

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int yearOfBirth = 0;
                        int.TryParse(dr["YearOfBirth"].ToString(), out yearOfBirth);

                        yield return
                            new String[] 
                            {
                                dr["EntryId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString()
                            };
                    }
                }
            }
        }

        public static IEnumerable<String[]> getGrouppedList() {
            return getGrouppedList(null);
        }
        
        public static IEnumerable<String[]> getGrouppedList(String searchValue) {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand("", cn)) {
                StringBuilder query = new StringBuilder(
                    @"SELECT r.EntryId, r.TmStamp, r.Email, r.FirstName, r.LastName, r.ClubName, r.AgeGroup,
                        r.ModelName, r.ModelClass, r.ModelScale, r.ModelPublisher, r.ModelCategory, COALESCE(r.YearOfBirth,0) AS YearOfBirth,
                        CASE WHEN m.DisplayOrder IS NULL THEN -1 ELSE m.DisplayOrder END AS DisplayOrder
                    FROM Registration r
                        LEFT JOIN ModelCategory m ON m.Id = r.ModelCategoryId");

                if (!String.IsNullOrEmpty(searchValue)) {
                    query.Append(" WHERE ");

                    int n;
                    if (Int32.TryParse(searchValue, out n)) {
                        query.Append(" EntryId = @EntryId AND ");
                        cm.Parameters.Add("@EntryId", System.Data.DbType.Int32).Value = n;
                    }

                    query.Append(" Email LIKE @SearchValue OR FirstName LIKE @SearchValue OR LastName LIKE @SearchValue OR ModelName LIKE @SearchValue ");
                    cm.Parameters.Add("@SearchValue", System.Data.DbType.String, 64).Value = "%" + searchValue + "%";
                }
                query.Append(" ORDER BY DisplayOrder, r.ModelCategory, r.AgeGroup, r.ModelClass, r.EntryId");

                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.CommandText = query.ToString();

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        int yearOfBirth = 0;
                        int.TryParse(dr["YearOfBirth"].ToString(), out yearOfBirth);

                        yield return
                            new String[] 
                            {
                                dr["EntryId"].ToString(),
                                ((DateTime)dr["TmStamp"]).ToString(Resources.DateFormat),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                yearOfBirth.ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString()
                            };
                    }
                }
            }
        }
        
        public static List<RegistrationEntry> getListForJudging()
        {
            List<RegistrationEntry> ret = new List<RegistrationEntry>();

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"SELECT EntryId, TmStamp, Email, FirstName, LastName, ClubName, AgeGroup,
                        ModelName, ModelClass, ModelScale, ModelPublisher, ModelCategory, COALESCE(ModelCategoryId, -1) AS ModelCategoryId, COALESCE(YearOfBirth,0) AS YearOfBirth 
                    FROM Registration 
                    ORDER BY ModelCategory, AgeGroup, ModelClass, EntryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ret.Add(
                            new RegistrationEntry(
                                dr.GetInt64(dr.GetOrdinal("EntryId")),
                                dr.GetDateTime(dr.GetOrdinal("TmStamp")),
                                dr["Email"].ToString(),
                                dr["FirstName"].ToString(),
                                dr["LastName"].ToString(),
                                dr["ClubName"].ToString(),
                                dr["AgeGroup"].ToString(),
                                dr["ModelName"].ToString(),
                                dr["ModelClass"].ToString(),
                                dr["ModelScale"].ToString(),
                                dr["ModelPublisher"].ToString(),
                                dr["ModelCategory"].ToString(),
                                dr.GetInt64(dr.GetOrdinal("ModelCategoryId")),
                                dr.GetInt32(dr.GetOrdinal("YearOfBirth"))
                        ));
                    }
                    return ret;
                }
            }
        }

        public static IEnumerable<String[]> getListForMergingCategories() {
            
            String query = @"SELECT r.AgeGroup, r.ModelClass, r.ModelCategory, r.ModelCategoryId, COUNT(r.EntryId) AS EntryCount, CASE m.DisplayOrder WHEN NULL THEN 0 ELSE m.DisplayOrder END AS DisplayOrder
                FROM Registration r
	                LEFT JOIN ModelCategory m ON m.Id = r.ModelCategoryId
	                LEFT JOIN AgeGroup ag ON r.AgeGroup = ag.Name
                GROUP BY r.AgeGroup, r.ModelClass, r.ModelCategoryId
                ORDER BY m.DisplayOrder, r.ModelCategory, ag.Age, r.ModelClass";
            
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(query, cn)) {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader dr = cm.ExecuteReader()) {
                    while (dr.Read()) {
                        yield return
                            new String[] 
                                {
                                    dr["AgeGroup"].ToString(),
                                    dr["ModelClass"].ToString(),
                                    dr["ModelCategory"].ToString(),
                                    dr["ModelCategoryId"].ToString(),
                                    dr["EntryCount"].ToString()
                                };
                    }
                }
            }
        }
        
        public void add()
        {
            if(this.entryId > 0)
            {
                throw new InvalidOperationException("EntryID populated");
            }

            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"INSERT INTO Registration(TmStamp,Email,FirstName,LastName,ClubName,AgeGroup,ModelName,ModelClass,ModelScale,ModelPublisher,ModelCategory,YearOfBirth)
                  VALUES(@tmStamp,@email,@firstName,@lastName,@clubName,@ageGroup,@modelName,@modelClass,@modelScale,@modelPublisher,@modelCategory,@yearOfBirth)", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@tmStamp", System.Data.DbType.DateTime).Value = this.timeStamp;
                cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = this.email;
                cm.Parameters.Add("@firstName", System.Data.DbType.String, 64).Value = this.firstName;
                cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = this.lastName;
                cm.Parameters.Add("@clubName", System.Data.DbType.String, 128).Value = this.clubName;
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, 16).Value = this.ageGroup;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = this.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = this.modelClass;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = this.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = this.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = this.modelCategory;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = this.yearOfBirth;
                cm.ExecuteNonQuery();

                this.entryId = cn.LastInsertRowId;
            }
        }

        public void update()
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(
                @"UPDATE Registration SET Email = @email, FirstName = @firstName, LastName = @lastName, ClubName = @clubName,
                        AgeGroup = @ageGroup, ModelName = @modelName, ModelClass = @modelClass, ModelScale = @modelScale, 
                        ModelPublisher = @modelPublisher, ModelCategory = @modelCategory, ModelCategoryId = COALESCE(@modelCategoryId, -1), YearOfBirth = @yearOfBirth
                  WHERE EntryId = @entryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;

                cm.Parameters.Add("@email", System.Data.DbType.String, 256).Value = this.email;
                cm.Parameters.Add("@firstName", System.Data.DbType.String, 64).Value = this.firstName;
                cm.Parameters.Add("@lastName", System.Data.DbType.String, 64).Value = this.lastName;
                cm.Parameters.Add("@clubName", System.Data.DbType.String, 128).Value = this.clubName;
                cm.Parameters.Add("@ageGroup", System.Data.DbType.String, 16).Value = this.ageGroup;
                cm.Parameters.Add("@modelName", System.Data.DbType.String, 256).Value = this.modelName;
                cm.Parameters.Add("@modelClass", System.Data.DbType.String, 128).Value = this.modelClass;
                cm.Parameters.Add("@modelScale", System.Data.DbType.String, 8).Value = this.modelScale;
                cm.Parameters.Add("@modelPublisher", System.Data.DbType.String, 64).Value = this.modelPublisher;
                cm.Parameters.Add("@modelCategory", System.Data.DbType.String, 64).Value = this.modelCategory;
                cm.Parameters.Add("@modelCategoryId", System.Data.DbType.Int64).Value = this.modelCategoryId;
                cm.Parameters.Add("@yearOfBirth", System.Data.DbType.Int32).Value = this.yearOfBirth;
                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = this.entryId;

                cm.ExecuteNonQuery();
            }
        }

        public static bool delete(long entryId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(Resources.getConnectionString()))
            using (SQLiteCommand cm = new SQLiteCommand(@"DELETE FROM Registration WHERE EntryId = @entryId", cn))
            {
                cn.Open();
                cm.CommandType = System.Data.CommandType.Text;
                cm.Parameters.Add("@entryId", System.Data.DbType.Int64).Value = entryId;
                return ((int)cm.ExecuteNonQuery() == 1);
            }
        }

        public string[] toArray()
        {
            return new string[] {
                this.entryId.ToString(),
                timeStamp.ToString(Resources.DateFormat),
                this.email,
                this.firstName,
                this.lastName,
                this.clubName,
                this.ageGroup,
                this.modelName,
                this.modelCategory,
                this.modelScale,
                this.modelPublisher,
                this.modelClass,
                this.yearOfBirth.ToString()
            };
        }
    }
}
