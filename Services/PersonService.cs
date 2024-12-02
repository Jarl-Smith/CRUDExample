﻿using Entities;
using ServciceContracts;
using ServciceContracts.DataTransferObject;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services {
    public class PersonService : IPersonService {

        private readonly List<Person> _person;
        private readonly ICountryService _countryService;

        public PersonService() {
            _person = new List<Person>();
            _countryService = new CountryService();
        }

        private PersonResponse convertPersonToPersonResponse(Person person) {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryByCountryID(personResponse.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest) {
            if(personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _person.Add(person);
            return convertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPerson() {
            return _person.Select(person => person.ToPersonResponse()).ToList();
        }

        public List<PersonResponse> GetFilterPerson(string searchBy, string? searchString) {
            List<PersonResponse> allPerson = GetAllPerson();
            List<PersonResponse> matchingPerson = allPerson;
            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) { return allPerson; }

            switch(searchBy) {
                case nameof(Person.PersonName):
                    matchingPerson = allPerson.Where(
                        (person) => {
                            if(string.IsNullOrEmpty(person.PersonName)) {
                                return true;
                            } else {
                                return person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase);
                            }
                        }).ToList();
                    break;
                case nameof(Person.Email):
                    matchingPerson = allPerson.Where(
                        (person) => {
                            if(string.IsNullOrEmpty(person.Email)) {
                                return true;
                            } else {
                                return person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase);
                            }
                        }).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPerson = allPerson.Where(
                        (person) => {
                            if(person.DateOfBirth == null) {
                                return false;
                            } else {
                                return person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase);
                            }
                        }).ToList();
                    break;
                default:
                    break;
            }
            return matchingPerson;
        }

        public PersonResponse? GetPersonByPersonID(Guid? guid) {
            return guid != null ? _person.Where(person => person.PersonID == guid).FirstOrDefault()?.ToPersonResponse() : null;
        }

        public List<PersonResponse> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption) {
            List<PersonResponse> sortPerson = (sortBy, sortOrderOption)
            switch {
                (nameof(PersonResponse.PersonName), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.Age), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Age).ToList(),
                _ => allPerson
            };
            return sortPerson;
        }
    }
}
