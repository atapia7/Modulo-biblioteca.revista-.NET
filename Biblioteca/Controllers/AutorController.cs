using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //uso del SelectList
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
namespace Biblioteca.Controllers
{
    public class AutorController : Controller
    {
        string cadena = @"server=.;database=DB_BIBLIOTECA;Trusted_Connection = True;" +
            "MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";

        IEnumerable<Estado> estados()
        {
            List<Estado> estados = new List<Estado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from ESTADO_AUTOR", cn);
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
        IEnumerable<Autor> autores()
        {
            List<Autor> autores = new List<Autor>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select a.IdAutor,a.Descripcion,e.IdEstado,e.NombreEstado from AUTOR a join ESTADO_AUTOR e on a.IdEstado=e.IdEstado ", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    autores.Add(new Autor()
                    {
                        IdAutor = dr.GetInt32(0),
                        Descripcion = dr.GetString(1),
                        IdEstado = dr.GetInt32(2),
                        NomEstado=dr.GetString(3),
                    });
                }
            }
            return autores;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(()=>autores()));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado");
            return View(await Task.Run(()=> new Autor()));  
        }
        [HttpPost] public async Task<IActionResult> Create(Autor reg)
        {
            string mensaje = "";

            using(SqlConnection cn=new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_inserta_autor", cn);
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                    cmd.Parameters.AddWithValue("@IdEstado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $" se ha insertado {c} de nombre {reg.Descripcion} autor(e)s ";                   
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;            
ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado",reg.IdEstado);
            return View(await Task.Run(() => reg));
           
            
        }
              
       Autor Buscar(int codigo)
        {
            return autores().FirstOrDefault(c => c.IdAutor == codigo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Autor reg = Buscar(id);

            ViewBag.estados = new SelectList(await Task.Run(() => estados()), "IdEstado", "NombreEstado", reg.IdEstado);
            
            return View(await Task.Run(() => reg));
        }
        [HttpPost] public async Task<IActionResult> Edit(Autor reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualiza_Autor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.IdAutor);
                    cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                    cmd.Parameters.AddWithValue("@IdEstado", reg.IdEstado);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado el {c} autor(s)";
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
