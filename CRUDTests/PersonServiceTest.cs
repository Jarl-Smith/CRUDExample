using ServiceContracts;
using ServiceContracts.Enums;
using Services;
using ServiceContracts.DataTransferObject;
using Xunit.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests {
    public class PersonServiceTest {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper) {
            ApplicationDbContext personsDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            _countryService = new CountryService(personsDbContext);
            _personService = new PersonService(personsDbContext, _countryService);
            _testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPersonAddRequest() {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
               async () => {
                   //Act
                   await _personService.AddPerson(personAddRequest);
               });
        }
        [Fact]
        public async Task AddPerson_NullPersonName() {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => {
                    //Act
                    await _personService.AddPerson(personAddRequest);
                });
        }
        [Fact]
        public async Task AddPerson_ProperPersonDetail() {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "John", Email = "123456@123.com", DateOfBirth = DateTime.Parse("1995-01-01"), Gender = GenderOptions.Male, CountryID = Guid.NewGuid(), Address = "China", ReceiveNewsLetters = true };
            //Act
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            //Assert
            Assert.True(personResponse.PersonID != Guid.Empty);
        }
        #endregion

        #region GetAllPerson
        [Fact]
        public async Task GetAllPerson_Empty() {
            List<PersonResponse> personResponses = await _personService.GetAllPerson();
            Assert.Empty(personResponses);
        }
        #endregion

        #region GetPersonByPersonID
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID() {
            //Arrange
            Guid? guid = null;
            //Act
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(guid);
            //Assert
            Assert.Null(personResponse);
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullPersonUpdateRequest() {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => {
                    //Act
                    await _personService.UpdatePerson(personUpdateRequest);
                });
        }
        [Fact]
        public async Task UpdatePerson_InvalidPersonID() {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(
                 async () => {
                     //Act
                     await _personService.UpdatePerson(personUpdateRequest);
                 });
        }
        #endregion
    }
}
