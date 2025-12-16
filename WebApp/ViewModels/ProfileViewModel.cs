using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Логін")]
        public string Login { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Дата реєстрації")]
        public DateTime DateOfCreated { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Паролі не співпадають")]
        public string? ConfirmNewPassword { get; set; }


    }
}
