using Rejestracja.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Dao {
    interface IAgeGroupDao {
        bool exists(int upperAge, int classId);
        AgeGroup getOlderAgeGroup(String ageGroupName, int classId);
        List<AgeGroup> getList(int classId);
        int add(String name, int age, int classId);
        void deleteClassAgeGroups(int classId);
        void delete(int id);
    }
}
