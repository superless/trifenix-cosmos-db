using Microsoft.Azure.Cosmos;

namespace trifenix.Interfaces
{
    /// <summary>
    /// Generador de container
    /// </summary>
    public interface ICosmosDbContainer
    {
        /// <summary>
        /// Genera el container
        /// </summary>
        Container _container { get; }
    }
}
