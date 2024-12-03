using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DataTransferObject;

namespace CRUDExample.Controllers {
    [Controller]
    public class Person : Controller {

        private readonly IPersonService _personService;

        public Person(IPersonService personService) {
            _personService = personService;
        }

        [Route("/")]
        [Route("/person/index")]
        public IActionResult Index(string searchBy, string? searchString) {
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.PersonProperty = new Dictionary<string, string>() {
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
            return View(filterPerson);
        }
    }
}
