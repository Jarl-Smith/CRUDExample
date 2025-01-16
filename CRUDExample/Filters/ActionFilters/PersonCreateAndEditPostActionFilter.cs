using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DataTransferObject;

namespace CRUDExample.Filters.ActionFilters {
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter {

        private readonly ICountryService _countryService;

        public PersonCreateAndEditPostActionFilter(ICountryService countryService) {
            _countryService = countryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            if(context.Controller is PersonController personController) {
                if(!personController.ModelState.IsValid) {//如果客户端发来的数据没能通过验证，则进行短路
                    List<CountryResponse> countryResponses = await _countryService.GetAllCountries();
                    personController.ViewBag.Countries = countryResponses.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });
                    personController.ViewBag.Errors = personController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    context.Result = personController.View(context.ActionArguments["personRequest"]);
                    return;
                }
            }
            await next();
        }
    }
}
