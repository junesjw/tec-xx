using Dapper;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace tec_xx
***REMOVED***
    public class DataAccess : IDataAccess
    ***REMOVED***
        public async Task<string> LoadCharacterAccessName(string character)
        ***REMOVED***
            string sql = $"select Character from Aliases where Alias = '***REMOVED***character***REMOVED***'";

            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            ***REMOVED***
                var data = await connection.QuerySingleAsync<string>(sql);

                return data;
          ***REMOVED***
      ***REMOVED***

        public async Task<T> LoadData<T, U>(string table, string character, PropertyInfo[] props, U parameters)
        ***REMOVED***
            var sb = new StringBuilder();

            for (int i = 0; i < props.Count(); i++)
            ***REMOVED***
                sb.Append(props[i].Name);

                if (i + 1 != props.Count())
                ***REMOVED***
                    sb.Append(", ");
              ***REMOVED***
          ***REMOVED***

            string sql = $"select ***REMOVED***sb***REMOVED*** from ***REMOVED***table***REMOVED*** where Character = '***REMOVED***character***REMOVED***'";

            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            ***REMOVED***
                var data = await connection.QuerySingleAsync<T>(sql, parameters);

                return data;
          ***REMOVED***
      ***REMOVED***

        public async Task<bool> CharacterExists(string character, string column, string table)
        ***REMOVED***
            string sql = $"select count(distinct 1) from ***REMOVED***table***REMOVED*** where ***REMOVED***column***REMOVED*** = '***REMOVED***character***REMOVED***'";

            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            ***REMOVED***
                connection.Open();
                var exists = await connection.ExecuteScalarAsync<bool>(sql);

                return exists;
          ***REMOVED***
      ***REMOVED***

        private static string LoadConnectionString(string id = "Default")
        ***REMOVED***
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
      ***REMOVED***
  ***REMOVED***
***REMOVED***
