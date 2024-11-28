using ServciceContracts;
using ServciceContracts.Enums;
using Services;
using ServciceContracts.DataTransferObject;
using Entities;
using System.Net;

namespace CRUDTests {
    public class PersonServiceTest {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public PersonServiceTest() {
            _personService = new PersonService();
            _countryService = new CountryService();
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
            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = "Smith", Email = "123456@123.com", DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, Address = "qwe", ReceiveNewsLetters = true };
            //Act
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);
            PersonResponse? personResponse_from_get = _personService.GetPersonByPersonID(personResponse_from_add.PersonID);
            //Assert
            Assert.Equal(personResponse_from_add.PersonID, personResponse_from_get?.PersonID);
        }
        #endregion
    }
}
