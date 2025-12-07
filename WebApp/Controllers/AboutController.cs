using Microsoft.AspNetCore.Mvc;
using WebApp.Entity;

namespace WebApp.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult Index()         //опис про нас
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()   //наші контакти
        {
            return View();
        }

        [HttpPost]
        public ViewResult SaveClientMessage(ClientMessage clientMessage)
        {
            if (ModelState.IsValid)
            {
                // save to db
                return View("Thanks", clientMessage);
            }
            else
            {
                return View("ContactUs");
            }
        }

        public IActionResult Guides()     //наші гіди
        {
            return View();
        }

        public IActionResult Gallery()     //наша галерея
        {
            return View();
        }
    }
}
