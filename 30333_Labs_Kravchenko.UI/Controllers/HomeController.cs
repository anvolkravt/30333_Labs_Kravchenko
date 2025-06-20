using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _30333_Labs_Kravchenko.UI.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly List<ListDemo> _listData;
        public HomeController(ILogger<HomeController> logger)
        {
            //_logger = logger;
            _listData = new List<ListDemo>
            {
                new ListDemo {Id=1, Name="Item 1"},
                new ListDemo {Id=2, Name="Item 2"},
                new ListDemo {Id=3, Name="Item 3"}
            };
        }

        public IActionResult Index()
        {
            ViewData["LabTitle"] = "Лабораторная работа №2";
            //SelectList data = new SelectList(_listData, "Id", "Name");
            ViewData["List"] = new SelectList(_listData, "Id", "Name");
            return View();
        }
        public class ListDemo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}
