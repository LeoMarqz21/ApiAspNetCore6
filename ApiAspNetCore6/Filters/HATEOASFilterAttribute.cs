using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiAspNetCore6.Filters
{
    public class HATEOASFilterAttribute : ResultFilterAttribute
    {
        protected bool IncludeHATEOAS(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            if(!IsSuccessful(result))
            {
                return false;
            }
            var headers = context.HttpContext.Request.Headers["includeHATEOAS"];
            if(headers.Count== 0)
            {
                return false;
            }
            var value = headers[0];
            if(!value.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        private bool IsSuccessful(ObjectResult objectResult)
        {
            if(objectResult != null || objectResult.Value == null)
            {
                return false;
            }
            if(objectResult.StatusCode.HasValue && !objectResult.StatusCode.Value.ToString().StartsWith("2"))
            {
                return false;
            }
            return true;
        }
    }
}
