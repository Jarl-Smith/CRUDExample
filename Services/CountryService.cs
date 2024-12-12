using Entities;
using ServiceContracts;
using ServiceContracts.DataTransferObject;

namespace Services {
    public class CountryService : ICountryService {
        private readonly PersonsDbContext _db;

        public CountryService(PersonsDbContext personsDbContext) {
            _db = personsDbContext;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest) {
            if(countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            if(countryAddRequest.CountryName == null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }
            if(_db.Countries.Count(c => c.CountryName == countryAddRequest.CountryName) > 0) {
                throw new ArgumentException("Given CountryName already exists");
            }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            _db.SaveChanges();
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries() {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? guid) {
            if(guid == null) return null;
            Country? result = _db.Countries.FirstOrDefault(country => country.CountryID == guid);
            if(result == null) return null;
            return result.ToCountryResponse();
        }
    }
}
