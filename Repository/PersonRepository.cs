using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repository {
    public class PersonRepository : IPersonRepository {
        private readonly ApplicationDbContext _db;
        public PersonRepository(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }
        public async Task<Person> AddPerson(Person person) {
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByID(Guid personID) {
            Person? matchPerson = await _db.Persons.FirstOrDefaultAsync(p => p.PersonID == personID);
            if(matchPerson == null) {
                return false;
            } else {
                _db.Persons.Remove(matchPerson);
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<Person>> GetAllPerson() {
            return await _db.Persons.Include(nameof(Person.Country)).ToListAsync();
        }

        public async Task<List<Person>> GetFilterPerson(Expression<Func<Person, bool>> predicate) {
            return await _db.Persons.Include(nameof(Person.Country))
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonByID(Guid personID) {
            return await _db.Persons.Include(nameof(Person.Country))
                .FirstOrDefaultAsync(p => p.PersonID == personID);
        }

        public async Task<Person> UpdatePerson(Person person) {
            Person? matchPerson = await _db.Persons.FirstAsync(p => p.PersonID == person.PersonID);
            if(matchPerson == null) {
                return person;
            } else {
                matchPerson.PersonName = person.PersonName;
                matchPerson.Email = person.Email;
                matchPerson.DateOfBirth = person.DateOfBirth;
                matchPerson.Gender = person.Gender;
                matchPerson.CountryID = person.CountryID;
                matchPerson.Address = person.Address;
                matchPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
                await _db.SaveChangesAsync();
                return matchPerson;
            }
        }
    }
}
