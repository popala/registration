using System;

namespace Rejestracja
{
    class WinningEntry
    {
        public long resultId;
        public String firstName;
        public String lastName;
        public String ageGroup;
        public String modelName;
        public String modelClass;
        public String modelCategory;

        public long? awardId;
        public String awardTitle;
        public int? place;

        public WinningEntry(long resultId, String firstName, String lastName, String ageGroup, String modelName, String modelClass, String modelCategory, int place)
        {
            this.resultId = resultId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelClass = modelClass;
            this.modelCategory = modelCategory;
            this.place = place;
        }

        public WinningEntry(long resultId, String firstName, String lastName, String ageGroup, String modelName, String modelClass, String modelCategory, long awardId, String awardTitle)
        {
            this.resultId = resultId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.ageGroup = ageGroup;
            this.modelName = modelName;
            this.modelClass = modelClass;
            this.modelCategory = modelCategory;
            this.awardId = awardId;
            this.awardTitle = awardTitle;
        }
    }
}
