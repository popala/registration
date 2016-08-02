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
    class ModelCategory {
        public int id;
        public String code;
        public String name;
        public String fullName;
        public String modelClass;
        public int displayOrder;

        public ModelCategory(int id, String code, String name, String modelClass, int displayOrder) {
            this.id = id;
            this.code = code;
            this.name = name;
            this.fullName = String.Format("{0} ({1})", name, code);
            this.modelClass = modelClass;
            this.displayOrder = displayOrder;
        }
    }
}
