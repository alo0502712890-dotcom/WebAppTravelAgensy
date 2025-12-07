using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents.Footer
{
    public class FooterCompanyViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private OptionModels _optionModel;
        public FooterCompanyViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }
        public IViewComponentResult Invoke()
        {
            return View("FooterCompany", _optionModel.GetFooterCompany());
        }
    }
}
