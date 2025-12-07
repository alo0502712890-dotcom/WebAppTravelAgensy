using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Entity
{
    /// <summary>
    /// Представляет опцию в системе (например, настройки).
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Уникальный идентификатор для опции.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название опции.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, укажите название опции.")]
        [StringLength(200, ErrorMessage = "Название опции не может быть длиннее 200 символов.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ключ опции.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, укажите ключ опции.")]
        [StringLength(1000, ErrorMessage = "Ключ опции не может быть длиннее 100 символов.")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Значение опции.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, укажите значение опции.")]
        [StringLength(500, ErrorMessage = "Значение опции не может быть длиннее 500 символов.")]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Связь опции с другой сущностью (например, описание).
        /// </summary>
        [StringLength(500, ErrorMessage = "Описание не может быть длиннее 500 символов.")]
        public string Relation { get; set; } = string.Empty;

        /// <summary>
        /// Порядковый номер опции в списке.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Порядковый номер должен быть положительным числом.")]
        public int Order { get; set; }

        /// <summary>
        /// Указывает, является ли опция системной.
        /// По умолчанию — false.
        /// </summary>
        [DefaultValue(false)]
        public bool IsSystem { get; set; } = false;
    }
}
