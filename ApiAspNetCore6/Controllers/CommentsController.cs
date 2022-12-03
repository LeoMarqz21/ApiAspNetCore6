using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<DisplayComment>>> GetAll(int bookId)
        {
            var bookExist = await context.Books
                .AnyAsync(book => book.Id == bookId);
            if (!bookExist)
            {
                return NotFound();
            }
            var comments = await context.Comments
                .Where(comment=>comment.BookId == bookId).ToListAsync();
            return mapper.Map<List<DisplayComment>>(comments);
        }

        [HttpPost]
        public async Task<ActionResult> Create(int bookId, CreateComment createComment)
        {
            var bookExist = await context.Books.AnyAsync(book=>book.Id == bookId);
            if(!bookExist)
            {
                return NotFound();
            }
            var comment = mapper.Map<Comment>(createComment);
            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
