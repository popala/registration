using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class Award {
        public long id;
        public String title;
        public long displayOrder;

        public const int TITLE_MAX_LENGTH = 256;

        public Award(long id, String title, long displayOrder) {
            this.id = id;
            this.title = title;
            this.displayOrder = displayOrder;
        }
    }
}
