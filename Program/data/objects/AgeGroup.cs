using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.objects {
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
