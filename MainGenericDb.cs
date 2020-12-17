using Cosmonaut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.arguments;

namespace trifenix.connect.db.cosmos
{

    /// <summary>
    /// Implementación de operaciones de base de datos cosmosDb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MainGenericDb<T> : IMainGenericDb<T> where T : DocumentBase {

        /// <summary>
        /// Store de Cosmonaut
        /// </summary>
        public ICosmosStore<T> Store { get; }

        
        

        /// <summary>
        /// Implementación de operaciones de base de datos para cosmosDb
        /// </summary>
        /// <param name="args">identificaciones de cosmosDb</param>
        public MainGenericDb(CosmosDbArguments args) {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            Store = new CosmosStore<T>(storeSettings);
            
        }

        /// <summary>
        /// Crea o actualiza una entidad 
        /// Upsert, Añade o actualiza
        /// </summary>
        /// <param name="entity">entidad a guardar</param>
        /// <returns>identificador de la entidad a aguardar</returns>
        public async Task<string> CreateUpdate(T entity) {

            // el elemento a guardar debe tener un id.
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentBase>(entity);

            // inserta o actualiza.
            var result = await Store.UpsertAsync(entity);

            // si no tiene exito, lanza excepción.
            if (!result.IsSuccess)
                throw result.Exception;

            // retorna el identificador de la entidad.
            return result.Entity.Id;
        }

        

        /// <summary>
        /// Obtiene una entidad desde el store
        /// </summary>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public async Task<T> GetEntity(string id) => await Store.FindAsync(id);
       

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        /// <param name="id">identificador de la entidad a eliminar</param>
        /// <returns></returns>
        public async Task DeleteEntity(string id) {
            
            // elimina en la base de datos.
            await Store.RemoveByIdAsync(id, new Microsoft.Azure.Documents.Client.RequestOptions { 
                PartitionKey = new Microsoft.Azure.Documents.PartitionKey("CosmosEntityName")
            });
        }


        public async Task<T2> SingleQuery<T2>(string query, params object[] args) 
        {
            
            var result = await Store.QuerySingleAsync<T2>(string.Format(query, args));
            return result;
        }

        public async Task<IEnumerable<T2>> MultipleQuery<T2>(string query, params object[] args) 
        {
            
            var result = await Store.QueryMultipleAsync<T2>(string.Format(query, args));
            return result;
        }



    }

}