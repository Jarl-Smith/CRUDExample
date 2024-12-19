using Entities;
using System.Linq.Expressions;
namespace RepositoryContracts {
    public interface IPersonRepository {
        Task<Person> AddPerson(Person person);
        Task<List<Person>> GetAllPerson();
        Task<Person?> GetPersonByID(Guid personID);
        Task<List<Person>> GetFilterPerson(Expression<Func<Person, bool>> predicate);
        Task<Person> UpdatePerson(Person person);
        Task<bool> DeletePersonByID(Guid personID);
    }
}
