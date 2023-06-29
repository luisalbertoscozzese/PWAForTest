using Microsoft.AspNetCore.Mvc;
using PWAForTest.Clases;
using PWAForTest.Models;
using System;

namespace PWAForTest.Controllers
{
    public class LibroController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public LibroController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public LibroCLS recuperarLibro(int iidlibro)
        {
            LibroCLS libroCLS = new LibroCLS();
            using (DbA9acbfDbbibliotecaContext db = new DbA9acbfDbbibliotecaContext())
            {
                Libro libro = db.Libros.Where(p => p.Iidlibro == iidlibro).First();
                libroCLS.iidlibro= libro.Iidlibro;
                libroCLS.titulo = libro.Titulo;
                libroCLS.resumen = libro.Resumen;
                libroCLS.numeropaginas =(int)libro.Numpaginas;
                libroCLS.stock = (int)libro.Stock;
                libroCLS.iidautor = (int)libro.Iidautor;
                libroCLS.base64 = libro.Archivo == null? "" : "data:image/png;base64, " + 
                    Convert.ToBase64String(libro.Archivo);

            }

            return libroCLS;
        }

        public int guardarDatos(LibroCLS libroCLS)
        {
            int rpta = 0;

            string basefoto = libroCLS.base64.Replace("data:image/png;base64,", "");
            byte[] buffer=null;

            if (libroCLS.base64 != "data:,")
            {
                buffer = Convert.FromBase64String(basefoto);
            }

            using (DbA9acbfDbbibliotecaContext db = new DbA9acbfDbbibliotecaContext())
            {
                try
                {
                    if (libroCLS.iidlibro == 0)
                    {
                        Libro libro = new Libro();
                        libro.Titulo = libroCLS.titulo;
                        libro.Resumen= libroCLS.resumen;
                        libro.Numpaginas = libroCLS.numeropaginas;
                        libro.Stock= libroCLS.stock;
                        libro.Iidautor= libroCLS.iidautor;
                        libro.Bhabilitado = 1;
                        if (buffer != null)
                        {
                            libro.Archivo = buffer;
                        }
                        db.Libros.Add(libro);
                        db.SaveChanges();
                        rpta= 1;

                    }
                    else
                    {
                        Libro libro = db.Libros.Where(p => p.Iidlibro == libroCLS.iidlibro).First();
                        libro.Titulo = libroCLS.titulo;
                        libro.Resumen = libroCLS.resumen;
                        libro.Numpaginas = libroCLS.numeropaginas;
                        libro.Stock = libroCLS.stock;
                        libro.Iidautor = libroCLS.iidautor;
                        if (buffer != null)
                        {
                            libro.Archivo = buffer;
                        }
                        db.SaveChanges();
                        rpta = 1;

                    }

                }
                catch (Exception ex)
                {

                    rpta = 0;
                }
            }

                return rpta;
        }

        public List<LibroCLS> listarLibros()
        {
            List<LibroCLS> lista = new List<LibroCLS>();
            string rutaCompleta = Path.Combine(_env.ContentRootPath, "wwwroot/img/NoImgAvailable.png");
            byte[] buffer = System.IO.File.ReadAllBytes(rutaCompleta);
            string base64NoImgAvailable = Convert.ToBase64String(buffer);
            string base64NoImgAvailableFinal = "data:img/png;base64," + base64NoImgAvailable;
            using (DbA9acbfDbbibliotecaContext db = new DbA9acbfDbbibliotecaContext())
            {
                lista= (from libro in db.Libros
                        join autor in db.Autors
                        on libro.Iidautor equals autor.Iidautor
                       where libro.Bhabilitado==1
                       select new LibroCLS
                       {
                           iidlibro=libro.Iidlibro,
                           titulo="<h6>"+libro.Titulo+"</h6><p class='mb-1'>"+ autor.Nombre + " " + 
                           autor.Appaterno + "</p>"+"<p class='text-secondary mb-1'>Stock: "+ libro.Stock + "</p>",
                           numeropaginas=(int)libro.Numpaginas,
                           base64 = libro.Archivo == null ? base64NoImgAvailableFinal
                                 : "data:image/png;base64," + Convert.ToBase64String(libro.Archivo)
                       }).ToList();

            }       
            
            return lista;
        }

    }
}
