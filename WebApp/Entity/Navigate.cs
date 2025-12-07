using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entity
{
    /// <summary>
    /// Представляет элемент навигации в приложении.
    /// </summary>
    public class Navigate
    {
        /// <summary>
        /// Уникальный идентификатор для элемента навигации.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок элемента навигации (например, текст ссылки).
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, укажите заголовок.")]
        [StringLength(200, ErrorMessage = "Заголовок не может быть длиннее 200 символов.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-ссылка, на которую ведет элемент навигации.
        /// Поле обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Пожалуйста, укажите URL.")]
        [Url(ErrorMessage = "Пожалуйста, укажите корректный URL.")]
        [StringLength(500, ErrorMessage = "URL не может быть длиннее 500 символов.")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Порядковый номер элемента в меню или навигации.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Порядковый номер должен быть положительным числом.")]
        public int Order { get; set; }

        /// <summary>
        /// Идентификатор родительского элемента навигации.
        /// Может быть null, если данный элемент не имеет родителя.
        /// </summary>
        [ForeignKey("ParentID")]
        public int? ParentID { get; set; } = null;

        /// <summary>
        /// Список дочерних элементов навигации, связанных с текущим элементом.
        /// </summary>
        public ICollection<Navigate> Childs { get; set; } = new List<Navigate>();
    }
}
