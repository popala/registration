using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {

    class ModelerComparer : IEqualityComparer<Modeler> {

        public bool Equals(Modeler x, Modeler y) {

            int score = 0;

            if(x.firstName.Equals(y.firstName, StringComparison.CurrentCultureIgnoreCase))
                score++;
            if(x.lastName.Equals(y.lastName, StringComparison.CurrentCultureIgnoreCase))
                score++;
            if((x.yearOfBirth > 0 && y.yearOfBirth > 0) && x.yearOfBirth == y.yearOfBirth)
                score++;
            if(x.email.Equals(y.email, StringComparison.CurrentCultureIgnoreCase))
                score++;

            return (score > 2);
        }

        public int GetHashCode(Modeler modeler) {
            if(modeler == null) {
                return 0;
            }
            //return (modeler.firstName.Trim().ToLower() + modeler.lastName.Trim().ToLower()).GetHashCode();
            return ("1").GetHashCode();
        }
    }

    class Modeler {
        public int id;
        public String firstName;
        public String lastName;
        public String clubName;
        public int yearOfBirth;
        public String email;

        public const int MAX_NAME_LENGTH = 128;
        public const int MAX_EMAIL_LENGTH = 128;

        public Modeler() {
        }

        public Modeler(String firstName, String lastName, String clubName, int yearOfBirth, String email) {
            this.id = -1;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.yearOfBirth = yearOfBirth;
            this.email = email;
        }

        public Modeler(int id, String firstName, String lastName, String clubName, int yearOfBirth, String email) {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.yearOfBirth = yearOfBirth;
            this.email = email;
        }

        public bool IsIdentical(Modeler m) {
            if(m == null) {
                return false;
            }

            return (
                this.id == m.id &&
                this.firstName.Equals(m.firstName, StringComparison.CurrentCultureIgnoreCase) &&
                this.lastName.Equals(m.lastName, StringComparison.CurrentCultureIgnoreCase) &&
                this.clubName.Equals(m.clubName, StringComparison.CurrentCultureIgnoreCase) &&
                this.yearOfBirth == m.yearOfBirth &&
                this.email.Equals(m.email, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool Matches(Modeler other) {
            if(other == null) {
                return false;
            }
            int score = 0;

            if(this.firstName.Equals(other.firstName, StringComparison.CurrentCultureIgnoreCase))
                score++;
            if(this.lastName.Equals(other.lastName, StringComparison.CurrentCultureIgnoreCase))
                score++;
            if((this.yearOfBirth > 0 && other.yearOfBirth > 0) && this.yearOfBirth == other.yearOfBirth)
                score++;
            if(this.email.Equals(other.email, StringComparison.CurrentCultureIgnoreCase))
                score++;

            return (score > 2);
        }
    }
}
