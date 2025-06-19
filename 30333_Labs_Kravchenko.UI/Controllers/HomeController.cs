using Microsoft.AspNetCore.Mvc;

namespace _30333_Labs_Kravchenko.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
