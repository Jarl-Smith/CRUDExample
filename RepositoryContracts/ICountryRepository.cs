using Entities;
namespace RepositoryContracts {
    public interface ICountryRepository {
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryByID(Guid countryID);
        Task<Country?> GetCountryByName(string countryName);
        Task<bool> DeleteCountryByID(Guid countryID);
    }
}
