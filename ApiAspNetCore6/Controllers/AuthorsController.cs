using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AuthorsController> logger;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, ILogger<AuthorsController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        } 

        [HttpGet("{id:int}", Name = "getAuthorById")]
        public async Task<ActionResult<DisplayAuthorWithBooks>> FindById([FromRoute] int id)
        {
            var author = await context.Authors
                .Include(author=>author.AuthorsBooks)
                .ThenInclude(authorBook=>authorBook.Book)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (author is null)
            {
                logger.LogInformation("INFORMATION: /authors/id:int --> author no encontrado");
                return NotFound();
            }
            return mapper.Map<DisplayAuthorWithBooks>(author);
        }

        [HttpGet("{name}", Name = "getAuthorByName")]
        public async Task<ActionResult<List<DisplayAuthor>>> FindByName([FromRoute] string name = null)
        {
            List<Author> authors = await context.Authors.Where(author => author.Name.Contains(name)).ToListAsync();
            if (authors is null)
            {
                return NotFound();
            }
            return mapper.Map<List<DisplayAuthor>>(authors);
        }

        [HttpGet(Name = "getAuthors")]
        public async Task<ActionResult<List<DisplayAuthor>>> GetAll()
        {
            var authors = await context.Authors.ToListAsync();
            return mapper.Map<List<DisplayAuthor>>(authors);
        }


        [HttpPost(Name = "createAuthor")]
        public async Task<ActionResult> Create([FromBody] CreateAuthor createAuthor)
        {
            var exists = await context.Authors.AnyAsync(x => x.Name == createAuthor.Name);
            if (exists)
            {
                return BadRequest($"Ya existe el autor con el nombre [{createAuthor.Name}]");
            }
            var author = mapper.Map<Author>(createAuthor);
            context.Add(author);
            await context.SaveChangesAsync();
            var displayAuthor = mapper.Map<DisplayAuthor>(author);
            return CreatedAtRoute("GetAuthorById", new { id = author.Id }, displayAuthor);
        }

        [HttpPut("{id:int}", Name = "updateAuthor")]
        public async Task<ActionResult> Update([FromBody] UpdateAuthor updateAuthor, [FromRoute] int id)
        {
            var exists = await context.Authors.AnyAsync(a => a.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            var author = mapper.Map<Author>(updateAuthor);
            author.Id = id;
            context.Update(author);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "deleteAuthor")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var exists = await context.Authors.AnyAsync(a => a.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
