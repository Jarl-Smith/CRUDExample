using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DataTransferObject;

namespace CRUDExample.Filters.ActionFilters {
    public class PersonListActionFilter : IActionFilter {

        private readonly ILogger<PersonListActionFilter> _logger;
        private static readonly List<string> searchByOptions = new List<string>() { nameof(PersonResponse.PersonName), nameof(PersonResponse.Email), nameof(PersonResponse.Address), nameof(PersonResponse.Gender), nameof(PersonResponse.DateOfBirth), nameof(PersonResponse.Country), nameof(PersonResponse.Age), nameof(PersonResponse.ReceiveNewsLetters) };

        public PersonListActionFilter(ILogger<PersonListActionFilter> logger) {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context) {
            _logger.LogInformation("PersonListActionFilter.OnActionExecuted");
        }

        //此处验证Route参数的合法性，并进行修正
        public void OnActionExecuting(ActionExecutingContext context) {
            _logger.LogInformation("PersonListActionFilter.OnActionExecuting");
            if(context.ActionArguments.ContainsKey("searchBy")) {//如果Route参数有searchBy
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
