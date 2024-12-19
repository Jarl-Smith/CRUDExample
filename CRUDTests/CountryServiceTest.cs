using ServiceContracts;
using Entities;
using Services;
using ServiceContracts.DataTransferObject;
using Microsoft.EntityFrameworkCore;


namespace CRUDTests {
    public class CountryServiceTest {
        private readonly ICountryService _countriesService;

        public CountryServiceTest() {
            _countriesService = new CountryService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountryRequest() {
            //Arrange
            CountryAddRequest? countryAddRequest = null;
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                //Act
                await _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public async Task AddCountry_NullCountryName() {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public async Task AddCountry_DuplicateCountryName() {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "123" };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest() { CountryName = "123" };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _countriesService.AddCountry(countryAddRequest1);
                await _countriesService.AddCountry(countryAddRequest2);
            });
        }

        [Fact]
        public async Task AddCountry_ProperCountryDetail() {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest() { CountryName = "123" };
            //Act
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest1);
            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmptyList() {
            //Act
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            //Assert
            Assert.Empty(countries);
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID() {
            //Arrange
            Guid? countryID = null;
            //Act
            CountryResponse? response = await _countriesService.GetCountryByCountryID(countryID);
            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID() {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse response1 = await _countriesService.AddCountry(countryAddRequest);
            //Act
            CountryResponse? response2 = await _countriesService.GetCountryByCountryID(response1.CountryID);
            //Assert
            Assert.Equal(response1.CountryID, response2.CountryID);
        }
        #endregion
    }
}