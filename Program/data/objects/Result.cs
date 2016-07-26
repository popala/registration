
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
