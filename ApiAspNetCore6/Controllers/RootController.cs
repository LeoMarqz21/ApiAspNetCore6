using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<DataHATEOAS>> GetRoot()
        {
            var dataHateoas = new List<DataHATEOAS>();

            dataHateoas.Add(new DataHATEOAS(link: Url.Link("get-root", new { }), description: "self", method: "GET"));

            return dataHateoas;
        } 
    }
}
