using WebApp.DB;
using WebApp.Entity;

namespace WebApp.Models
{
    public class OptionModels
    {
        private AgencyDBContext _agencyDBContext;
        public OptionModels(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
        }

        public IEnumerable<Option> GetSocialLinks()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "social-link").OrderBy(o => o.Order).ToList();
        }

        public IEnumerable<Option> GetUserMenu()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "user-menu").OrderBy(o => o.Order).ToList();
        }

        public Option? GetSiteLogo()
        {
            return _agencyDBContext.Options.FirstOrDefault(o => o.Name == "site-logo");
        }

        public IEnumerable<Option> GetDashboardItems()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "dashboard-item").OrderBy(o => o.Order).ToList();
        }

        public Option? GetNavbarButton()
        {
            return _agencyDBContext.Options.FirstOrDefault(o => o.Relation == "navbar-button");
        }



        public IEnumerable<Option> GetFooterContacts()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "footer-contacts").OrderBy(o => o.Order).ToList();
        }

        public IEnumerable<Option> GetFooterCompany()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "footer-company").OrderBy(o => o.Order).ToList();
        }

        public IEnumerable<Option> GetFooterSupport()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "footer-support").OrderBy(o => o.Order).ToList();
        }

        public IEnumerable<Option> FooterLanguage()
        {
            return _agencyDBContext.Options.Where(o => o.Relation == "footer-language").OrderBy(o => o.Order).ToList();
        }
    }
}
