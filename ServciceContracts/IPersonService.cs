using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;

namespace ServiceContracts {
    public interface IPersonService {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        Task<List<PersonResponse>> GetAllPerson();
        Task<PersonResponse?> GetPersonByPersonID(Guid? guid);
        Task<List<PersonResponse>> GetFilterPerson(string searchBy, string? searchString);
        Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption);
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        Task<bool> DeletePersonByID(Guid? guid);
    }
}
