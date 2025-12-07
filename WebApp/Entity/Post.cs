namespace WebApp.Entity
{
    public class Post
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string Slug { get; set; } = String.Empty;

        public string ShortDescription { get; set; } = String.Empty;

        public string ImgSrc { get; set; } = String.Empty;

        public string Context { get; set; } = String.Empty;

        public PostStatuses PostStatuses { get; set; } = PostStatuses.Created;
        public DateTime DateOfCreated { get; set; } = DateTime.Now;

        public DateTime DateOfUpdated { get; set; } = DateTime.Now;

        public DateTime DateOfPublished { get; set; } = DateTime.Now;

        public virtual ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
        public virtual ICollection<PostCategories> PostCategories { get; set; } = new List<PostCategories>();
    }
}
