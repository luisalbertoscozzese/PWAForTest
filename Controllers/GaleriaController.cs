using Microsoft.AspNetCore.Mvc;

namespace PWAForTest.Controllers
{
    public class GaleriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
