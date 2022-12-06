using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "getRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DataHATEOAS>>> GetRoot()
        {
            var dataHateoas = new List<DataHATEOAS>();
            //root
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getRoot", new { }), description: "self", method: "GET"));
            var isAdmin = await authorizationService.AuthorizeAsync(User, "IsAdmin");
            if(isAdmin.Succeeded)
            {
                //admins
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("promoteToAdmin", new { }), description: "promover a admin", method: "POST"));
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("removeAdmin", new { }), description: "eliminar a admin", method: "POST"));
                //author
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("createAuthor", new { }), description: "crear autor", method: "POST"));
                //book
                dataHateoas.Add(new DataHATEOAS(link: Url.Link("createBook", new { }), description: "crear libro", method: "POST"));
            }
            //account
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("registerUser", new { }), description: "registro de usuario", method: "POST"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("loginUser", new { }), description: "inicio de sesión", method: "POST"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("renewToken", new { }), description: "renovar token", method: "POST"));
            //author
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getAuthorById", new { }), description: "obtener autor por Id", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getAuthorByName", new { }), description: "obtener autor por Nombre", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getAuthors", new { }), description: "renovar token", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("updateAuthor", new { }), description: "actualizar autor", method: "UPDATE"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("deleteAuthor", new { }), description: "eliminar autor", method: "DELETE"));
            //book
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getBookById", new { }), description: "obtener libro", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getBooks", new { }), description: "obtener libros", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("updateBook", new { }), description: "actualizar libro", method: "UPDATE"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("patchBook", new { }), description: "actualizar dato libro", method: "PATCH"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("deleteBook", new { }), description: "eliminar libro", method: "PATCH"));
            //comments
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getCommentById", new { }), description: "obtener comentario", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("getComments", new { }), description: "obtener comentarios", method: "GET"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("createComment", new { }), description: "crear comentario", method: "POST"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("updateComment", new { }), description: "actualizar comentario", method: "PUT"));
            dataHateoas.Add(new DataHATEOAS(link: Url.Link("updateComment", new { }), description: "actualizar comentario", method: "PUT"));
            
            return dataHateoas;
        } 
    }
}
