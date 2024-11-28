using ServciceContracts.DataTransferObject;

namespace ServciceContracts {
    public interface IPersonService {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPerson();

        PersonResponse? GetPersonByPersonID(Guid? guid);

    }
}
