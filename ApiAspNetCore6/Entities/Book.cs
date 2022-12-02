using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [FirstLetterCapitalized]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
    }
}
