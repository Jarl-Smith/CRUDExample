
using Entities;

namespace ServciceContracts.DataTransferObject {
    public class PersonResponse {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }
    }

    public static class PersonExtensions {
        public static PersonResponse ToPersonResponse(this Person person) {
            return new PersonResponse() { PersonID = person.PersonID, PersonName = person.PersonName, Email = person.Email, DateOfBirth = person.DateOfBirth, Gender = person.Gender, CountryID = person.CountryID, Address = person.Address, ReceiveNewsLetters = person.ReceiveNewsLetters, Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null };
        }
    }
}
