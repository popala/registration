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
using Rejestracja.Data.Objects;
using System;
using System.Collections.Generic;

namespace Rejestracja.Data.Dao {
    interface IModelerDao {
        IEnumerable<Modeler> getList();
        IEnumerable<Modeler> search(String value);
        int add(String firstName, String lastName, String clubName, int yearOfBirth, String email);
        int add(Modeler modeler);
        void update(Modeler modeler);
        void update(int id, String firstName, String lastName, String clubName, int yearOfBirth, String email);
        Modeler get(int id);
        void delete(int id);
    }
}
