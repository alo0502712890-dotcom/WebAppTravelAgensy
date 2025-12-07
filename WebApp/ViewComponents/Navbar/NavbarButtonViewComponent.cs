using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents.Navbar
{
    public class NavbarButtonViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;
        private readonly OptionModels _optionModel;

        public NavbarButtonViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }

        public IViewComponentResult Invoke()
        {
            var button = _optionModel.GetNavbarButton();
            return View("NavbarButton", button);
        }
    }
}
