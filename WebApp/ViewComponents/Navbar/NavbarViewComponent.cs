using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Models;

namespace WebApp.ViewComponents.Navbar
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AgencyDBContext _context;

        public NavbarViewComponent(AgencyDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View("Navbar");
        }
    }
}
