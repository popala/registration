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
