using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

namespace WebApp.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserLogin { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime DateOfCreated { get; set; } = DateTime.Now;
        public bool IsRequired { get; set; } = false;

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        [ForeignKey("ParentId")]
        public int? ParentCommentId { get; set; } = null;

        public virtual Post Post { get; set; }  // Navigation to Post

        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }  // Navigation to parent comment

        public ICollection<Comment> Childs { get; set; } = new List<Comment>();
    }
}
