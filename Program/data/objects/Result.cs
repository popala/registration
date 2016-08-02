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
namespace Rejestracja.Data.Objects {
    class Result {
        public int resultId;
        public int place;
        public RegistrationEntry entry;
        public Award award;

        public Result(int resultId, RegistrationEntry entry, Award award) {
            this.resultId = resultId;
            this.entry = entry;
            this.award = award;
            this.place = -1;
        }

        public Result(int resultId, RegistrationEntry entry, int place) {
            this.resultId = resultId;
            this.entry = entry;
            this.award = null;
            this.place = place;
        }
    }
}
