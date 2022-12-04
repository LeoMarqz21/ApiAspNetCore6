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
    [Route("api/books/{bookId:int}/comments")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetComment")]
        public async Task<ActionResult<DisplayComment>> FindById(int id)
        {
            var comment = await context.Comments
                .FirstOrDefaultAsync(comment=> comment.Id == id);
            if(comment is null)
            {
                return NotFound();
            }
            return mapper.Map<DisplayComment>(comment);
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
            var displayComment = mapper.Map<DisplayComment>(comment);
            return CreatedAtRoute("GetComment", new {id=comment.Id, bookId = comment.BookId}, displayComment);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(UpdateComment updateComment, int bookId, int id)
        {
            var bookExist = await context.Books.AnyAsync(book => book.Id == bookId);
            if (!bookExist)
            {
                return NotFound();
            }
            var commentExist = await context.Comments.AnyAsync(comment => comment.Id == id);
            if(!commentExist)
            {
                return NotFound();
            }
            var comment = mapper.Map<Comment>(updateComment);
            comment.Id = id;
            comment.BookId = bookId;
            context.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
