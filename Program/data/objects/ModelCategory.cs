using System;

namespace Rejestracja.Data.Objects {
    class ModelCategory {
        public int id;
        public String code;
        public String name;
        public String fullName;
        public String modelClass;
        public int displayOrder;

        public ModelCategory(int id, String code, String name, String modelClass, int displayOrder) {
            this.id = id;
            this.code = code;
            this.name = name;
            this.fullName = String.Format("{0} ({1})", name, code);
            this.modelClass = modelClass;
            this.displayOrder = displayOrder;
        }
    }
}
