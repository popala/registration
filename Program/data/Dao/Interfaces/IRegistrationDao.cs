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
    interface IRegistrationDao {
        IEnumerable<Registration> getList();
        IEnumerable<Registration> getList(int modelId);
        int add(DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName);
        int add(Registration registration);
        Registration get(int modelId, int categoryId);
        Registration get(int registrationId);
        void delete(int registrationId);
        void delete(int modelId, String categoryName);
        void delete(int modelId, int categoryId);
    }
}
