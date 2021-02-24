using Microsoft.Azure.Cosmos;
using trifenix.model;

namespace trifenix.Interfaces
{
    /// <summary>
    ///  Define el nombre del contedor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerContext<T> where T : DocumentDb
    {
        /// <summary>
        /// Nombre de contenedor
        /// </summary>
        string ContainerName { get; }
    }
}
