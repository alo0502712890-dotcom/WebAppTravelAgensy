using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.Entity
{
    /// <summary>
    /// Класс, представляющий пользователя.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        [Required(ErrorMessage = "Email обязателен для заполнения.")]
        [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
        [StringLength(128, ErrorMessage = "Электронная почта не должна превышать 128 символов.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [Required(ErrorMessage = "Логин обязателен для заполнения.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 32 символов.")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата создания пользователя.
        /// </summary>
        public DateTime DateOfCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата последнего обновления данных пользователя.
        /// </summary>
        /// 

        [ValidateNever]
        public DateTime? DateOfUpdated { get; set; } = null;

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

    }
}
