using Entities;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using RepositoryContracts;

namespace Services {
    public class CountryService : ICountryService {

        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository) {
            _countryRepository = countryRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest) {
            if(countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            if(countryAddRequest.CountryName == null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }
            if(await _countryRepository.GetCountryByName(countryAddRequest.CountryName) != null) {
                throw new ArgumentException("Given CountryName already exists");
            }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            await _countryRepository.AddCountry(country);
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries() {
            //这里需要注意，必须把数据从数据库转换为内存中的对象才能在Linq表达式中使用自定义的方法，否则会报InvalidOperationException
            List<Country> countries = await _countryRepository.GetAllCountries();
            return countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? guid) {
            if(guid == null) return null;
            Country? result = await _countryRepository.GetCountryByID(guid.Value);
            return result?.ToCountryResponse();
        }
    }
}
