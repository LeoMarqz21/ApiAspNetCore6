using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.DTOs
{
    public class PatchBook
    {
        [Required]
        [FirstLetterCapitalized]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
