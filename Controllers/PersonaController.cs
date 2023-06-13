using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;

namespace PWAForTest.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public List<PersonaCLS> listarPersonas() 
        { 
            List<PersonaCLS> lista= new List<PersonaCLS>();
            using (DbA9acbfDbbibliotecaContext db = new DbA9acbfDbbibliotecaContext()) 
            { 
                lista= (from persona in db.Personas 
                       where persona.Bhabilitado==1
                       select new PersonaCLS 
                       { 
                       iidpersona= persona.Iidpersona,
                       nombreCompleto= persona.Nombre+" "+persona.Appaterno+" "+persona.Apmaterno,
                       correo=persona.Correo
                       
                       }).ToList();
                return lista;
            
            }
        
        }

    }
}
