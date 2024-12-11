using Microsoft.EntityFrameworkCore;

namespace Entities {
    public class PersonsDbContext : DbContext {

        public PersonsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //seed data for person
            string personJson = System.IO.File.ReadAllText("persondata.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personJson);
            foreach(Person person in persons) {
                modelBuilder.Entity<Person>().HasData(person);
            }
            //seed data for country
            string countryJson = System.IO.File.ReadAllText("countrydata.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countryJson);
            foreach(Country c in countries) {
                modelBuilder.Entity<Country>().HasData(c);
            }
        }
    }
}
