using ServiceContracts.DataTransferObject;

namespace ServiceContracts {
    public interface ICountryService {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
        CountryResponse? GetCountryByCountryID(Guid? guid);
    }
}
