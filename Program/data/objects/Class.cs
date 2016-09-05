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
    class Class {
        public int id;
        public String name;
        public const int MAX_NAME_LENGTH = 128;

        public Class(int id, String name) {
            this.id = id;
            this.name = name;
        }

        public Class(String name) {
            this.name = name;
        }
    }
}
