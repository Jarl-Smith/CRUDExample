using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DataTransferObject;

namespace CRUDExample.Filters.ActionFilters {
    public class PersonListActionFilter : IActionFilter {

        private readonly ILogger<PersonListActionFilter> _logger;
        private static readonly List<string> searchByOptions = new List<string>() { nameof(PersonResponse.PersonName), nameof(PersonResponse.Email), nameof(PersonResponse.Address), nameof(PersonResponse.Gender), nameof(PersonResponse.DateOfBirth), nameof(PersonResponse.Country), nameof(PersonResponse.Age), nameof(PersonResponse.ReceiveNewsLetters) };
        private static readonly IDictionary<string, string> searchField = new Dictionary<string, string>() {
                {"Person Name",nameof(PersonResponse.PersonName) },
                {"Email", nameof(PersonResponse.Email) },
                {"Date Of Birth", nameof(PersonResponse.DateOfBirth) },
                {"Gender", nameof(PersonResponse.Gender) },
                {"Country", nameof(PersonResponse.Country) },
                {"Address", nameof(PersonResponse.Address) },
                {"Receive News Letters", nameof(PersonResponse.ReceiveNewsLetters) },
                {"Age", nameof(PersonResponse.Age) }
            };
        public PersonListActionFilter(ILogger<PersonListActionFilter> logger) {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context) {
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(PersonListActionFilter), nameof(OnActionExecuted));//Structured Logging
            PersonController controller = (PersonController)context.Controller;
            IDictionary<string, object?>? parameters = context.HttpContext.Items["queryString"] as IDictionary<string, object?>;//取出queryString
            if(parameters != null) {
                //将特定queryString存放到ViewData中，方便View访问
                if(parameters.ContainsKey("searchBy")) {
                    controller.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
                }
                if(parameters.ContainsKey("searchString")) {
                    controller.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }
                if(parameters.ContainsKey("sortBy")) {
                    controller.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }
                if(parameters.ContainsKey("sortOrderOption")) {
                    controller.ViewData["CurrentSortOrderOption"] = Convert.ToString(parameters["sortOrderOption"]);
                }
            }
            controller.ViewData["ColumnNameAndProperty"] = searchField;
        }

        //在进入Controller的Action方法前，此处验证QueryString参数的合法性，并进行修正
        public void OnActionExecuting(ActionExecutingContext context) {
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(PersonListActionFilter), nameof(OnActionExecuting));//Structured Logging
            context.HttpContext.Items["queryString"] = context.ActionArguments;//因为OnActionExecuted无法访问ActionArguments，所以此处将ActionArguments存储到HttpContext.Items属性中，方便OnActionExecuted访问
            if(context.ActionArguments.ContainsKey("searchBy")) {//如果QueryString参数有searchBy
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);//取出searchBy的值
                if(!string.IsNullOrEmpty(searchBy)) {//如果searchBy的值非空
                    if(!searchByOptions.Any(temp => temp == searchBy)) {//且searchBy的值不匹配任意PersonResponse的列名
                        context.ActionArguments["searchBy"] = searchByOptions[0];//searchBy修正为PersonName
                    }
                }
            }
        }
    }
}
