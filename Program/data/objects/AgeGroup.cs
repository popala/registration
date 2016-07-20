using System;

namespace Rejestracja.Data.Objects {
    class AgeGroup {
        public long id;
        public String name;
        public int upperAge;
        public int bottomAge;

        public AgeGroup(long id, String name, int upperAge, int bottomAge) {
            this.id = id;
            this.name = name;
            this.upperAge = upperAge;
            this.bottomAge = bottomAge;
        }
    }
}
