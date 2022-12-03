using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.DTOs
{
    public class CreateBook
    {
        [FirstLetterCapitalized]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public List<int> AuthorsIds { get; set; }
    }
}
