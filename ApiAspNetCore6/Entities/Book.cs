using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [FirstLetterCapitalized]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AuthorBook> AuthorsBooks { get; set; }
    }
}
