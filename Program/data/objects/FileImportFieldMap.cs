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
    class FileImportFieldMap {
        public int TimeStamp { get; set; }
        public int Email { get; set; }
        public int FirstName { get; set; }
        public int LastName { get; set; }
        public int ClubName { get; set; }
        public int AgeGroup { get; set; }
        public bool CalculateAgeGroup { get; set; }
        public int ModelName { get; set; }
        public int [] ModelCategory { get; set; }
        public int ModelScale { get; set; }
        public int ModelPublisher { get; set; }
        public int ModelClass { get; set; }
        public bool DeriveClassFromCategory { get; set; }
        public int YearOfBirth { get; set; }
    }
}
