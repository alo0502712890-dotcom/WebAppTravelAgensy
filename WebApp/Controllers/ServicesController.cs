using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()           //наші послуги
        {
            return View();
        }

        public IActionResult Testimonial()     //відгуки
        {
            return View();
        }

        public IActionResult Booking()        //забронювать тур
        {
            return View();
        }
    }
}
