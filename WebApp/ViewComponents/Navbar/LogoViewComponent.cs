using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Entity;
using WebApp.Models;

namespace WebApp.ViewComponents.Navbar
{
    public class LogoViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private OptionModels _optionModel;
        public LogoViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }

        public IViewComponentResult Invoke()
        {
            return View("Logo", _optionModel.GetSiteLogo());
        }
    }
    
}
