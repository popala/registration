using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class Modeler {
        public int id;
        public String firstName;
        public String lastName;
        public String clubName;
        public int yearOfBirth;
        public String email;

        public const int MAX_NAME_LENGTH = 128;
        public const int MAX_EMAIL_LENGTH = 128;

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
    }
}
