using ServiceContracts.DataTransferObject;

namespace ServiceContracts {
    public interface ICountryService {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        Task<List<CountryResponse>> GetAllCountries();
        Task<CountryResponse?> GetCountryByCountryID(Guid? guid);
    }
}
