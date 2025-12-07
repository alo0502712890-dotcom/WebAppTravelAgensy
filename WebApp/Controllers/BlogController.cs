using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebApp.DB;
using WebApp.Entity;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AgencyDBContext _agencyDBContext;

        private TagModel _tagModel;
        private CategoryModel _categoryModel;
        private PostModel _postModel;

        public BlogController(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _tagModel = new TagModel(agencyDBContext);
            _categoryModel = new CategoryModel(agencyDBContext);
            _postModel = new PostModel(agencyDBContext);
        }
        
        
        [HttpGet]
        [HttpGet]
        public IActionResult Index(string? category, string? tag, int page = 1)
        {
            int pageSize = 6;

            var result = _postModel.GetPagedPosts(page, pageSize, category, tag);
            int totalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);

            // ---------- СТВОРЮЄМО ДЕРЕВО ТІЛЬКИ ПО ВИДИМИХ ПОСТАХ ----------
            var usedCategories = result.Posts
                .SelectMany(p => p.PostCategories.Select(pc => pc.Category))
                .Distinct()
                .ToList();

            var rootCategories = usedCategories
                .Select(c => c.ParentID == null ? c : c.Parent)
                .Distinct()
                .ToList();

            foreach (var parent in rootCategories)
            {
                parent.Childs = usedCategories
                    .Where(c => c.ParentID == parent.Id)
                    .ToList();
            }

            // ---------- Формуємо ViewModel ----------
            BlogViewModel vm = new BlogViewModel
            {
                Posts = result.Posts,
                Categories = rootCategories,      // ← ТІЛЬКИ потрібні категорії
                Tags = result.Posts
                    .SelectMany(p => p.PostTags.Select(pt => pt.Tag))
                    .Distinct()
                    .ToList(),
                CurrentPage = page,
                TotalPages = totalPages,
                CategorySlug = category,
                TagSlug = tag,
                PageSize = pageSize
            };

            return View(vm);
        }




        [HttpGet]
        public IActionResult Post(string? slug)
        {
            if (slug == null)
            {
                return RedirectToAction("Index", "Error");
            }
            Post? onePost = _postModel.GetPostBySlug(slug);
            if (onePost == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View(onePost);
        }
    }
}
