using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Filters;
using ApiAspNetCore6.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiAspNetCore6.Utilities
{
    public class HATEOASAuthorFilterAttribute : HATEOASFilterAttribute
    {
        private readonly ApiLinkGenerator apiLinkGenerator;

        public HATEOASAuthorFilterAttribute(ApiLinkGenerator apiLinkGenerator)
        {
            this.apiLinkGenerator = apiLinkGenerator;
        }

        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context, 
            ResultExecutionDelegate next
            )
        {
            var include = IncludeHATEOAS(context);
            if(!include)
            {
                await next();
                return;
            }
            var result = context.Result as ObjectResult;
            var model = result.Value as DisplayAuthor ?? 
                throw new ArgumentNullException($"Se esperaba una instancia de {nameof(result.Value)}");
            await apiLinkGenerator.GenerateLinks(model);
            await next();

        }
    }
}
