﻿using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DataTransferObject {
    public class PersonUpdateRequest {
        [Required(ErrorMessage = "PersonID can't be blank")]
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "PersonName should not be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson() {
            return new Person() { PersonID = this.PersonID, PersonName = this.PersonName, Email = this.Email, DateOfBirth = this.DateOfBirth, Gender = this.Gender.ToString(), CountryID = this.CountryID, Address = this.Address, ReceiveNewsLetters = this.ReceiveNewsLetters };
        }
    }
}