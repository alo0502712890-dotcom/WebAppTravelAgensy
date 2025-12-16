using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Логін обов'язковий")]
        [Display(Name = "Логін")]
        [MaxLength(50)]
        public string Login { get; set; }

        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю обов'язкове")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }

        
        [Required(ErrorMessage = "Ви повинні погодитись з умовами")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Ви повинні погодитись з умовами")]
        public bool Check { get; set; }

        public string? ErrorMessage { get; set; }
    }
}