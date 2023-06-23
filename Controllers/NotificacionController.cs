using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;

namespace PWAForTest.Controllers
{
    public class NotificacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string generarllavePublica()
        {
            return Notificaciones.llavePublica;
        }

        public int guardarSubscripcion(SubscripcionCLS suscripcion)
        {
            int rpta = 0;

            try
            {
                using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext())
                {
                    Notificacione notificacione= new Notificacione();
                    notificacione.Endpointnotificacion = suscripcion.endpoint;
                    notificacione.Authnotificacion = suscripcion.auth;
                    notificacione.P256dhnotificacion = suscripcion.p256dh;
                    notificacione.Bhabilitado = 1;
                    bd.Notificaciones.Add(notificacione);
                    bd.SaveChanges();
                    rpta = 1;
                }

            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }


    }
}
