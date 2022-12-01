using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiAspNetCore6.Filters
{
    public class MyActionFilter : IActionFilter //filtros a nivel de accion 
    {
        private readonly ILogger<MyActionFilter> logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            this.logger = logger;
        }

        //first
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la acción");
        }

        //second
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de ejecutar la acción");
        }

    }
}
