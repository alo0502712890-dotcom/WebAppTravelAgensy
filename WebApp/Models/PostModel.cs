using Microsoft.EntityFrameworkCore;
using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class PostModel
    {
        private AgencyDBContext _agencyDBContext;

        public PostModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }

        public IEnumerable<Post> GetPosts()
        {
            return _agencyDBContext.Posts.Where(p => p.PostStatuses == PostStatuses.Published).OrderBy(p => p.DateOfPublished)
                  .ToList();
        }

        public IEnumerable<Post> GetPostsByCategorySlug(string categorySlug)
        {
            var posts = from post in _agencyDBContext.Posts
                        join postCategory in _agencyDBContext.PostCategories
                            on post.Id equals postCategory.PostId
                        join category in _agencyDBContext.Categories
                            on postCategory.CategoryId equals category.Id
                        where category.Slug == categorySlug
                        select post;

            return posts.ToList();
        }

        public IEnumerable<Post> GetPostsByTagSlug(string tagSlug)
        {
            var posts = from post in _agencyDBContext.Posts
                        join postTags in _agencyDBContext.PostTags
                            on post.Id equals postTags.PostId
                        join tags in _agencyDBContext.Tags
                            on postTags.TagId equals tags.Id
                        where tags.Slug == tagSlug
                        select post;

            return posts.ToList();
        }

        public Post? GetPostBySlug(string slug)
        {
            return _agencyDBContext.Posts.Where(p => p.Slug.ToLower() == slug.ToLower()).FirstOrDefault();
        }



        public (List<Post> Posts, int TotalCount) GetPagedPosts(int page, int pageSize, string? category = null, string? tag = null)
        {
            IQueryable<Post> query = _agencyDBContext.Posts
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.PostCategories).ThenInclude(pc => pc.Category)
                .ThenInclude(c => c.Parent)               
                .OrderByDescending(p => p.DateOfPublished);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.PostCategories.Any(pc => pc.Category.Slug == category));
            }

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Slug == tag));
            }

            int total = query.Count();

            var posts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (posts, total);
        }

        public Post? GetPostById(int id)
        {
            return _agencyDBContext.Posts
                .FirstOrDefault(p => p.Id == id);
        }


    }
}
