using System;

namespace Rejestracja.Data.Objects {
    class Award {
        public int id;
        public String title;
        public int displayOrder;

        public const int TITLE_MAX_LENGTH = 256;

        public Award(int id, String title, int displayOrder) {
            this.id = id;
            this.title = title;
            this.displayOrder = displayOrder;
        }
    }
}
