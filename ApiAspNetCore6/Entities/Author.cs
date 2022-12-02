using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, MinimumLength = 4, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]
        [FirstLetterCapitalized]
        public string Name { get; set; }

    }
}
