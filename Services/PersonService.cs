using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;
using Services.Helpers;
using System;

namespace Services {
    public class PersonService : IPersonService {

        private readonly PersonsDbContext _db;

        public PersonService(PersonsDbContext personsDbContext, ICountryService countryService) {
            _db = personsDbContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest) {
            if(personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPerson() {
            //这里需要注意，必须把数据从数据库转换为内存中的对象才能在Linq表达式中使用自定义的方法，否则会报InvalidOperationException
            List<Person> persons = await _db.Persons.Include(nameof(Person.Country)).ToListAsync();
            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public async Task<List<PersonResponse>> GetFilterPerson(string searchBy, string? searchString) {
            List<PersonResponse> allPerson = await GetAllPerson();
            List<PersonResponse> matchingPerson = allPerson;
            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) { return allPerson; }

            switch(searchBy) {
                case nameof(PersonResponse.PersonName):
                    matchingPerson = allPerson.Where(
                        (person) => {
                            if(string.IsNullOrEmpty(person.PersonName)) {
                                return true;
                            } else {
                                return person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase);
                            }
                        }).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPerson = allPerson.Where(
                        (person) => {
                            if(string.IsNullOrEmpty(person.Email)) {
                                return true;
                            } else {
                                return person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase);
                            }
                        }).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
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

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? guid) {
            if(guid == null) return null;

            Person? p = await _db.Persons.Include(nameof(Person.Country)).FirstOrDefaultAsync(person => person.PersonID == guid);
            return p?.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption) {
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest) {
            if(personUpdateRequest == null) { throw new ArgumentNullException(nameof(personUpdateRequest)); }
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = await _db.Persons.Include(nameof(Person.Country)).FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);
            if(matchingPerson == null) { throw new ArgumentException("Given id doesn't exist"); }
            //Update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            await _db.SaveChangesAsync();
            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePersonByID(Guid? guid) {
            if(guid == null) { throw new ArgumentNullException(nameof(guid)); }
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == guid);
            if(matchingPerson == null) { return false; }
            _db.Persons.Remove(matchingPerson);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
