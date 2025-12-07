using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents
{
    public class SocialMediaViewComponent : ViewComponent
    {
        private readonly OptionModels _optionModel;

        public SocialMediaViewComponent(AgencyDBContext context)
        {
            _optionModel = new OptionModels(context);
        }

        public IViewComponentResult Invoke()
        {
            var links = _optionModel.GetSocialLinks();
            return View("SocialMedia", links);
        }
    }
}
