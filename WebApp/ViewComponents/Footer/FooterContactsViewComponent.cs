using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents.Footer
{
    public class FooterContactsViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private OptionModels _optionModel;
        public FooterContactsViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }
        public IViewComponentResult Invoke()
        {
            return View("FooterContacts", _optionModel.GetFooterContacts());
        }
    }
}
