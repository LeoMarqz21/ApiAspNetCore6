using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.DTOs
{
    public class CreateAuthor
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, MinimumLength = 4, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]
        [FirstLetterCapitalized]
        public string Name { get; set; }
    }
}
