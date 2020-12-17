using System;
using trifenix.connect.entities.cosmos;

namespace trifenix.connect.db.cosmos.exceptions {
    public class NonIdException<T> : BaseException<T> where T:DocumentBase {
        public NonIdException(T docBase) : base(docBase) { }

        public override string Message => $"el elemento de tipo {DbObject.GetType()} no  tiene id";
    }

    public abstract class BaseException<T> : Exception where T:DocumentBase {
        public BaseException(T docBase) {
            DbObject = docBase;
        }

        public T DbObject { get; }
    }
}