using ApiAspNetCore6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<BooksController> logger;

        public BooksController(ApplicationDbContext context, ILogger<BooksController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await context.Books.Include(x => x.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Book book)
        {
            var existAuthor = await context.Authors.AnyAsync(a => a.Id == book.AuthorId);
            if (!existAuthor)
            {
                return BadRequest($"No existe un autor con id:[{book.AuthorId}]");
            }
            context.Add(book);
            await context.SaveChangesAsync();
            return Ok("Libro agregado");
        }

        [HttpGet("test/run")]
        public ActionResult TestHeader([FromHeader] string nameApp, [FromQuery] string author)
        {
            return Ok(new { nameApp, author });
        }
    }
}
