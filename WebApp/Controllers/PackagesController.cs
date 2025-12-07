using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PackagesController : Controller
    {
        public IActionResult Index()          //слайдер пакетів турів
        {
            return View();
        }

        public IActionResult Destination()    //популярні напрями
        {
            return View();
        }

        public IActionResult Tour()           //підбір туру
        {
            return View();
        }
    }
}
