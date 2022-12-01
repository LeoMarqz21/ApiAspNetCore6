using ApiAspNetCore6.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiAspNetCore6.Entities
{
    public class Author //: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, MinimumLength = 4, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres")]
        [FirstLetterCapitalized]
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(!string.IsNullOrEmpty(Name))
        //    {
        //        var firstLetter = Name[0].ToString();
        //        if(firstLetter != firstLetter.ToUpper())
        //        {
        //            yield return new ValidationResult("La primer letra debe ser mayuscula",
        //                new string[] { nameof(Name) });
        //        }

        //    }
        //}
    }
}
