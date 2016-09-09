using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class Model {
        public int id;
        public String name;
        public String publisher;
        public String scale;
        public int modelerId;

        public const int MAX_NAME_LENGTH = 256;

        public Model() {
        }

        public Model(String name, String publisher, String scale) {
            this.id = -1;
            this.name = name;
            this.publisher = publisher;
            this.scale = scale;
            this.modelerId = -1;
        }

        public Model(String name, String publisher, String scale, int modelerId) {
            this.id = -1;
            this.name = name;
            this.publisher = publisher;
            this.scale = scale;
            this.modelerId = modelerId;
        }

        public Model(int id, String name, String publisher, String scale, int modelerId) {
            this.id = id;
            this.name = name;
            this.publisher = publisher;
            this.scale = scale;
            this.modelerId = modelerId;
        }
    }
}
