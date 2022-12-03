﻿using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using ApiAspNetCore6.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<DisplayBook>> Get(int id)
        {
            var book = await context.Books
                .Include(book=>book.AuthorsBooks)
                .ThenInclude(authorBook => authorBook.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            return mapper.Map<DisplayBook>(book);
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
            if(book is not null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Order = i;
                }
            }
            context.Add(book);
            await context.SaveChangesAsync();
            return Ok("Libro agregado");
        }

    }
}
