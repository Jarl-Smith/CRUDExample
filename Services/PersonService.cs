using Entities;
using ServciceContracts;
using ServciceContracts.DataTransferObject;

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
            if(string.IsNullOrEmpty(personAddRequest.PersonName)) throw new ArgumentException("PersonName should not be blank");
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _person.Add(person);
            return convertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPerson() {
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonID(Guid? guid) {
            throw new NotImplementedException();
        }

        private PersonResponse convertPersonToPersonResponse(Person person) {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryByCountryID(personResponse.CountryID)?.CountryName;
            return personResponse;
        }
    }
}
