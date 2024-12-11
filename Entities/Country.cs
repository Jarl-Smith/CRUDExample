using System.ComponentModel.DataAnnotations;

namespace Entities {
    /// <summary>
    /// Domain Model
    /// </summary>
    public class Country {
        [Key]
        public Guid CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
