using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //uso del SelectList
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
namespace Biblioteca.Controllers
{
    public class CategoriaController : Controller
    {
        IEnumerable<Estado> estados()
        {
            List<Estado> estados = new List<Estado>();
            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
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
        IEnumerable<Categoria> categorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            {
                SqlCommand cmd = new SqlCommand("Select a.IdCategoria,a.Nombre,e.IdEstado,e.NombreEstado from CATEGORIA a join ESTADO_CATEGORIA e on a.IdEstado=e.IdEstado ", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    categorias.Add(new Categoria()
                    {
                        IdCategoria = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        IdEstado = dr.GetInt32(2),
                        Estado = dr.GetString(3),
                    });
                }  }
            return categorias;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => categorias()));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => new Categoria()));
        }
        [HttpPost] public async Task<IActionResult> Create(Categoria reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_inserta_Categoria", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@IdEstado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $" se ha insertado {c} categoria(e)s ";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;

                }
                finally { cn.Close(); }

            }
            ViewBag.Alert = mensaje;
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado", reg.IdEstado);
            return View(await Task.Run(() => reg));
        }
        Categoria Buscar(int codigo)
        {
            return categorias().FirstOrDefault(c => c.IdCategoria == codigo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Categoria reg = Buscar(id);
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado", reg.IdEstado);

            return View(await Task.Run(() => reg));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Categoria reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualiza_categoria", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.IdCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", reg.Nombre);
                    cmd.Parameters.AddWithValue("@IdEstado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado la {c} categoria(s)";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado", reg.IdEstado);
            return View(await Task.Run(() => reg));
        }

    }
}
