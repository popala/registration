
namespace Rejestracja.Data.Objects {
    class FileImportFieldMap {
        public int TimeStamp { get; set; }
        public int Email { get; set; }
        public int FirstName { get; set; }
        public int LastName { get; set; }
        public int ClubName { get; set; }
        public int AgeGroup { get; set; }
        public bool CalculateAgeGroup { get; set; }
        public int ModelName { get; set; }
        public int [] ModelCategory { get; set; }
        public int ModelScale { get; set; }
        public int ModelPublisher { get; set; }
        public int ModelClass { get; set; }
        public bool DeriveClassFromCategory { get; set; }
        public int YearOfBirth { get; set; }
    }
}
