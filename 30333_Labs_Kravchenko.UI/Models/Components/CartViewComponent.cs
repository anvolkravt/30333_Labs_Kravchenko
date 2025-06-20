using Microsoft.AspNetCore.Mvc;

namespace _30333_Labs_Kravchenko.UI.Models.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
