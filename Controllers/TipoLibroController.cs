using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;

namespace PWAForTest.Controllers
{
    public class TipoLibroController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public TipoLibroController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<TipoLibroCLS> listarTipoLibro(string nombreTipoLibrobusqueda) 
        { 
            List<TipoLibroCLS> lista = new List<TipoLibroCLS>();
            string rutaCompleta = Path.Combine(_env.ContentRootPath, "wwwroot/img/NoImgAvailable.png");
            byte[] buffer = System.IO.File.ReadAllBytes(rutaCompleta);
            string base64NoImgAvailable= Convert.ToBase64String(buffer);
            string base64NoImgAvailableFinal ="data:img/png;base64," + base64NoImgAvailable;
            using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext())
            {
                if (nombreTipoLibrobusqueda == null)
                {
                    lista = (from tipoLibro in bd.TipoLibros
                             where tipoLibro.Bhabilitado == 1
                             select new TipoLibroCLS
                             {
                                 idtipolibro = tipoLibro.Iidtipolibro,
                                 nombre = tipoLibro.Nombretipolibro,
                                 descripcion = tipoLibro.Descripcion,
                                 base64= tipoLibro.Nombrearchivo==null? base64NoImgAvailableFinal 
                                 : "data:image/" + Path.GetExtension(tipoLibro.Nombrearchivo).Replace(".","") + ";base64," + Convert.ToBase64String(tipoLibro.Archivo?? new byte[0])
                }).ToList();
                }
                else 
                {
                    lista = (from tipoLibro in bd.TipoLibros
                             where tipoLibro.Bhabilitado == 1 &&
                             tipoLibro.Nombretipolibro.Contains(nombreTipoLibrobusqueda)
                             select new TipoLibroCLS
                             {
                                 idtipolibro = tipoLibro.Iidtipolibro,
                                 nombre = tipoLibro.Nombretipolibro,
                                 descripcion = tipoLibro.Descripcion,
                                 base64 = tipoLibro.Nombrearchivo == null ? base64NoImgAvailableFinal
                                 : "data:image/" + Path.GetExtension(tipoLibro.Nombrearchivo).Replace(".", "") + ";base64," + Convert.ToBase64String(tipoLibro.Archivo ?? new byte[0])
                             }).ToList();

                }



                return lista;
            }
        }

        public TipoLibroCLS recuperarTipoLibro(int id) 
        {
            using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext())
            {
                TipoLibroCLS tipoLibroCLS= new TipoLibroCLS();

                TipoLibro tipoLibro = bd.TipoLibros.Where(x => x.Iidtipolibro == id).First();

                tipoLibroCLS.idtipolibro = tipoLibro.Iidtipolibro;

                tipoLibroCLS.nombre = tipoLibro.Nombretipolibro;

                tipoLibroCLS.descripcion = tipoLibro.Descripcion;

                tipoLibroCLS.base64 = tipoLibro.Archivo==null? string.Empty: "data:image/" + Path.GetExtension(tipoLibro.Nombrearchivo).Replace(".","") + ";base64,"+ Convert.ToBase64String(tipoLibro.Archivo);

                return tipoLibroCLS;
            }
        }

        public int guardarTipoLibro(TipoLibroCLS tipoLibroCLS, IFormFile fotoEnviar) 
        {
            int rpta = 0;
            byte[] buffer;
            string nombreFoto = string.Empty;

            if (fotoEnviar != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fotoEnviar.CopyTo(ms);
                    nombreFoto = fotoEnviar.FileName;
                    buffer = ms.ToArray();
                    tipoLibroCLS.foto = buffer;
                    tipoLibroCLS.nombreFoto = nombreFoto;
                }
            }

            using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext()) 
            {
                try
                {
                    if (tipoLibroCLS.idtipolibro == 0)
                    {
                        TipoLibro tipoLibro = new TipoLibro();
                        tipoLibro.Nombretipolibro = tipoLibroCLS.nombre;
                        tipoLibro.Descripcion = tipoLibroCLS.descripcion;
                        tipoLibro.Archivo = tipoLibroCLS.foto;
                        tipoLibro.Nombrearchivo= tipoLibroCLS.nombreFoto;
                        tipoLibro.Bhabilitado = 1;
                        bd.TipoLibros.Add(tipoLibro);
                        bd.SaveChanges();
                        rpta = 1;

                    }
                    else
                    {
                        TipoLibro tipoLibro = bd.TipoLibros.Where(x => x.Iidtipolibro == tipoLibroCLS.idtipolibro).First();
                        tipoLibro.Nombretipolibro = tipoLibroCLS.nombre;
                        tipoLibro.Descripcion = tipoLibroCLS.descripcion;
                        if (nombreFoto != string.Empty)
                        {
                            tipoLibro.Archivo = tipoLibroCLS.foto;
                            tipoLibro.Nombrearchivo = tipoLibroCLS.nombreFoto;
                        }
                        bd.SaveChanges();
                        rpta = 1;
                    }
                }
                catch (Exception)
                {
                    rpta = 0;
                }
            }

                return rpta;
        }

    }
}
