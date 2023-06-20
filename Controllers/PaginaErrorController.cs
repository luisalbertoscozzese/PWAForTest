using Microsoft.AspNetCore.Mvc;

namespace PWAForTest.Controllers
{
    public class PaginaErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
