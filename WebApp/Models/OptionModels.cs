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



        public IEnumerable<Option> GetAllOptions()
        {
            return _agencyDBContext.Options.ToList();
        }

        public Option? GetOptionById(int optionId)
        {
            return _agencyDBContext.Options.FirstOrDefault(o => o.Id == optionId);
        }

        public IEnumerable<string> GetUniqueRelations()
        {
            return _agencyDBContext.Options
            .Select(o => o.Relation)
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Distinct()
            .ToList();
        }

        public bool RemoveOption(Option remove)
        {
            try
            {
                var isDeleted = _agencyDBContext.Options.Remove(remove) == null ? false : true;
                _agencyDBContext.SaveChanges();
                return isDeleted;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool UpdateOption(Option option)
        {
            Option? origin = GetOptionById(option.Id);
            if (origin != null)
            {
                origin.Order = option.Order;
                origin.Name = option.Name;
                origin.Value = option.Value;
                origin.Relation = option.Relation == null ? String.Empty : option.Relation;
                origin.Key = option.Key;

                return _agencyDBContext.SaveChanges() == 1 ? true : false;
            }
            return false;
        }

        public List<string> GetRelations()
        {
            return _agencyDBContext.Options
                .Select(o => o.Relation)
                .Distinct()
                .ToList();
        }


        public bool CreateOption(Option option)
        {
            if (option.Relation == null)
            {
                option.Relation = String.Empty;
            }
            if (_agencyDBContext.Options.Add(option) == null)
            {
                return false;
            }
            else
            {
                return _agencyDBContext.SaveChanges() == 1 ? true : false;
            }
        }

        public List<Option> GetOptionsByRelation(string relation)
        {
            return _agencyDBContext.Options
                .Where(o => o.Relation == relation && o.IsSystem)
                .OrderBy(o => o.Id)
                .ToList();
        }

    }
}
