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
using System.Collections.Generic;

namespace Rejestracja.Data.Objects {
    class Class {

        public const String IMPORTED_CLASS_TITLE = "Kategorie z importu";

        public enum ScoringCardType {
            SeparateAgeGroupSeparateCategory = 0,
            SeparateAgeGroupCombinedCategory = 1,
            AllCombined = 2
        }

        public enum ClassificationType {
            Places = 0,
            Medals = 1,
            Distinctions = 2
        }

        public int id;
        public String name;
        public bool printCustomRegistrationCard;
        public bool useCustomAgeGroups;
        public String registrationCardTemplate;
        public String judgingFormTemplate;
        public String diplomaTemplate;
        public ScoringCardType scoringCardType;
        public List<Category> categories;
        public List<AgeGroup> ageGroups;
        public ClassificationType classificationType;
        public bool usePointRange;
        public int[] pointRanges;
        public bool useDistinctions;

        public const int MAX_NAME_LENGTH = 128;

        public Class(int id, String name) {
            this.id = id;
            this.name = name;
            this.categories = new List<Category>();
            
            this.printCustomRegistrationCard = false;
            this.useCustomAgeGroups = false;
            this.registrationCardTemplate = null;
            this.judgingFormTemplate = null;
            this.diplomaTemplate = null;
            this.scoringCardType = ScoringCardType.AllCombined;
        }

        public Class(int id, String name, 
                String registrationCardTemplate, String judgingFormTemplate, String diplomaTemplate, 
                bool useCustomAgeGroups, ScoringCardType scoringCardType, ClassificationType classificationType,
                bool usePointRange, bool useDistinctions) {
            this.id = id;
            this.name = name;
            this.categories = new List<Category>();
            this.ageGroups = new List<AgeGroup>();

            this.printCustomRegistrationCard = (!String.IsNullOrWhiteSpace(registrationCardTemplate));
            this.useCustomAgeGroups = useCustomAgeGroups;
            this.registrationCardTemplate = registrationCardTemplate;
            this.judgingFormTemplate = judgingFormTemplate;
            this.diplomaTemplate = diplomaTemplate;
            this.scoringCardType = scoringCardType;

            this.classificationType = classificationType;
            this.usePointRange = usePointRange;
            this.useDistinctions = useDistinctions;
        }
    }
}
