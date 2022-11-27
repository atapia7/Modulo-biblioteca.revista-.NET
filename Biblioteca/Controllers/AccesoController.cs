using Microsoft.AspNetCore.Session;
using Microsoft.Data.SqlClient;
using System.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class AccesoController : Controller
    {
        string sesion = ""; //sera el key del Session
        int valida(string login, string clave) //recupero el valor de @sw
        {
            int sw = 0;
            using (SqlConnection cn = new SqlConnection(Conexion.cadena))
            {
                cn.Open();
               SqlCommand cmd = new SqlCommand("usp_verifica_acceso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@clave", clave);
                cmd.Parameters.Add("@fullname", SqlDbType.VarChar, 40).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@sw", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery(); //ejecuto
                HttpContext.Session.SetString(sesion, cmd.Parameters["@fullname"].ToString());
                sw =Convert.ToInt32(cmd.Parameters["@sw"].Value.ToString());
                cn.Close();
            }
            return sw;
        }
        public async Task<IActionResult> Logueo()
        {
            //envio un nuevo Usuario y la llave sesion se limpia
            HttpContext.Session.SetString(sesion, "");
            return View(await Task.Run(() => new Usuario()));

        }
        [HttpPost] public async Task<IActionResult> Logueo(Usuario reg)
        {
           //validar el contenido de los input
           if (!ModelState.IsValid) return View(await Task.Run(() => reg));
           //si los datos se ingresaron en los input correctamente
           //ejecutar el metodo y el Session ya tiene valor
           int sw = valida(reg.login, reg.clave);
            if (sw == 0)
            {
                ModelState.AddModelError("", HttpContext.Session.GetString(sesion));
                return View(await Task.Run(() => reg));
            }
            else

                return RedirectToAction("Portal","Ecommerce");
        }
        public IActionResult Plataforma()

        {

            return View();

        }
    }
}
