using WebApp.Entity;

namespace WebApp.ViewModels
{
    public class EditedCategoryViewModel
    {
        public Category? EditedCategory { get; internal set; }
        public IEnumerable<Category> Categories { get; internal set; }
    }
}
