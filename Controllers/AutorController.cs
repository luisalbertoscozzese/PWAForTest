using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;

namespace PWAForTest.Controllers
{
    public class AutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<AutorCLS> listarAutor()
        {
            var lista= new List<AutorCLS>();
            using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext())
            {
                lista = (from autor in bd.Autors
                         where autor.Bhabilitado==1
                         select new AutorCLS
                         {
                             iidautor=autor.Iidautor,
                             nombreautor= autor.Nombre+ " " +autor.Appaterno + " " + autor.Apmaterno
                         }).ToList();
            }

            return lista;
        }
    }
}
