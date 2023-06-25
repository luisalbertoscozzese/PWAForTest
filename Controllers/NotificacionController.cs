using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;
using WebPush;

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

        public async Task<int> enviarNotificaciones(string parametroPorContenido) 
        {
            int rpta = 0;

            string subject = @"mailto:luisalbertoscozzese@gmail.com";
            string clavePublica = Notificaciones.llavePublica;
            string clavePrivada = Notificaciones.llavePrivada;

            var vapidDetail = new VapidDetails(subject, clavePublica, clavePrivada);

            PushSubscription oPushSubscription;

            var webPushClient=new WebPushClient();

            try
            {
                using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext())
                {
                    List<Notificacione> lista = bd.Notificaciones.ToList();

                    foreach (Notificacione oNotificacione in lista)
                    {
                        try
                        {


                            oPushSubscription = new PushSubscription(oNotificacione.Endpointnotificacion,
                                oNotificacione.P256dhnotificacion, oNotificacione.Authnotificacion);
                            await webPushClient.SendNotificationAsync(oPushSubscription, parametroPorContenido,
                                vapidDetail);
                            rpta = 1;
                        }
                        catch (WebPushException ex)
                        {
                            if (ex.StatusCode.ToString() == "Gone")
                            {
                                bd.Remove(oNotificacione);
                                bd.SaveChanges();
                                rpta = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }

            return rpta;


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
