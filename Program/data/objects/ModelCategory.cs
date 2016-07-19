using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class ModelCategory {
        public long id;
        public String code;
        public String name;
        public String fullName;
        public String modelClass;
        public int displayOrder;

        public ModelCategory(long id, String code, String name, String modelClass, int displayOrder) {
            this.id = id;
            this.code = code;
            this.name = name;
            this.fullName = String.Format("{0} ({1})", name, code);
            this.modelClass = modelClass;
            this.displayOrder = displayOrder;
        }
    }
}
