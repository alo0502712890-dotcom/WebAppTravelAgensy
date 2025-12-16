using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.ViewComponents
{
    public class UserDashboardMenuViewComponent : ViewComponent
    {
        private readonly OptionModels _optionModel;

        public UserDashboardMenuViewComponent(OptionModels optionModel)
        {
            _optionModel = optionModel;
        }

        public IViewComponentResult Invoke()
        {
            var items = _optionModel.GetOptionsByRelation("dashboard-item");
            return View(items);
        }
    }
}
