using WebApp.Entity;

namespace WebApp.ViewModels
{
    public class BlogViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        // -------- Пагінація --------
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 6; // 6 постів на сторінку

        // -------- Фільтри --------
        public string? CategorySlug { get; set; }
        public string? TagSlug { get; set; }
    }
}
