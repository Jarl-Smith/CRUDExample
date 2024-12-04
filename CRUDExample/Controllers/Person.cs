using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers {
    [Controller]
    public class Person : Controller {

        private readonly IPersonService _personService;

        public Person(IPersonService personService) {
            _personService = personService;
        }

        [Route("/")]
        [Route("/person/index")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOption sortOrderOption = SortOrderOption.ASC) {
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrderOption = sortOrderOption.ToString();
            ViewBag.ColumnNameAndProperty = new Dictionary<string, string>() {
                {"Person Name",nameof(PersonResponse.PersonName) },
                {"Email", nameof(PersonResponse.Email) },
                {"Date Of Birth", nameof(PersonResponse.DateOfBirth) },
                {"Gender", nameof(PersonResponse.Gender) },
                {"Country", nameof(PersonResponse.Country) },
                {"Address", nameof(PersonResponse.Address) },
                {"Receive News Letters", nameof(PersonResponse.ReceiveNewsLetters) },
                {"Age", nameof(PersonResponse.Age) }
            };
            List<PersonResponse> filterPerson = _personService.GetFilterPerson(searchBy, searchString);
            List<PersonResponse> sortedPerson = _personService.GetSortedPerson(filterPerson, sortBy, sortOrderOption);
            return View(sortedPerson);
        }
    }
}
