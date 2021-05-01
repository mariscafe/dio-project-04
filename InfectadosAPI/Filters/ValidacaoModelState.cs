using InfectadosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace InfectadosAPI.Filters
{
    public class ValidacaoModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validacao = new ErrorModel(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage));

                context.Result = new BadRequestObjectResult(validacao);
            }
        }
    }
}
