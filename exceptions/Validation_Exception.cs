using System;
using System.Collections.Generic;

namespace trifenix.connect.db.cosmos.exceptions {
    public class Validation_Exception : Exception {

        public List<string> ErrorMessages;

    }
}