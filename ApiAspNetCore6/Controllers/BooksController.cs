using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using ApiAspNetCore6.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, ILogger<BooksController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }


        [HttpGet("{id:int}", Name = "GetBook")]
        public async Task<ActionResult<DisplayBookWithAuthors>> FindById(int id)
        {
            var book = await context.Books
                .Include(book=>book.AuthorsBooks)
                .ThenInclude(authorBook => authorBook.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            book.AuthorsBooks = book.AuthorsBooks.OrderBy(b=>b.Order).ToList();
            return mapper.Map<DisplayBookWithAuthors>(book);
        }

        [HttpGet]
        public async Task<ActionResult<List<DisplayBook>>> GetAll()
        {
            var books = await context.Books
                .ToListAsync();
            return mapper.Map<List<DisplayBook>>(books);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBook createBook)
        {
            if(createBook.AuthorsIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }
            var authorsIds = await context.Authors
                .Where(author => createBook.AuthorsIds.Contains(author.Id))
                .Select(author => author.Id)
                .ToListAsync();
            if(createBook.AuthorsIds.Count != authorsIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
            var book = mapper.Map<Book>(createBook);
            SortAuthorsInABook(book);
            context.Add(book);
            await context.SaveChangesAsync();
            var displayBook = mapper.Map<DisplayBook>(book);
            return CreatedAtRoute("GetBook", new {id=book.Id}, displayBook);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, UpdateBook updateBook)
        {
            var book = await context.Books
                .Include(x => x.AuthorsBooks)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(book is null)
            {
                return NotFound();
            }
            book = mapper.Map(updateBook, book);
            SortAuthorsInABook(book);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchBook> jsonPatchDocument)
        {
            if(jsonPatchDocument == null)
            {
                return BadRequest();
            }
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if(book is null)
            {
                return NotFound();
            }
            var patchBook = mapper.Map<PatchBook>(book);
            jsonPatchDocument.ApplyTo(patchBook, ModelState);
            var isValid = TryValidateModel(patchBook);
            if(!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(patchBook, book);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void SortAuthorsInABook(Book book)
        {
            if (book is not null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Order = i;
                }
            }
        }

    }
}
