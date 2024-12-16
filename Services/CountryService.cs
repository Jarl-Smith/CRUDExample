using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DataTransferObject;

namespace Services {
    public class CountryService : ICountryService {
        private readonly PersonsDbContext _db;

        public CountryService(PersonsDbContext personsDbContext) {
            _db = personsDbContext;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest) {
            if(countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            if(countryAddRequest.CountryName == null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }
            if(await _db.Countries.CountAsync(c => c.CountryName == countryAddRequest.CountryName) > 0) {
                throw new ArgumentException("Given CountryName already exists");
            }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries() {
            //这里需要注意，必须把数据从数据库转换为内存中的对象才能在Linq表达式中使用自定义的方法，否则会报InvalidOperationException
            List<Country> countries = await _db.Countries.ToListAsync();
            return countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? guid) {
            if(guid == null) return null;
            Country? result = await _db.Countries.FirstOrDefaultAsync(country => country.CountryID == guid);
            if(result == null) return null;
            return result.ToCountryResponse();
        }
    }
}
