using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; //uso del SelectList
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
namespace Biblioteca.Controllers
{
    public class PersonaController : Controller
    {
        string cadena = @"server=.;database=DB_BIBLIOTECA;Trusted_Connection = True;" +
            "MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";
        IEnumerable<Persona> personas()
        {

            List<Persona> personas = new List<Persona>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("usp_listar_Personas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    personas.Add(new Persona()
                    {
                        idPersona = dr.GetInt32(0),
                        nombrePer = dr.GetString(1),
                        apellidoPer = dr.GetString(2),
                        correo = dr.GetString(3),
                        clave = dr.GetString(4),
                        persona = dr.GetString(5),
                        estado = dr.GetBoolean(6),
                        fechaCreacion = dr.GetDateTime(7),


                    });
                }
            }
            return personas;


        }

        IEnumerable<TipoPersona> tipoPersonas()
        {
            List<TipoPersona> tipoPersonas = new List<TipoPersona>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Select * from TIPO_PERSONA", cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    tipoPersonas.Add(new TipoPersona()
                    {
                        IdtipoPersona = dr.GetInt32(0),
                        descripcion = dr.GetString(1)
                    }); ;
                }
            }
            return tipoPersonas;
        }
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => personas()));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.tipoPersonas = new SelectList(await Task.Run(() => tipoPersonas()), "IdtipoPersona", "descripcion");

            return View(await Task.Run(() => new Persona()));
        }
        [HttpPost]
        public async Task<IActionResult> Create(Persona reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_InsertaPersona_adm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombres", reg.nombrePer);
                    cmd.Parameters.AddWithValue("@Apellidos", reg.apellidoPer);
                    cmd.Parameters.AddWithValue("@correo", reg.correo);
                    cmd.Parameters.AddWithValue("@clave", reg.clave);
                    cmd.Parameters.AddWithValue("@idTipoPersona", reg.IdtipoPersona);
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

            ViewBag.tipoPersonas = new SelectList(await Task.Run(() => tipoPersonas()), "IdtipoPersona", "descripcion");
            return View(await Task.Run(() => reg));
        }

        Persona Buscar(int codigo)
        {

            return personas().FirstOrDefault(c => c.idPersona == codigo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Persona reg = Buscar(id);
            ViewBag.tipoPersonas = new SelectList(await Task.Run(() => tipoPersonas()), "IdtipoPersona", "descripcion");

            return View(await Task.Run(() => reg));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Persona reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualiza_persona", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", reg.codigo);
                    cmd.Parameters.AddWithValue("@Nombres", reg.nombrePer);
                    cmd.Parameters.AddWithValue("@Apellidos", reg.apellidoPer);
                    cmd.Parameters.AddWithValue("@correo", reg.correo);
                    cmd.Parameters.AddWithValue("@clave", reg.clave);
                    cmd.Parameters.AddWithValue("@idTipoPersona", reg.IdtipoPersona);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {c} libro(s)";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;
            ViewBag.tipoPersonas = new SelectList(await Task.Run(() => tipoPersonas()), "IdtipoPersona", "descripcion");

            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("Delete PERSONA where IdPersona = @id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha eliminado {c} libro(s)";
                }
                catch (Exception ex) { mensaje = ex.Message; }
                finally { cn.Close(); }
            }
            ViewBag.Alert = mensaje;

            return View(await Task.Run(() => personas()));
        }
    }
}