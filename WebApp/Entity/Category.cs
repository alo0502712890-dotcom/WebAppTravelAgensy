using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entity
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string Slug { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public string ImgSrc { get; set; } = String.Empty;

        [ForeignKey("ParentID")]
        public int? ParentID { get; set; } = null;


        public virtual Category? Parent { get; set; }
        public ICollection<Category> Childs { get; set; } = new List<Category>();
    }
}
