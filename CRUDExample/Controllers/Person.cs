using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers {
    [Controller]
    public class Person : Controller {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public Person(IPersonService personService, ICountryService countryService) {
            _personService = personService;
            _countryService = countryService;
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

        [Route("/person/create")]
        [HttpGet]
        public IActionResult Create() {
            ViewBag.Countries = _countryService.GetAllCountries();
            return View();
        }

        [Route("/person/create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest? personAddRequest) {
            if(!ModelState.IsValid) {
                ViewBag.Countries = _countryService.GetAllCountries();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index","Person");
        }
    }
}
