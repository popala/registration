using System;

namespace Rejestracja.Data.Objects {
    class ModelScale {
        public int id;
        public String name;
        public int displayOrder;

        public ModelScale(int id, String name, int displayOrder) {
            this.id = id;
            this.name = name;
            this.displayOrder = displayOrder;
        }
    }
}
