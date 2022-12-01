using ApiAspNetCore6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(ApplicationDbContext context, ILogger<AuthorsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> Get([FromRoute] int id)
        {
            var author = await context.Authors.Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (author is null)
            {
                logger.LogInformation("INFORMATION: /authors/id:int --> author no encontrado");
                return NotFound();
            }
            return author;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Author>> FindByName([FromRoute] string name = null)
        {
            Author author = await context.Authors.Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Name.Contains(name));
            if (author is null)
            {
                return NotFound();
            }
            return author;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll()
        {
            return await context.Authors
                .Include(x => x.Books)
                .ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Author author)
        {
            var exists = await context.Authors.AnyAsync(x => x.Name == author.Name);
            if (exists)
            {
                return BadRequest($"Ya existe el autor con el nombre [{author.Name}]");
            }
            context.Add(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromBody] Author author, [FromRoute] int id)
        {
            if (author.Id != id)
            {
                return BadRequest($"El id:[{author.Id}] de autor no coincide con el id:[{id}] de la url !!!");
            }
            var exists = await context.Authors.AnyAsync(a => a.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            context.Update(author);
            await context.SaveChangesAsync();
            return Ok("Autor agregado");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var exists = await context.Authors.AnyAsync(a => a.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return Ok("Autor eliminado exitosamente");
        }
    }
}
