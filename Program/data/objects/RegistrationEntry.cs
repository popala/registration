/*
 * Copyright (C) 2016 Paweł Opała https://github.com/popala/registration
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program.  If not, see http://www.gnu.org/licenses/.
 */
using System;

namespace Rejestracja.Data.Objects {
    class RegistrationEntry {
        public int registrationId;
        public DateTime timeStamp;
        public String ageGroupName;

        public int categoryId;
        public String categoryName;
        public String className;

        public int modelerId;
        public String firstName;
        public String lastName;
        public String clubName;
        public int yearOfBirth;
        public String email;

        public int modelId;
        public String modelName;
        public String modelPublisher;
        public String modelScale;
        
        public int place;

        public RegistrationEntry() {
        }

        public RegistrationEntry(
                int registrationId, DateTime timeStamp, String ageGroupName,
                int categoryId, String categoryName, String className,
                int modelerId, String firstName, String lastName, String clubName, int yearOfBirth, String email,  
                int modelId, String modelName, String modelPublisher, String modelScale) {

            this.registrationId = registrationId;
            this.timeStamp = timeStamp;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroupName = ageGroupName;
            this.modelName = modelName;
            this.categoryName = categoryName;
            this.categoryId = categoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.className = className;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(int registrationId, String ageGroupName, String modelName, String className, String modelScale, String modelPublisher, int place) {
            this.registrationId = registrationId;
            this.ageGroupName = ageGroupName;
            this.modelName = modelName;
            this.className = className;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.place = place;
        }

        public String ToCsvString() {
            String [] props = new String[] {
                registrationId.ToString(), timeStamp.ToString(), email, firstName, lastName, clubName, ageGroupName, modelName, categoryName, categoryId.ToString(),
                modelScale, modelPublisher, className, yearOfBirth.ToString(), place.ToString()
            };
            return "\"" + String.Join("\",\"", props) + "\"";
        }
    }
}
