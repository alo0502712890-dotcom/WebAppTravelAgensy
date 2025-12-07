using global::WebApp.DB;
using global::WebApp.Entity;
using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class CategoryModel
    {
        private AgencyDBContext _agencyDBContext;
        public CategoryModel(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }

        public IEnumerable<Category> GetCategoryTree()
        {
            var rootCategories = _agencyDBContext.Categories
                                        .Where(c => c.ParentID == null)
                                        .ToList();

            foreach (var category in rootCategories)
            {
                BuildCategoryTree(category);
            }

            return rootCategories;
        }

        private void BuildCategoryTree(Category category)
        {
            var childCategories = _agencyDBContext.Categories
                                         .Where(c => c.ParentID == category.Id)
                                         .ToList();

            foreach (var child in childCategories)
            {
                category.Childs.Add(child);
                BuildCategoryTree(child);
            }
        }
    }

}
