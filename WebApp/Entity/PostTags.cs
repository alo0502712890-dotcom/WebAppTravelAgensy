using System.ComponentModel.DataAnnotations.Schema;
using Azure;

namespace WebApp.Entity
{
    public class PostTags
    {
        public int Id { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }


        [ForeignKey("TagId")]
        public int TagId { get; set; }


        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
