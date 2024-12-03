using Entities;

namespace ServiceContracts.DataTransferObject {
    public class CountryAddRequest {
        public string? CountryName { get; set; }

        public Country ToCountry() {
            return new Country() { CountryName = this.CountryName };
        }
    }
}
