using Microsoft.AspNetCore.Mvc;

namespace PWAForTest.Controllers
{
    public class ContactosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
