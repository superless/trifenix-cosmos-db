using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.arguments;
using trifenix.model;
using trifenix.Interfaces;
using Microsoft.Azure.Cosmos;
using trifenix.exception;

namespace trifenix.connect.db.cosmos
{

    /// <summary>
    /// Implementación de operaciones de base de datos cosmosDb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MainGenericDb<T> : IMainGenericDb<T>, IContainerContext<T> where T : DocumentDb {

        /// <summary>
        /// Nombre de contenedor
        /// </summary>
        public abstract string ContainerName { get; }

        public MainGenericDb(CosmosDbArguments args)
        {
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(args.EndPointUrl, args.PrimaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(client, args.NameDb, args.ContainersName);

        }

        private readonly Microsoft.Azure.Cosmos.Container _container;

        /// <summary>
        /// Crea o actualiza una entidad 
        /// Upsert, Añade o actualiza
        /// </summary>
        /// <param name="entity">entidad a guardar</param>
        /// <returns>identificador de la entidad a aguardar</returns>
        public async Task<string> CreateUpdate(T entity) {
         
            if(string.IsNullOrWhiteSpace(entity.Id))
            {

                var result = await _container.CreateItemAsync<T>(entity, new PartitionKey(entity.DocumentPartition));
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CustomException("No se pudo ingresar id");
                }
                return result.Resource.Id;
            }
            else
            {
                try
                {
                    ItemResponse<T> result = await _container.UpsertItemAsync<T>(entity, new PartitionKey(entity.DocumentPartition));
                    return result.Resource.Id;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
            }
        }      

        /// <summary>
        /// Obtiene una entidad desde el store
        /// </summary>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        public async Task<T> GetEntity(string id)
        {
            ItemResponse<T> result = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return result.Resource;
        }
       

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        /// <param name="id">identificador de la entidad a eliminar</param>
        /// <returns></returns>
        public async Task DeleteEntity(string id) 
        {
            await this._container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }


        public async Task<T2> SingleQuery<T2>(string queryString, params object[] args) 
        {
            //TODO: Revisar para una sola Query
            var query = _container.GetItemQueryIterator<T2>(new QueryDefinition(string.Format(queryString, args)));
            return result;
        }

        public async Task<IEnumerable<T2>> MultipleQuery<T2>(string queryString, params object[] args) 
        {
            
            var query = _container.GetItemQueryIterator<T2>(new QueryDefinition(string.Format(queryString, args)));
            List<T2> result = new List<T2>();
            while(query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response.ToList());
            }
            return result;
        }



    }

}