using ApiAspNetCore6.Validations;

namespace ApiAspNetCore6.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [FirstLetterCapitalized]
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
