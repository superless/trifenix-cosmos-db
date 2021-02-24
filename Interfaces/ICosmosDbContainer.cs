using Microsoft.Azure.Cosmos;

namespace trifenix.Interfaces
{
    /// <summary>
    /// Interfaz de contenedor
    /// </summary>
    public interface ICosmosDbContainer
    {
        /// <summary>
        /// Contenedor
        /// </summary>
        Container _container { get; }
    }
}