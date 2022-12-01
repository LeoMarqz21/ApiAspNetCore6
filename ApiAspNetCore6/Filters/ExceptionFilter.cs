using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiAspNetCore6.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute //filtro global
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }

    }
}
