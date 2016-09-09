using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Data.Objects {
    class Registration {
        public int id;
        public DateTime timeStamp;
        public int modelId;
        public int categoryId;
        public String categoryName;
        public String ageGroupName;

        public Registration() {
        }

        public Registration(DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName) {
            this.id = -1;
            this.timeStamp = timeStamp;
            this.modelId = modelId;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.ageGroupName = ageGroupName;
        }

        public Registration(int id, DateTime timeStamp, int modelId, int categoryId, String categoryName, String ageGroupName) {
            this.id = id;
            this.timeStamp = timeStamp;
            this.modelId = modelId;
            this.categoryId = categoryId;
            this.categoryName = categoryName;
            this.ageGroupName = ageGroupName;
        }
    }
}
