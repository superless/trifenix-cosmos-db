using System;
using System.Collections.Generic;

namespace trifenix.connect.db.cosmos.exceptions {

    /// <summary>
    /// Excepción de validación con una colección de los errores.
    /// </summary>
    public class ValidationException : Exception
    {
        public List<string> ErrorMessages;

    }
}