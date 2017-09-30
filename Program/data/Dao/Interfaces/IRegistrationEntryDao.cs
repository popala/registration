﻿/*
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

namespace Rejestracja.Data.Dao
{
    interface IRegistrationEntryDao
    {
        RegistrationEntry get(int entryId);
        List<RegistrationEntry> getRegistrationForModel(int modelId);
        IEnumerable<String[]> getList(String searchValue, int sortField, bool sortAscending);
        IEnumerable<String[]> getGrouppedList();
        IEnumerable<String[]> getGrouppedList(String searchValue);
        List<RegistrationEntry> getListForJudging();
        IEnumerable<String[]> getListForMergingCategories(int maxEntryCount);
        void mergeAgeGroupsInCategory(long modelCategoryId, String sourceAgeGroup, String targetAgeGroup);
        void changeCategory(int registrationId, int categoryId);
        int add(DateTime timeStamp, int modelId, int categoryId, String ageGroupName);
        bool delete(long registrationId);
        List<KeyValuePair<string, string>> getRegistrationStats();
    }
}
