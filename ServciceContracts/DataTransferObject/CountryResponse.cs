using Entities;
namespace ServiceContracts.DataTransferObject {
    public class CountryResponse {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj) {
            if(obj.GetType() != typeof(CountryResponse)) {
                return false;
            }
            CountryResponse response = (CountryResponse)obj;
            return response.CountryID == this.CountryID && response.CountryName == this.CountryName;
        }
    }

    public static class CountryExtensions {
        public static CountryResponse ToCountryResponse( this Country country ) {
            return new CountryResponse() { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
