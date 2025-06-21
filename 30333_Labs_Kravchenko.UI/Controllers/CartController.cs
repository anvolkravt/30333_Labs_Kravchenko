using Microsoft.AspNetCore.Mvc;

namespace _30333_Labs_Kravchenko.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Add(int id, string returnUrl)
        {
            // Заглушка: выводим параметры для отладки
            ViewData["Message"] = $"Добавлен препарат с ID: {id}, ReturnUrl: {returnUrl}";
            return View();
        }
    }
}
