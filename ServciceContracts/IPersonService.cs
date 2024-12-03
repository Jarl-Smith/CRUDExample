using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;

namespace ServiceContracts {
    public interface IPersonService {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPerson();
        PersonResponse? GetPersonByPersonID(Guid? guid);
        List<PersonResponse> GetFilterPerson(string searchBy, string? searchString);
        List<PersonResponse> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption);
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        bool DeletePersonByID(Guid? guid);
    }
}
