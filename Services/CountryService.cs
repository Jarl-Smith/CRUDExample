﻿using Entities;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using System.Text.Json;

namespace Services {
    public class CountryService : ICountryService {
        private List<Country> _countries;

        public CountryService() {
            string fileName = "countrydata.json";
            string jsonString = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName));
            _countries = JsonSerializer.Deserialize<List<Country>>(jsonString);
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest) {
            if(countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            if(countryAddRequest.CountryName == null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }
            if(_countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0) { throw new ArgumentException("Given CountryName already exists"); }
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries() {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? guid) {
            if(guid == null) return null;
            Country? result = _countries.FirstOrDefault(country => country.CountryID == guid);
            if(result == null) return null;
            return result.ToCountryResponse();
        }
    }
}
