using System;

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
