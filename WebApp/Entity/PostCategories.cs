using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entity
{
    public class PostCategories
    {
        public int Id { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public virtual Post Post { get; set; }

        public virtual Category Category { get; set; }
    }
}
