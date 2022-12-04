using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.DTOs
{
    public class UserCredentialsRegister
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]    
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un email valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
