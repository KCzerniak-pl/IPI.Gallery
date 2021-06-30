using System.ComponentModel.DataAnnotations;

namespace GalleryWebApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "To pole jest wymagane")]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres e-mail")]
        [Display(Name = "Adres e-mail:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(25, ErrorMessage = "Hasło musi mieć minium {2} i maksymalnie {1} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło:")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RemeberMe { get; set; }
    }
}