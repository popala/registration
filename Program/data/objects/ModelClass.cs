using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class ModelClass {
        public long id;
        public String name;
        public const int MAX_NAME_LENGTH = 128;

        public ModelClass(long id, String name) {
            this.id = id;
            this.name = name;
        }

        public ModelClass(String name) {
            this.name = name;
        }
    }
}
