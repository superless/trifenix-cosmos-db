using System.Threading.Tasks;

namespace trifenix.Interfaces
{
    /// <summary>
    /// Interfaz encargada de factorizar el container
    /// </summary>
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        /// Verifica si el container se le asigno un nombre
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);

        /// <summary>
        /// Garantiza la conexion
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();
    }
}
