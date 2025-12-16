using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.Entity
{
    /// <summary>
    /// Класс, представляющий пользователя.
    /// </summary>
    public class User : IdentityUser<int>
    {
        [MaxLength(50)]
        [Display(Name = "Логін")]
        public string Login { get; set; }

        public DateTime DateOfCreated { get; set; } = DateTime.Now;
        public DateTime? DateOfUpdated { get; set; }
    }
}
