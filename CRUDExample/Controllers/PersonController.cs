﻿using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DataTransferObject;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers {

    [Route("[controller]")]
    public class PersonController : Controller {

        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService personService, ICountryService countryService, ILogger<PersonController> logger) {
            _personService = personService;
            _countryService = countryService;
            _logger = logger;
        }

        [Route("/")]
        [Route("[action]")]
        [TypeFilter(typeof(PersonListActionFilter))]
        [TypeFilter(typeof(PersonListResultFilter))]
        //通过Asp.Net Core的QueryString Binding，接收客户发送的请求，按照请求进行筛选+排序，并返回结果
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOption sortOrderOption = SortOrderOption.ASC) {
            List<PersonResponse> filterPerson = await _personService.GetFilterPerson(searchBy, searchString);
            List<PersonResponse> sortedPerson = await _personService.GetSortedPerson(filterPerson, sortBy, sortOrderOption);
            return View(sortedPerson);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create() {
            List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
            ViewBag.Countries = countryResponses.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //通过Asp.Net Core的model binding，接收用户的数据，对model进行验证，验证不通过将验证结果返回，验证通过执行添加操作并返回主界面
        public async Task<IActionResult> Create(PersonAddRequest? personRequest) {

            PersonResponse personResponse = await _personService.AddPerson(personRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personID}")]//Eg:person/edit/afea1654
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personID) {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
            if(personResponse == null) {
                return RedirectToAction("Index");
            } else {
                List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
                ViewBag.Countries = countryResponses.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() }).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest) {
            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personRequest.PersonID);
            if(personResponse == null) { return RedirectToAction("Index"); }

            await _personService.UpdatePerson(personRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID) {
            if(personID == null) {
                return RedirectToAction("Index");
            } else {
                PersonResponse? personResponse = await _personService.GetPersonByPersonID(personID);
                if(personResponse == null) {
                    return RedirectToAction("Index");
                } else {
                    return View(personResponse);
                }
            }
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonResponse personResponse) {
            PersonResponse? personResponse1 = await _personService.GetPersonByPersonID(personResponse.PersonID);
            if(personResponse1 == null) {
                return RedirectToAction("Index");
            }
            await _personService.DeletePersonByID(personResponse1.PersonID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllPersonAsExcel() {
            MemoryStream memoryStream = await _personService.GetAllPersonAsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllPerson.xlsx");
        }
    }
}
