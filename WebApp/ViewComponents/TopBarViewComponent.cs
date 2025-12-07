using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Entity;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.ViewComponents
{
    public class TopBarViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _agencyDBContext;

        private OptionModels _optionModel;
        public TopBarViewComponent(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _optionModel = new OptionModels(agencyDBContext);
        }

        public IViewComponentResult Invoke()
        {
            TopBarViewModel topBarViewModel = new TopBarViewModel();

            topBarViewModel.SocialLinks = _optionModel.GetSocialLinks();
            topBarViewModel.UserMenu = _optionModel.GetUserMenu();
            topBarViewModel.DashboardItems = _optionModel.GetDashboardItems();

            return View("TopBar", topBarViewModel);
        }
    }
}
