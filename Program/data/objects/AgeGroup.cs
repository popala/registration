using System;

namespace Rejestracja.Data.Objects {
    class AgeGroup {

        public const int NAME_MAX_LENGTH = 64;

        public int id;
        public String name;
        public int upperAge;
        public int bottomAge;

        public AgeGroup(int id, String name, int upperAge, int bottomAge) {
            this.id = id;
            this.name = name;
            this.upperAge = upperAge;
            this.bottomAge = bottomAge;
        }
    }
}
