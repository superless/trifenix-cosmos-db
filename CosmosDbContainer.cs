using Microsoft.Azure.Cosmos;
using trifenix.Interfaces;

namespace trifenix
{
    /// <summary>
    /// Asignacion de cliente, nombre de contenedor y nombre de base de datos
    /// </summary>
    public class CosmosDbContainer : ICosmosDbContainer
    {
        /// <summary>
        /// Asignacion de container
        /// </summary>
        public Container _container { get; }

        /// <summary>
        /// Asignacion de nombre de base de datos y nombre de container
        /// </summary>
        /// <param name="cosmosClient"></param>
        /// <param name="databaseName"></param>
        /// <param name="containerName"></param>
        public CosmosDbContainer(CosmosClient cosmosClient,
                                 string databaseName,
                                 string containerName)
        {
            this._container = cosmosClient.GetContainer(databaseName, containerName);
        }
    }
}