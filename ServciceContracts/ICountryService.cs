using ServciceContracts.DataTransferObject;

namespace ServciceContracts {
    public interface ICountryService {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
        CountryResponse? GetCountryByCountryID(Guid? guid);
    }
}
