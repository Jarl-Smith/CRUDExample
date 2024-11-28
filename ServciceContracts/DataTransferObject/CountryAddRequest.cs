using Entities;

namespace ServciceContracts.DataTransferObject {
    public class CountryAddRequest {
        public string? CountryName { get; set; }

        public Country ToCountry() {
            return new Country() { CountryName = this.CountryName };
        }
    }
}
