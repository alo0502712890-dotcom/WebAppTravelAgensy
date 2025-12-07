using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email обов'язковий.")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "Логін обов'язковий.")]
        public string User_Login { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий.")]
        [DataType(DataType.Password)]
        public string User_Password { get; set; }

        [Required(ErrorMessage = "Повтор пароля обов'язковий.")]
        [DataType(DataType.Password)]
        [Compare("User_Password", ErrorMessage = "Паролі не співпадають.")]
        public string User_Password_Confirm { get; set; }

        public string Check { get; set; } 

        public string? ErrorMessage { get; set; }
    }
}
