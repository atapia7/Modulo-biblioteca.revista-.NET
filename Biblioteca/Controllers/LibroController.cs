using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //uso del SelectList
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
namespace Biblioteca.Controllers
{
    public class LibroController : Controller
    {
        string cadena = @"server=.;database=DB_BIBLIOTECA;Trusted_Connection = True;" +
            "MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";
        IEnumerable<Estado> estados()
        {
            List<Estado> estados = new List<Estado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from ESTADO_CATEGORIA", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    estados.Add(new Estado()
                    {
                        IdEstado = dr.GetInt32(0),
                        NombreEstado = dr.GetString(1),
                    });
                }
            }
            return estados;
        }
        IEnumerable<Editorial> editoriales()
        {
            List<Editorial> editoriales = new List<Editorial>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from EDITORIAL", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    editoriales.Add(new Editorial()
                    {
                        IdEditorial = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                    });
                }
            }
            return editoriales;
        }
        IEnumerable<Categoria> categorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from CATEGORIA", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    categorias.Add(new Categoria()
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                    });
                }
            }
            return categorias;
        }
        IEnumerable<Autor> autores()
        {
            List<Autor> autores = new List<Autor>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from AUTOR", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    autores.Add(new Autor()
                    {
                        IdAutor = dr.GetInt32(0),
                        Descripcion = dr.GetString(1),
                    });
                }
            }
            return autores;
        }

        IEnumerable<Libro> libros()
        {
            List<Libro> libros = new List<Libro>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("EXEC listalibro", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    libros.Add(new Libro()
                    {
                        idLibro = dr.GetInt32(0),
                        tituloLib = dr.GetString(1),
                        nombrePortada = dr.GetString(2),
                        idAutor = dr.GetInt32(3),
                        nomAutor=dr.GetString(4),
                        idCategoria = dr.GetInt32(5),
                        nomCategoria = dr.GetString(6),
                        idEditorial=dr.GetInt32(7),
                        nomEditorial=dr.GetString(8),
                        ubicacionLib=dr.GetString(9),
                        ejemplares=dr.GetInt32(10),
                        estado=dr.GetString(11),
                    });
                }
            }
            return libros;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => libros()));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.editoriales = new SelectList(await Task.Run(() => editoriales()), "IdEditorial", "Nombre");
            ViewBag.categorias = new SelectList(await Task.Run(() => categorias()), "IdCategoria", "Nombre");
            ViewBag.autores = new SelectList(await Task.Run(() => autores()), "IdAutor", "Descripcion");
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => new Libro()));
        }
        [HttpPost] public async Task<IActionResult> Create(Libro reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_inserta_Libro", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Titulo", reg.tituloLib);
                    cmd.Parameters.AddWithValue("@NombrePortada", reg.nombrePortada);
                    cmd.Parameters.AddWithValue("@IdAutor", reg.idAutor);
                    cmd.Parameters.AddWithValue("@IdCategoria", reg.idCategoria);
                    cmd.Parameters.AddWithValue("@IdEditorial", reg.idEditorial);
                    cmd.Parameters.AddWithValue("@Ubicacion", reg.ubicacionLib);
                    cmd.Parameters.AddWithValue("@Ejemplares", reg.ejemplares);
                    cmd.Parameters.AddWithValue("@Estado", reg.estado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $" se ha insertado {c} libros(e)s ";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;
            ViewBag.editoriales = new SelectList(await Task.Run(() => editoriales()), "IdEditorial", "Nombre");
            ViewBag.categorias = new SelectList(await Task.Run(() => categorias()), "IdCategoria", "Nombre");
            ViewBag.autores = new SelectList(await Task.Run(() => autores()), "IdAutor", "Descripcion");
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => reg));
        }
        Libro Buscar(int codigo)
        {
            return libros().FirstOrDefault(c => c.idLibro == codigo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Libro reg = Buscar(id);
            ViewBag.editoriales = new SelectList(await Task.Run(() => editoriales()), "IdEditorial", "Nombre");
            ViewBag.categorias = new SelectList(await Task.Run(() => categorias()), "IdCategoria", "Nombre");
            ViewBag.autores = new SelectList(await Task.Run(() => autores()), "IdAutor", "Descripcion");
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => reg));
        }
        [HttpPost] public async Task<IActionResult> Edit(Libro reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualiza_libro", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.idLibro);
                    cmd.Parameters.AddWithValue("@Titulo", reg.tituloLib);
                    cmd.Parameters.AddWithValue("@NombrePortada", reg.nombrePortada);
                    cmd.Parameters.AddWithValue("@IdAutor", reg.idAutor);
                    cmd.Parameters.AddWithValue("@IdCategoria", reg.idCategoria);
                    cmd.Parameters.AddWithValue("@IdEditorial", reg.idEditorial);
                    cmd.Parameters.AddWithValue("@Ubicacion", reg.ubicacionLib);
                    cmd.Parameters.AddWithValue("@Ejemplares", reg.ejemplares);
                    cmd.Parameters.AddWithValue("@Estado", reg.estado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {c} libro(s)";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;
            ViewBag.editoriales = new SelectList(await Task.Run(() => editoriales()), "IdEditorial", "Nombre");
            ViewBag.categorias = new SelectList(await Task.Run(() => categorias()), "IdCategoria", "Nombre");
            ViewBag.autores = new SelectList(await Task.Run(() => autores()), "IdAutor", "Descripcion");
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => reg));
        }


    }
}
