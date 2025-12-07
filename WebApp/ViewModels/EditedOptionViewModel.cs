using WebApp.Entity;

namespace WebApp.ViewModels
{
    public class EditedOptionViewModel
    {
        public Option Option { get; set; }

        public IEnumerable<string> Relations { get; set; }
    }
}
