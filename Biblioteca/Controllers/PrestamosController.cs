using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Biblioteca.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models.PrestamoDI;

namespace Biblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        string cadena = @"server=.;database=DB_BIBLIOTECA;Trusted_Connection = True;" +
            "MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";
        IEnumerable<EstadoPrestamo> estados_prestamos()
        {
            List<EstadoPrestamo> estados_prestamos = new List<EstadoPrestamo>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select IdEstadoPrestamo,Descripcion from ESTADO_PRESTAMO", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    estados_prestamos.Add(new EstadoPrestamo()
                    {
                        idEstadoPrestamo = dr.GetInt32(0),
                        desEstadoPrest = dr.GetString(1),
                    });
                }
            }
            return estados_prestamos;
        }
        IEnumerable<Libro> libros()
        {
            List<Libro> libros = new List<Libro>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select IdLibro,Titulo from LIBRO", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    libros.Add(new Libro()
                    {idLibro = dr.GetInt32(0),
                         tituloLib= dr.GetString(1),
                    });
                }
            }
            return libros;
        }
        IEnumerable<Persona> personas()
        {
            List<Persona> personas = new List<Persona>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select IdPersona,Nombre from PERSONA", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    personas.Add(new Persona()
                    {
                        idPersona = dr.GetInt32(0),
                        nombrePer = dr.GetString(1),
                    });
                }
            }
            return personas;
        }

        IEnumerable<Prestamo> prestamos() {
            List<Prestamo> prestamos = new List<Prestamo>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec usp_listaPrestamo", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    prestamos.Add(new Prestamo()
                    {
                        idPrestamo = dr.GetInt32(0),
                        idEstadoPrestamo = dr.GetInt32(1),
                        EstadoPrestamo=dr.GetString(2),
                        idPersona=dr.GetInt32(3),
                        NPersona=dr.GetString(4),
                        idLibro=dr.GetInt32(5),
                        NLibro=dr.GetString(6),
                        fechaDevolucion=dr.GetDateTime(7),
                        fechaConfirmacion=dr.GetDateTime(8),
                        estadoEntregado = dr.GetString(9),
                        estadoRecibido = dr.GetString(10),
                        estado = dr.GetBoolean(11),
                        fechaCreacion = dr.GetDateTime(12),
                    });
                }
            }
            return prestamos;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => prestamos()));
        }
        //public async Task<IActionResult> Create()
        //{
        //    ViewBag.estados_prestamos = new SelectList(await Task.Run(() => estados_prestamos()), /"IdEstadoPrestamo", /"Descripcion"); 
        //    ViewBag.libros = new SelectList(await Task.Run(() => libros()), "IIdLibro", "Titulo"); 
        //    ViewBag.personas = new SelectList(await Task.Run(() => personas()), "IdPersona", "Nombre");          
        //    return View(await Task.Run(() => new Libro()));
        //}
        IPrestamo injector;
        public PrestamosController()
        {
            injector = new PrestamoRepository();

        }
     
        public async Task<IActionResult> Details(int codigo)
        {
            return View(await Task.Run(() => injector.Buscar(codigo)));


        }

        public async Task<IActionResult> Edit(int codigo)
        {
            Prestamo reg = injector.Buscar(codigo);

            ViewBag.estados = new SelectList(injector.estados(), "idEstadoPrestamo", "desEstadoPrest", reg.idPrestamo);
            ViewBag.libros = new SelectList(await Task.Run(() => libros()), "idLibro", "tituloLib", reg.idLibro);
            return View(await Task.Run(() => reg));

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Prestamo reg)
        {
            ViewBag.mensaje = injector.actualizar(reg);
            ViewBag.estados = new SelectList(injector.estados(), "idEstadoPrestamo", "desEstadoPrest", reg.idPrestamo);
            return View(await Task.Run(() => reg));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.personas = new SelectList(injector.personas(), "idPersona", "nombrePer");

            ViewBag.libros = new SelectList(injector.libros(), "idLibro", "tituloLib");

            return View(await Task.Run(() => new Prestamo()));

        }
        [HttpPost]
        public async Task<IActionResult> Create(Prestamo reg)
        {

            ViewBag.mensaje = injector.Agregar(reg);
            ViewBag.personas = new SelectList(injector.personas(), "idPersona", "nombrePer", reg.idPersona);

            ViewBag.libros = new SelectList(injector.libros(), "idLibro", "tituloLib", reg.idLibro);
            return View(await Task.Run(() => reg));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Prestamo reg = injector.Buscar(id);
            ViewBag.mensaje = injector.eliminar(id);
            
            return View(await Task.Run(() => injector.eliminar(reg.idPrestamo)));
        }

    }
}
