using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ConfirmGoogleViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Логін обов'язковий")]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль мінімум 6 символів")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ви повинні погодитись з умовами")]
        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }
    }
}
