using Microsoft.Azure.Cosmos;
using trifenix.Interfaces;

namespace trifenix
{
    /// <summary>
    /// Asignacion a container un cliente
    /// </summary>
    public class CosmosDbContainer : ICosmosDbContainer
    {
        /// <summary>
        /// Se asigna el container a agregar
        /// </summary>
        public Container _container { get; }

        /// <summary>
        /// Se asigna el cliente al container especificado
        /// </summary>
        /// <param name="cosmosClient"></param>
        /// <param name="nameDb"></param>
        /// <param name="containerName"></param>
        public CosmosDbContainer(CosmosClient cosmosClient,
                                string nameDb,
                                string containerName)
        {
            this._container = cosmosClient.GetContainer(nameDb, containerName);
        }
    }
}
