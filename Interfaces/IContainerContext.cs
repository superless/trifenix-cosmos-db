using Microsoft.Azure.Cosmos;
using trifenix.model;

namespace trifenix.Interfaces
{
    /// <summary>
    /// Interfaz encargada de generar nombre del container y particion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerContext<T> where T : DocumentDb
    {
        /// <summary>
        /// Variable que determina el nombre del container
        /// </summary>
        string ContainerName { get; }

        /// <summary>
        /// Particion a generar
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PartitionKey ResolvePartitionKey(string name);
    }
}
