using System.ComponentModel.DataAnnotations;

namespace WebApp.Entity
{
    public class ClientMessage
    {
        /// <summary>
        /// Идентификатор сообщения клиента.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя, отправившего сообщение.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, заполните имя")]
        [StringLength(100, ErrorMessage = "Имя пользователя не может быть длиннее 100 символов.")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email пользователя.
        /// Поле обязательно для заполнения.
        /// Проверка на правильность формата email.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, заполните email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Укажите валидный email адрес")]
        [MaxLength(128, ErrorMessage = "Email может быть не более 128 символов")]
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Тема сообщения (не обязательное поле).
        /// </summary>
        [StringLength(200, ErrorMessage = "Тема сообщения не может быть длиннее 200 символов.")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Тело сообщения.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, заполните тело письма")]
        [StringLength(1000, ErrorMessage = "Сообщение не может быть длиннее 1000 символов.")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания сообщения.
        /// По умолчанию — текущее время.
        /// </summary>
        public DateTime DateOfCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Флаг, отвечающий за статус ответа на сообщение.
        /// По умолчанию — не отвечено.
        /// </summary>
        public bool IsAnswered { get; set; } = false;
    }
}
