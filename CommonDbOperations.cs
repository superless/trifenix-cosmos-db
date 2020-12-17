using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using trifenix.connect.entities.cosmos;
using trifenix.connect.interfaces.db.cosmos;

namespace trifenix.connect.db.cosmos
{

    /// <summary>
    /// Operaciones comunes de cosmosdb,
    /// Estas operaciones son debido a que cosmonaut usa un método estático
    /// para transformar un IQueryable a una lista o colección de elementos
    /// este método no es posible testearlo, 
    /// para poder hacerlo se ha creado esta clase.
    /// </summary>
    /// <typeparam name="T">Tipo de dato a operar, un objeto del model, ej : producto</typeparam>
    public class CommonDbOperations<T> : ICommonDbOperations<T> where T : DocumentBase
    {
        /// <summary>
        /// Retorna el primer elemento de acuerdo a la consulta
        /// </summary>
        /// <param name="list">listado obtenido desde Cosmonaut</param>
        /// <param name="predicate">predicado para filtrar</param>
        /// <returns>Usa el método estático de Cosmonaut para retornar la lista implementada</returns>
        public async Task<T> FirstOrDefaultAsync(IQueryable<T> list, Expression<Func<T, bool>> predicate) {
            if (list == null) return (T)(object)null;
            return await list.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retorna una colección de elementos de acuerdo a la consulta
        /// </summary>
        /// <param name="list">listado obtenido desde Cosmonaut</param>
        /// <returns>Usa el método estático de Cosmonaut para retornar la lista implementada</returns>
        public async Task<List<T>> TolistAsync(IQueryable<T> list) {
            if (list == null)
                return new List<T>();
            return await list.ToListAsync();
        }

        /// <summary>
        /// Retorna una colección de elementos de acuerdo a la consulta
        /// y retorna de manera páginada
        /// </summary>
        /// <param name="list">listado obtenido desde Cosmonaut</param>        
        /// <param name="page">página a retornar</param>
        /// <param name="totalElementsOnPage">total de elementos por página</param>
        /// <returns></returns>
        public IQueryable<T> WithPagination(IQueryable<T> list, int page, int totalElementsOnPage) {
            return list.WithPagination(page, totalElementsOnPage);
        }

    }
}