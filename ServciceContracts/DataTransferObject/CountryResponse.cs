using Entities;
using System.Runtime.CompilerServices;
namespace ServciceContracts.DataTransferObject {
    public class CountryResponse {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }

    public static class CountryExtensions {
        public static CountryResponse ToCountryResponse( this Country country ) {
            return new CountryResponse() { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
