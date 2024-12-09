using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DataTransferObject {
    public class PersonAddRequest {
        [Required(ErrorMessage = "PersonName should not be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage ="Please select gender of a person")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage ="Please select a country")]
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson() {
            return new Person() { PersonName = this.PersonName, Email = this.Email, DateOfBirth = this.DateOfBirth, Gender = this.Gender.ToString(), CountryID = this.CountryID, Address = this.Address, ReceiveNewsLetters = this.ReceiveNewsLetters };
        }
    }
}
