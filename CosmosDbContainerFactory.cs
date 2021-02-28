using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trifenix.Interfaces;

namespace trifenix
{
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly List<string> _containers;

        public CosmosDbContainerFactory(CosmosClient cosmosClient,
                                   string databaseName,
                                   List<string> containers)
        {
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _containers = containers ?? throw new ArgumentNullException(nameof(containers));
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        }

        public ICosmosDbContainer GetContainer(string containerName)
        {
            if (_containers == null)
            {
                throw new ArgumentException($"Container no encontrado: {containerName}");
            }

            return new CosmosDbContainer(_cosmosClient, _databaseName, containerName);
        }

        public async Task EnsureDbSetupAsync()
        {
            Microsoft.Azure.Cosmos.DatabaseResponse database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);
        }
    }
}