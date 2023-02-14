using Microsoft.AspNetCore.Mvc;

namespace Exercise.WebApi
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
