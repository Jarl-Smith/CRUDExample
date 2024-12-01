using ServciceContracts.DataTransferObject;
using ServciceContracts.Enums;
using ServiceContracts.Enums;

namespace ServciceContracts {
    public interface IPersonService {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPerson();
        PersonResponse? GetPersonByPersonID(Guid? guid);
        List<PersonResponse> GetFilterPerson(string searchBy, string? searchString);
        List<PersonResponse> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption);

    }
}
