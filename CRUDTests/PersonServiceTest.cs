using ServciceContracts;
using ServciceContracts.Enums;
using Services;
using ServciceContracts.DataTransferObject;
using Entities;
using System.Net;
using Xunit.Abstractions;
using ServiceContracts.Enums;
using ServiceContracts.DataTransferObject;

namespace CRUDTests {
    public class PersonServiceTest {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper) {
            _personService = new PersonService();
            _countryService = new CountryService();
            _testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullPersonAddRequest() {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(
                () => {
                    //Act
                    _personService.AddPerson(personAddRequest);
                });
        }
        [Fact]
        public void AddPerson_NullPersonName() {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            //Assert
            Assert.Throws<ArgumentException>(
                () => {
                    //Act
                    _personService.AddPerson(personAddRequest);
                });
        }
        [Fact]
        public void AddPerson_ProperPersonDetail() {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "John", Email = "123456@123.com", DateOfBirth = DateTime.Parse("1995-01-01"), Gender = GenderOptions.Male, CountryID = Guid.NewGuid(), Address = "China", ReceiveNewsLetters = true };
            //Act
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            //Assert
            Assert.True(personResponse.PersonID != Guid.Empty);
        }
        #endregion

        #region GetAllPerson
        [Fact]
        public void GetAllPerson_Empty() {
            Assert.Empty(_personService.GetAllPerson());
        }
        [Fact]
        public void GetAllPerson_AFewPerson() {
            //Arrange
            List<PersonResponse> personResponses_from_add = addFewPerson();
            //Act
            List<PersonResponse> personResponses_from_get = _personService.GetAllPerson();
            //Assert
            foreach(PersonResponse response in personResponses_from_add) {
                Assert.Contains(response, personResponses_from_get);
            }
        }
        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_NullPersonID() {
            //Arrange
            Guid? guid = null;
            //Act
            PersonResponse? personResponse = _personService.GetPersonByPersonID(guid);
            //Assert
            Assert.Null(personResponse);
        }
        [Fact]
        public void GetPersonByPersonID_WithPersonID() {
            //Arrange
            List<PersonResponse> personResponse_from_add = addFewPerson();
            //Act
            PersonResponse? personResponse_from_get = _personService.GetPersonByPersonID(personResponse_from_add[0].PersonID);
            //Assert
            Assert.Equal(personResponse_from_add[0].PersonID, personResponse_from_get?.PersonID);
        }
        #endregion

        #region GetFilterPerson
        [Fact]
        public void GetFilterPerson_EmptySearchText() {
            //Arrange
            List<PersonResponse> personResponses_from_add = addFewPerson();
            //Act
            List<PersonResponse> personResponses_from_get = _personService.GetFilterPerson(nameof(Person.PersonName), "");
            //Assert
            foreach(PersonResponse response in personResponses_from_add) {
                Assert.Contains(response, personResponses_from_get);
            }
        }
        [Fact]
        public void GetFilterPerson_SearchByPersonName() {
            //Arrange
            List<PersonResponse> personResponses_from_add = addFewPerson();
            //Act
            List<PersonResponse> personResponses_from_get = _personService.GetFilterPerson(nameof(Person.PersonName), "a");
            foreach(PersonResponse personResponse in personResponses_from_get) {
                _testOutputHelper.WriteLine($"{personResponse.PersonName}");
            }
            //Assert
            foreach(PersonResponse response in personResponses_from_get) {
                Assert.Contains(response, personResponses_from_get);
            }
        }
        #endregion

        #region GetSortedPerson
        [Fact]
        public void GetSortedPerson_() {
            //Arrange
            List<PersonResponse> person_from_add = addFewPerson();
            //Act
            List<PersonResponse> person_from_sort = _personService.GetSortedPerson(_personService.GetAllPerson(), nameof(PersonResponse.DateOfBirth), SortOrderOption.DESC);
            List<PersonResponse> person_from_test_sort = person_from_add.OrderByDescending(temp => temp.DateOfBirth).ToList();
            //Assert
            for(int i = 0; i < person_from_test_sort.Count; i++) {
                _testOutputHelper.WriteLine($"{person_from_sort[i].PersonName}  {person_from_sort[i].DateOfBirth.Value.ToString("dd MMMM yyyy")}");
                Assert.Equal(person_from_test_sort[i], person_from_sort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPersonUpdateRequest() {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }
        [Fact]
        public void UpdatePerson_InvalidPersonID() {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };
            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }
        [Fact]
        public void UpdatePerson_EmptyPersonName() {
            //Arrange
            List<PersonResponse> personResponses = addFewPerson();
            PersonUpdateRequest personUpdateRequest = personResponses[0].ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "";

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }
        [Fact]
        public void UpdatePerson_PersonFullDetailUpdate() {
            //Arrange
            List<PersonResponse> personResponses = addFewPerson();
            PersonUpdateRequest personUpdateRequest = personResponses[0].ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "asdqwezxc";
            //Act
            PersonResponse person_from_update = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse person_from_get = _personService.GetPersonByPersonID(person_from_update.PersonID);
            //Assert
            Assert.Equal(person_from_update, person_from_get);
        }
        #endregion

        private List<PersonResponse> addFewPerson() {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "China" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Smith", Email = "123456@123.com", DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, CountryID = countryResponse.CountryID, Address = "qwe", ReceiveNewsLetters = true };
            PersonAddRequest personAddRequest2 = new PersonAddRequest() { PersonName = "Jack", Email = "123456@123.com", DateOfBirth = DateTime.Parse("1994-01-01"), Gender = GenderOptions.Male, CountryID = countryResponse.CountryID, Address = "qwe", ReceiveNewsLetters = false };
            PersonAddRequest personAddRequest3 = new PersonAddRequest() { PersonName = "Alice", Email = "123456@123.com", DateOfBirth = DateTime.Parse("1995-01-01"), Gender = GenderOptions.Female, CountryID = countryResponse.CountryID, Address = "qwe", ReceiveNewsLetters = true };
            List<PersonResponse> personResponses =
            [
                _personService.AddPerson(personAddRequest),
                _personService.AddPerson(personAddRequest2),
                _personService.AddPerson(personAddRequest3),
            ];
            return personResponses;
        }
    }
}
