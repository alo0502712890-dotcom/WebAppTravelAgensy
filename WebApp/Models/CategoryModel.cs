using global::WebApp.DB;
using global::WebApp.Entity;
using WebApp.DB;
using WebApp.Dto;
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


        public IEnumerable<Category> GetAllCategories()
        {
            return _agencyDBContext.Categories.ToList();
        }

        public Category? GetCategoryById(int categoryId)
        {
            return _agencyDBContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool UpdateCategory(Category oldCategory, CategoryEditDto dtoCat, string newFilePath)
        {
            oldCategory.Slug = dtoCat.Slug;
            oldCategory.ParentID = dtoCat.ParentID;
            oldCategory.Description = dtoCat.Description;
            oldCategory.Name = dtoCat.Name;
            oldCategory.ImgSrc = newFilePath;

            return _agencyDBContext.SaveChanges() == 1 ? true : false;
        }


        public bool RemoveCategory(Category remove)
        {
            try
            {
                var isDeleted = _agencyDBContext.Categories.Remove(remove) == null ? false : true;
                _agencyDBContext.SaveChanges();
                return isDeleted;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateCategory(Category category)
        {
            if (_agencyDBContext.Categories.Add(category) == null)
            {
                return false;
            }
            else
            {
                return _agencyDBContext.SaveChanges() == 1 ? true : false;
            }
        }

        public bool SlugExists(string slug)
        {
            return _agencyDBContext.Categories.Any(c => c.Slug == slug);
        }

        public bool SlugExistsForEdit(string slug, int categoryId)
        {
            return _agencyDBContext.Categories
                .Any(c => c.Slug == slug && c.Id != categoryId);
        }

    }

}
