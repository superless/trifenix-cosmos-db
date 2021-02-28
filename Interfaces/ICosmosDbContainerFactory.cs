using System.Threading.Tasks;

namespace trifenix.Interfaces
{
    /// <summary>
    /// Interfaz de creacion de contenedor
    /// </summary>
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        /// Retorna un contenedor
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);

        /// <summary>
        /// Garantiza la creacion de BD
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();
    }
}
