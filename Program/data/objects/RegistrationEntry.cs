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
        public long entryId;
        public DateTime timeStamp;
        public String email;
        public String firstName;
        public String lastName;
        public String clubName;
        public String ageGroup;
        public String modelName;
        public String modelCategory;
        public long modelCategoryId;
        public String modelScale;
        public String modelPublisher;
        public String modelClass;
        public int yearOfBirth;
        public int place;
        public bool skipErrorValidation;

        public RegistrationEntry() {

        }

        public RegistrationEntry(long entryId, DateTime timeStamp, String email, String firstName, String lastName, String clubName, String ageGroup,
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId,
                                int yearOfBirth, bool skipErrorValidation) {
            this.entryId = entryId;
            this.timeStamp = timeStamp;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
            this.skipErrorValidation = skipErrorValidation;
        }

        public RegistrationEntry(long entryId, String email, String firstName, String lastName, String clubName, String ageGroup,
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId, int yearOfBirth) {
            this.entryId = entryId;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(DateTime timeStamp, String email, String firstName, String lastName, String clubName, String ageGroup,
                                String modelName, String modelClass, String modelScale, String modelPublisher, String modelCategory, long modelCategoryId, int yearOfBirth) {
            this.timeStamp = timeStamp;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            this.clubName = clubName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelCategory = modelCategory;
            this.modelCategoryId = modelCategoryId;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.modelClass = modelClass;
            this.yearOfBirth = yearOfBirth;
        }

        public RegistrationEntry(long entryId, String ageGroup, String modelName, String modelClass, String modelScale, String modelPublisher, int place) {
            this.entryId = entryId;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelClass = modelClass;
            this.modelScale = modelScale;
            this.modelPublisher = modelPublisher;
            this.place = place;
        }

        public String ToCsvString() {
            String [] props = new String[] {
                entryId.ToString(), timeStamp.ToString(), email, firstName, lastName, clubName, ageGroup, modelName, modelCategory, modelCategoryId.ToString(),
                modelScale, modelPublisher, modelClass, yearOfBirth.ToString(), place.ToString(), skipErrorValidation.ToString()
            };
            return "\"" + String.Join("\",\"", props) + "\"";
        }
    }
}
