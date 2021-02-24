using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.model;
using trifenix.Interfaces;
using Microsoft.Azure.Cosmos;
using trifenix.exception;

namespace trifenix.connect.db.cosmos
{
    /// <summary>
    /// Repositorio encargado de almacenar la base de datos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MainGenericDb<T> : IMainGenericDb<T>, IContainerContext<T> where T : DocumentDb
    {
        /// <summary>
        /// Nombre del contenedor
        /// </summary>
        public abstract string ContainerName { get; }

        /// <summary>
        /// Particion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string name);

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Microsoft.Azure.Cosmos.Container _container;

        /// <summary>
        /// Asigna el container
        /// </summary>
        /// <param name="cosmosDbContainerFactory"></param>
        public MainGenericDb(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        /// <summary>
        /// Crea o actualiza datos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> CreateUpdate(T entity)
        {
            // el elemento a guardar debe tener un id.
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                var result = await _container.CreateItemAsync<T>(entity, ResolvePartitionKey(entity.Id));

                // si no tiene exito, lanza excepción.
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new CustomException("Ingreso no tuvo exito");

                return result.Resource.Id;
            }
            else
            {
                // inserta o actualiza.
                var result = await _container.UpsertItemAsync<T>(entity, ResolvePartitionKey(entity.Id));

                // si no tiene exito, lanza excepción.
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new CustomException("Ingreso no tuvo exito");

                return result.Resource.Id;
            }
        }

        /// <summary>
        /// Obtiene una entidad desde el store
        /// </summary>
        /// <param name="uniqueId">identificador de la entidad</param>
        /// <returns></returns>
        public async Task<T> GetEntity(string uniqueId)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(uniqueId, ResolvePartitionKey(uniqueId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        /// <param name="id">identificador de la entidad a eliminar</param>
        /// <returns></returns>
        public async Task DeleteEntity(string id)
        {

            // elimina en la base de datos.
            await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
        }

        public Task<T2> SingleQuery<T2>(string query, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T2>> MultipleQuery<T2>(string query, params object[] args)
        {
            var result = await Store.QuerySingleAsync<T2>(string.Format(query, args));
            return result;
        }

        /*
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
        */
    }
}
