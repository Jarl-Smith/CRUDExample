using Entities;
using SerilogTimings;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;
using Services.Helpers;
using RepositoryContracts;

namespace Services {
    public class PersonService : IPersonService {

        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository) {
            _personRepository = personRepository;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest) {
            if(personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            Person person;
            using(Operation.Time("Time for AddPerson from database")) {
                person = personAddRequest.ToPerson();
                person.PersonID = Guid.NewGuid();
                await _personRepository.AddPerson(person);
            }
            return person.ToPersonResponse();
        }

        public async Task<bool> DeletePersonByID(Guid? guid) {
            if(guid == null) { throw new ArgumentNullException(nameof(guid)); }
            if(await _personRepository.GetPersonByID(guid.Value) == null) { return false; }
            await _personRepository.DeletePersonByID(guid.Value);
            return true;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest) {
            if(personUpdateRequest == null) { throw new ArgumentNullException(nameof(personUpdateRequest)); }
            ValidationHelper.ModelValidation(personUpdateRequest);

            Person updatedPerson;
            using(Operation.Time("Time for UpdatePerson from database")) {
                if(await _personRepository.GetPersonByID(personUpdateRequest.PersonID) == null) { throw new ArgumentException("Given id doesn't exist"); }
                updatedPerson = await _personRepository.UpdatePerson(personUpdateRequest.ToPerson());
            }
            return updatedPerson.ToPersonResponse();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? guid) {
            if(guid == null) return null;

            Person? p;
            using(Operation.Time("Time for GetPersonByPersonID from database")) {
                p = await _personRepository.GetPersonByID(guid.Value);
            }
            return p?.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPerson() {
            List<Person> person;
            using(Operation.Time("Time form GetAllPerson from database")) {
                person = await _personRepository.GetAllPerson();
            }
            return person.Select(p => p.ToPersonResponse()).ToList();
        }

        public async Task<List<PersonResponse>> GetFilterPerson(string searchBy, string? searchString) {
            List<Person> tempList;
            using(Operation.Time("Time for GetFilterPerson from database")) {
                if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) {//搜索的字段或者搜索关键字为空，则直接返回所有
                    return (await _personRepository.GetAllPerson()).Select(temp => temp.ToPersonResponse()).ToList();
                } else {
                    switch(searchBy) {
                        case nameof(PersonResponse.PersonName)://根据PersonName进行关键字搜索，返回匹配成功的结果
                            tempList = await _personRepository.GetFilterPerson(person =>
                            string.IsNullOrEmpty(person.PersonName) ? true : person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                            break;
                        case nameof(PersonResponse.Email)://根据Email进行关键字搜索，返回匹配成功的结果
                            tempList = await _personRepository.GetFilterPerson(person =>
                            string.IsNullOrEmpty(person.Email) ? true : person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                            break;
                        case nameof(PersonResponse.DateOfBirth)://同理
                            tempList = await _personRepository.GetFilterPerson(person =>
                            person.DateOfBirth == null ? false : person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase));
                            break;
                        default:
                            tempList = await _personRepository.GetAllPerson();
                            break;
                    }
                }
            }
            return tempList.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPerson, string sortBy, SortOrderOption sortOrderOption) {
            List<PersonResponse> sortPerson;
            using(Operation.Time("Time for GetSortedPerson from database")) {
                sortPerson = (sortBy, sortOrderOption)
                switch {
                    (nameof(PersonResponse.PersonName), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.PersonName), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Email), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Email), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.DateOfBirth), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.DateOfBirth).ToList(),
                    (nameof(PersonResponse.DateOfBirth), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                    (nameof(PersonResponse.Gender), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Gender), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Country), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Country), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Address), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Address), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                    (nameof(PersonResponse.Age), SortOrderOption.ASC) => allPerson.OrderBy(temp => temp.Age).ToList(),
                    (nameof(PersonResponse.Age), SortOrderOption.DESC) => allPerson.OrderByDescending(temp => temp.Age).ToList(),
                    _ => allPerson
                };
            }
            return sortPerson;
        }

        public async Task<MemoryStream> GetAllPersonAsExcel() {
            MemoryStream memoryStream = new MemoryStream();
            List<PersonResponse> allPerson = await GetAllPerson();
            using(ExcelPackage package = new ExcelPackage(memoryStream)) {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("AllPerson");
                workSheet.Cells[1, 1].Value = nameof(PersonResponse.PersonID);
                workSheet.Cells[1, 2].Value = nameof(PersonResponse.PersonName);
                workSheet.Cells[1, 3].Value = nameof(PersonResponse.Email);
                workSheet.Cells[1, 4].Value = nameof(PersonResponse.DateOfBirth);
                workSheet.Cells[1, 5].Value = nameof(PersonResponse.Gender);
                workSheet.Cells[1, 6].Value = nameof(PersonResponse.Country);
                workSheet.Cells[1, 7].Value = nameof(PersonResponse.Address);
                workSheet.Cells[1, 8].Value = nameof(PersonResponse.ReceiveNewsLetters);
                workSheet.Cells[1, 9].Value = nameof(PersonResponse.Age);
                int recordStartRow = 2;
                foreach(PersonResponse personResponse in allPerson) {
                    workSheet.Cells[recordStartRow, 1].Value = personResponse.PersonID;
                    workSheet.Cells[recordStartRow, 2].Value = personResponse.PersonName;
                    workSheet.Cells[recordStartRow, 3].Value = personResponse.Email;
                    workSheet.Cells[recordStartRow, 4].Value = personResponse.DateOfBirth?.ToString("yyyy-MM-dd");
                    workSheet.Cells[recordStartRow, 5].Value = personResponse.Gender;
                    workSheet.Cells[recordStartRow, 6].Value = personResponse.Country;
                    workSheet.Cells[recordStartRow, 7].Value = personResponse.Address;
                    workSheet.Cells[recordStartRow, 8].Value = personResponse.ReceiveNewsLetters;
                    workSheet.Cells[recordStartRow, 9].Value = personResponse.Age;
                    recordStartRow++;
                }
                workSheet.Cells[1, 1, recordStartRow, 9].AutoFitColumns();
                await package.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
