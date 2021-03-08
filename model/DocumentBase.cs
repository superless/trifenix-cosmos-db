using Cosmonaut.Attributes;
using trifenix.connect.model;

namespace trifenix.connect.db.model
{
    /// <summary>
    /// Entidad base para modelos de bases de datos basados en cosmonaut.
    /// </summary>
    public abstract class DocumentBase : DocumentDb
    {


        /// <summary>
        /// Partición dentro de base de datos documental,
        /// esto permite a través de un indice particionar un segmento del indice de la base de datos
        /// por nombre de la entidad o nombre de la clase.
        /// </summary>        
        [CosmosPartitionKey]
        public string CosmosEntityName { get; set; }

        /// <summary>
        /// Asigna el nombre de la clase como partición.
        /// </summary>
        protected DocumentBase()
        {
            CosmosEntityName = GetType().Name;
        }

    }

}