using Microsoft.AspNetCore.Mvc.Rendering; //uso del SelectList
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
namespace Biblioteca.Controllers
{
    public class EditorialController : Controller
    {
        string cadena = @"server=.;database=DB_BIBLIOTECA;Trusted_Connection = True;" +
            "MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";
        IEnumerable<Estado> estados()
        {
            List<Estado> estados = new List<Estado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from ESTADO_EDITORIAL", cn);
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
        IEnumerable<Editorial> editoriales() {
            List<Editorial> editoriales = new List<Editorial>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
             SqlCommand cmd = new SqlCommand("Select a.IdEditorial,a.Nombre,a.fecha,a.IdEstado,e.NombreEstado from EDITORIAL a join ESTADO_EDITORIAL e on a.IdEstado=e.IdEstado", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    editoriales.Add(new Editorial()
                    {
                        IdEditorial = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        fecha=dr.GetDateTime(2),
                        IdEstado = dr.GetInt32(3),
                        Estado = dr.GetString(4),
                    });
                }
            }
            return editoriales;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => editoriales()));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(() => new Editorial()));
        }
        [HttpPost] public async Task<IActionResult> Create(Editorial reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_inserta_Editorial", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@fecha", reg.fecha);
                    cmd.Parameters.AddWithValue("@estado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $" se ha insertado {c} editorial(e)s ";
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
        Editorial Buscar(int codigo)
        {
            return editoriales().FirstOrDefault(c => c.IdEditorial == codigo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Editorial reg = Buscar(id);
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado", reg.IdEstado);

            return View(await Task.Run(() => reg));
        }
        [HttpPost] public async Task<IActionResult> Edit(Editorial reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualiza_Editorial", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.IdEditorial);
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    cmd.Parameters.AddWithValue("@fecha", reg.fecha);
                    cmd.Parameters.AddWithValue("@estado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado la {c} editorial(s)";
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
