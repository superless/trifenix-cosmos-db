using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using trifenix.connect.arguments;
using trifenix.Interfaces;

namespace trifenix
{
    /// <summary>
    /// Interfaz a la cual se le extiende un metodo
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Se incorpora una coleccion de servicios a Cosmos Db
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="primaryKey"></param>
        /// <param name="name"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string name,
                                                     List<ContainerInfo> containers)
        {
            Microsoft.Azure.Cosmos.CosmosClient cliente = new Microsoft.Azure.Cosmos.CosmosClient(endpointUrl, primaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(cliente, name, containers);

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;

        }
    }
}
