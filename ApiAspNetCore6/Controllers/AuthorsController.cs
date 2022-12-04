﻿using ApiAspNetCore6.DTOs;
using ApiAspNetCore6.Entities;
using AutoMapper;
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
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, ILogger<AuthorsController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
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

        [HttpGet("{name}")]
        public async Task<ActionResult<List<DisplayAuthor>>> FindByName([FromRoute] string name = null)
        {
            List<Author> authors = await context.Authors.Where(author => author.Name.Contains(name)).ToListAsync();
            if (authors is null)
            {
                return NotFound();
            }
            return mapper.Map<List<DisplayAuthor>>(authors);
        }

        [HttpGet]
        public async Task<ActionResult<List<DisplayAuthor>>> GetAll()
        {
            var authors = await context.Authors.ToListAsync();
            return mapper.Map<List<DisplayAuthor>>(authors);
        }


        [HttpPost]
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
            return CreatedAtRoute("GetAuthor", new { id = author.Id }, displayAuthor);
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
