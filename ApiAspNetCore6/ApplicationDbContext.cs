using ApiAspNetCore6.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAspNetCore6
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) 
        : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
