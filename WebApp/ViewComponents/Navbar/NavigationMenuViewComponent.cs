using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Entity;
using WebApp.Models;

namespace WebApp.ViewComponents.Navbar
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private NavigationModel _navigationModel;
        public NavigationMenuViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _navigationModel = new NavigationModel(agencyDBContext);
        }

        public IViewComponentResult Invoke()
        {
            return View("NavigationMenu", _navigationModel.GetNavigationThree());
        }
    }
}
