using WebApp.Entity;

namespace WebApp.ViewModels
{
    public class TopBarViewModel
    {
        public IEnumerable<Option> SocialLinks { get; set; } = new List<Option>();

        public IEnumerable<Option> UserMenu { get; set; } = new List<Option>();

        public IEnumerable<Option> DashboardItems { get; set; } = new List<Option>();

    }
}
