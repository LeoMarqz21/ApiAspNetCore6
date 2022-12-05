using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.DTOs
{
    public class ProposedAdministrator
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string Email { get; set; }
    }
}
