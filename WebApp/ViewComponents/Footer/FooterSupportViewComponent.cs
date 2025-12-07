using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents.Footer
{
    public class FooterSupportViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private OptionModels _optionModel;
        public FooterSupportViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }
        public IViewComponentResult Invoke()
        {
            return View("FooterSupport", _optionModel.GetFooterSupport());
        }
    }
}
