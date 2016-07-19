using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class ModelScale {
        public long id;
        public String name;
        public int displayOrder;

        public ModelScale(long id, String name, int displayOrder) {
            this.id = id;
            this.name = name;
            this.displayOrder = displayOrder;
        }
    }
}
