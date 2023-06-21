using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;

namespace PWAForTest.Controllers
{
    public class TipoLibroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<TipoLibroCLS> listarTipoLibro(string nombreTipoLibrobusqueda) 
        { 
            List<TipoLibroCLS> lista = new List<TipoLibroCLS>();
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
                                 descripcion = tipoLibro.Descripcion
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
                                 descripcion = tipoLibro.Descripcion
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

                return tipoLibroCLS;
            }
        }

        public int guardarTipoLibro(TipoLibroCLS tipoLibroCLS) 
        {
            int rpta = 0;

            using (DbA9acbfDbbibliotecaContext bd = new DbA9acbfDbbibliotecaContext()) 
            {
                try
                {
                    if (tipoLibroCLS.idtipolibro == 0)
                    {
                        TipoLibro tipoLibro = new TipoLibro();
                        tipoLibro.Nombretipolibro = tipoLibroCLS.nombre;
                        tipoLibro.Descripcion = tipoLibroCLS.descripcion;
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
