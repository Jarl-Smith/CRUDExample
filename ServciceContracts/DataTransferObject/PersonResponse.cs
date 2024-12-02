using Entities;
using ServciceContracts.Enums;
using ServiceContracts.DataTransferObject;

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

        public override bool Equals(object? obj) {
            if(obj.GetType() != typeof(PersonResponse)) {
                return false;
            }
            PersonResponse person = (PersonResponse)obj;
            return person.PersonID == this.PersonID && person.PersonName == this.PersonName && person.Email == this.Email && person.DateOfBirth == this.DateOfBirth && person.Gender == this.Gender && person.CountryID == this.CountryID && person.Country == this.Country && person.Address == this.Address && person.ReceiveNewsLetters == this.ReceiveNewsLetters && person.Age == this.Age;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public PersonUpdateRequest ToPersonUpdateRequest() {
            return new PersonUpdateRequest() { PersonID = this.PersonID, PersonName = this.PersonName, Email = this.Email, DateOfBirth = this.DateOfBirth, Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true), CountryID = this.CountryID, Address = this.Address, ReceiveNewsLetters = this.ReceiveNewsLetters };
        }
    }

    public static class PersonExtensions {
        public static PersonResponse ToPersonResponse(this Person person) {
            return new PersonResponse() { PersonID = person.PersonID, PersonName = person.PersonName, Email = person.Email, DateOfBirth = person.DateOfBirth, Gender = person.Gender, CountryID = person.CountryID, Address = person.Address, ReceiveNewsLetters = person.ReceiveNewsLetters, Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null };
        }
    }
}
