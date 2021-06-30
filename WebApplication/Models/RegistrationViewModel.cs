using System.ComponentModel.DataAnnotations;

namespace GalleryWebApplication.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(25, ErrorMessage = "Imię może mieć maksymalnie {1} znaków")]
        [Display(Name = "Imię:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(25, ErrorMessage = "Nazwisko może mieć maksymalnie {1} znaków")]
        [Display(Name = "Nazwisko:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres e-mail")]
        [Display(Name = "Adres e-mail:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(25, ErrorMessage = "Hasło musi mieć minium {2} i maksymalnie {1} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [StringLength(25)]
        [Compare("Password", ErrorMessage = "Wpisane hasła nie są takie same")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło:")]
        public string ConfirmPassword { get; set; }
    }
}