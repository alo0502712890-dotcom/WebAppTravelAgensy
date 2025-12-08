namespace WebApp.Dto
{
    public class CategoryEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
