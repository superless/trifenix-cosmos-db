﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.connect.db.cosmos.exceptions;
using trifenix.connect.interfaces.db.cosmos;
using trifenix.connect.arguments;
using trifenix.model;
using trifenix.Interfaces;

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


        //TODO: Consultar como utilizar las propiedades de argument segun nuevo esquema
/*
        /// <summary>
        /// Implementación de operaciones de base de datos para cosmosDb
        /// </summary>
        /// <param name="args">identificaciones de cosmosDb</param>
        public MainGenericDb(CosmosDbArguments args) {
            var storeSettings = new CosmosStoreSettings(args.NameDb, args.EndPointUrl, args.PrimaryKey);
            Store = new CosmosStore<T>(storeSettings);
            
        }*/

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Microsoft.Azure.Cosmos.Container _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cosmosDbContainerFactory"></param>
        public MainGenericDb(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        //TODO: Llamar a valor de DocumentPartition
             
        /// <summary>
        /// Crea o actualiza una entidad 
        /// Upsert, Añade o actualiza
        /// </summary>
        /// <param name="entity">entidad a guardar</param>
        /// <returns>identificador de la entidad a aguardar</returns>
        public async Task<string> CreateUpdate(T entity) {

            /*
            if(string.IsNullOrWhiteSpace(entity.id))
            {
                result = await _container.CreateItemAsync<T>(item, ResolvePartitionKey(item.Id));
                if (!result.IsSuccess)
                    throw result.Exception;
                return result.Entity.Id;
            }
            else
            {
                try
                {
                    ItemResponse<T> result = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                    return result.Resource.Id;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                }
            */

            // el elemento a guardar debe tener un id.
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new NonIdException<DocumentDb>(entity);

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

            /*
            await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
            */

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