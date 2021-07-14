using Microsoft.AspNetCore.Mvc;

namespace WebApp.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
