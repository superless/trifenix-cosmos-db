using Cosmonaut;
using System.Collections.Generic;
using System.Threading.Tasks;

using trifenix.connect.entities.cosmos;
using trifenix.connect.arguments;

namespace trifenix.connect.db.cosmos
{
    /// <summary>
    /// Consultas base para cosmosdb
    /// </summary>
    public class BaseQueries {

        public readonly CosmosDbArguments DbArguments;
        
        public BaseQueries(CosmosDbArguments dbArguments) {
            DbArguments = dbArguments;
            
        }

        
        /// <summary>
        /// Consulta que regresa solo un elemento de cosmos db
        /// </summary>
        /// <param name="query">consulta</param>
        /// <param name="args">argumentos</param>
        /// <typeparam name="TDOCUMENT">Tipo de elemento a obtener, debe heredar de documentBase</typeparam>
        /// <typeparam name="T">Tipo de resultado</typeparam>
        /// <returns></returns>
        public async Task<T> SingleQuery<TDOCUMENT,T>(string query, params object[] args) where TDOCUMENT : DocumentBase {

            var result = await new MainGenericDb<TDOCUMENT>(DbArguments).SingleQuery<T>(query, args);
            return result;
        }

        /// <summary>
        /// Consulta que retorna multiples elementos de un tipo
        /// </summary>
        /// <param name="query">consulta</param>
        /// <param name="args">argumentos</param>
        /// <typeparam name="TDOCUMENT">documento</typeparam>
        /// <typeparam name="T">Tipo de Respuesta</typeparam>
        /// <returns>Collección de resultados desde cosmosdb</returns>
        public async Task<IEnumerable<T>> MultipleQuery<TDOCUMENT, T>(string query, params object[] args) where TDOCUMENT : DocumentBase {
            
            
            var result = await new MainGenericDb<TDOCUMENT>(DbArguments).MultipleQuery<T>(string.Format(query, args));
            return result;
        }

        

    }

}