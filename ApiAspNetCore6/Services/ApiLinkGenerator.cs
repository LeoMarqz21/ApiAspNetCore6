using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace ApiAspNetCore6.Services
{
    public class ApiLinkGenerator
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;

        public ApiLinkGenerator(
            IAuthorizationService authorizationService, 
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor
            )
        {
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper BuildUrlHelper()
        {
            var factory = httpContextAccessor
                .HttpContext.RequestServices
                .GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private async Task<bool> IsAdmin()
        {
            var result = await authorizationService
                .AuthorizeAsync(httpContextAccessor.HttpContext.User, "IsAdmin");
            return result.Succeeded;
        }

        public async Task GenerateLinks(DisplayAuthor displayAuthor)
        {

            var isAdmin = await IsAdmin();
            var Url = BuildUrlHelper();

            displayAuthor.Links.Add(
            new DataHATEOAS(
                    Url.Link("getAuthorById", new { id = displayAuthor.Id }),
                    "Self",
                    "GET"
                    )
                );
            if(isAdmin)
            {
                displayAuthor.Links.Add(new DataHATEOAS(
                    Url.Link("updateAuthor", new { id = displayAuthor.Id }),
                    "actualizar autor",
                    "PUT"
                    ));
                displayAuthor.Links.Add(new DataHATEOAS(
                    Url.Link("deleteAuthor", new { id = displayAuthor.Id }),
                    "eliminar autor",
                    "DELETE"
                    ));
            }
                

        }

    }
}
