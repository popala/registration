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
    class RegistrationEntry {

        public Registration registration;
        public Category category;
        public List<Registration> importedRegistration;
        public Modeler modeler;
        public Model model;

        public RegistrationEntry() {
            this.registration = new Registration();
            this.model = new Model();
            this.modeler = new Modeler();
            this.category = new Category();
            this.importedRegistration = new List<Registration>();
        }

        public RegistrationEntry(Registration registration, Category category, Modeler modeler, Model model) {
            this.registration = registration;
            this.model = model;
            this.modeler = modeler;
            this.category = category;
        }

        public String ToCsvString() {
            String [] props = new String[] {
                registration.ToString(), model.ToString(), modeler.ToString(), category.ToString()
            };
            return "\"" + String.Join("\",\"", props) + "\"";
        }
    }
}
