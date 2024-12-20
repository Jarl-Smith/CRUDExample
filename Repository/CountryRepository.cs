using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository {
    public class CountryRepository : ICountryRepository {

        private readonly ApplicationDbContext _db;

        public CountryRepository(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }

        public async Task<Country> AddCountry(Country country) {
            await _db.Countries.AddAsync(country);
            return country;
        }

        public async Task<bool> DeleteCountryByID(Guid countryID) {
            Country? matchCountry = await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);
            if(matchCountry == null) {
                return false;
            } else {
                _db.Countries.Remove(matchCountry);
                return true;
            }
        }

        public async Task<Country?> GetCountryByID(Guid countryID) {
            return await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);
        }

        public async Task<List<Country>> GetAllCountries() {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByName(string countryName) {
            return await _db.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
        }
    }
}
