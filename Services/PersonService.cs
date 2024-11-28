using Entities;
using ServciceContracts;
using ServciceContracts.DataTransferObject;
using Services.Helpers;

namespace Services {
    public class PersonService : IPersonService {
        private readonly List<Person> _person;
        private readonly ICountryService _countryService;
        public PersonService() {
            _person = new List<Person>();
            _countryService = new CountryService();
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

        public PersonResponse? GetPersonByPersonID(Guid? guid) {
            return guid != null ? _person.Where(person => person.PersonID == guid).FirstOrDefault()?.ToPersonResponse() : null;
        }

        private PersonResponse convertPersonToPersonResponse(Person person) {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryByCountryID(personResponse.CountryID)?.CountryName;
            return personResponse;
        }
    }
}
