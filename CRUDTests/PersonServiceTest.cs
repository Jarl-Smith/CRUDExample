using ServciceContracts;
using ServciceContracts.Enums;
using Services;
using ServciceContracts.DataTransferObject;


namespace CRUDTests {
    public class PersonServiceTest {

        private readonly IPersonService _personService;

        public PersonServiceTest() {
            _personService = new PersonService();
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


    }
}
