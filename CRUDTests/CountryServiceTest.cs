using Xunit;
using ServiceContracts;
using Entities;
using Services;
using ServiceContracts.DataTransferObject;
using Microsoft.EntityFrameworkCore;


namespace CRUDTests {
    public class CountryServiceTest {
        private readonly ICountryService _countriesService;

        public CountryServiceTest() {
            _countriesService = new CountryService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountryRequest() {
            //Arrange
            CountryAddRequest? countryAddRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public void AddCountry_NullCountryName() {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { };
            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public void AddCountry_DuplicateCountryName() {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "123" };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest() { CountryName = "123" };
            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesService.AddCountry(countryAddRequest1);
                _countriesService.AddCountry(countryAddRequest2);
            });
        }

        [Fact]
        public void AddCountry_ProperCountryDetail() {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "123" };
            //Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest1);
            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList() {
            //Act
            Assert.Empty(_countriesService.GetAllCountries());
        }

        [Fact]
        public void GetAllCountries_AddFewCountries() {
            //Arrange
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>() {
                new CountryAddRequest(){CountryName="China"},
                new CountryAddRequest(){CountryName="America"}
            };
            //Act
            List<CountryResponse> countryResponses = new List<CountryResponse>();
            foreach(CountryAddRequest countryAddRequest in countryAddRequests) {
                countryResponses.Add(_countriesService.AddCountry(countryAddRequest));
            }
            List<CountryResponse> countryResponses1 = _countriesService.GetAllCountries();
            //Assert
            foreach(CountryResponse countryResponse in countryResponses) {
                Assert.Contains(countryResponse, countryResponses1);
            }
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public void GetCountryByCountryID_NullCountryID() {
            //Arrange
            Guid? countryID = null;
            //Act
            CountryResponse? response = _countriesService.GetCountryByCountryID(countryID);
            //Assert
            Assert.Null(response);
        }

        [Fact]
        public void GetCountryByCountryID_ValidCountryID() {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse response1 = _countriesService.AddCountry(countryAddRequest);
            //Act
            CountryResponse? response2 = _countriesService.GetCountryByCountryID(response1.CountryID);
            //Assert
            Assert.Equal(response1.CountryID, response2.CountryID);
        }
        #endregion
    }
}