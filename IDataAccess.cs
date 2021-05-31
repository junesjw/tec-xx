using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace tec_xx
***REMOVED***
    public interface IDataAccess
    ***REMOVED***
        Task<bool> CharacterExists(string character, string column, string table);
        Task<string> LoadCharacterAccessName(string character);
        Task<T> LoadData<T, U>(string table, string character, PropertyInfo[] props, U parameters);
  ***REMOVED***
***REMOVED***