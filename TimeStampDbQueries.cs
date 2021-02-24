using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using trifenix.connect.arguments;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.model;

namespace trifenix.connect.db.cosmos
{

    /// <summary>
    /// Realiza consultas de timestamp directo a una entidad de cosmos db
    /// todos los elementos de cosmos db mantienen la fecha
    /// </summary>
    public class TimeStampDbQueries : ITimeStampDbQueries {
        
        protected readonly CosmosStoreSettings StoreSettings;

        /// <summary>
        /// Constructor con los parámetros de conexión
        /// </summary>
        /// <param name="args">parámetros de cosmos db</param>
        public TimeStampDbQueries(CosmosDbArguments args) {
            StoreSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
        }
        
        /// <summary>
        /// Obtiene la colección de timestamps de un tipo de elemento
        /// </summary>
        /// <typeparam name="T">tipo de elemento</typeparam>
        /// <returns>Timestamp de todos los elementos de un tipo</returns>
        public async Task<long[]> GetTimestamps<T>() where T : DocumentDb {
            var store = new CosmosStore<T>(StoreSettings);
            var result = await store.QueryMultipleAsync<long>("SELECT value c._ts FROM c");
            if (result == null)
                return new  List<long>().ToArray();
            return result.ToArray();
        }

    }
}
