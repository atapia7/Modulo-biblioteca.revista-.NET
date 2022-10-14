using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Session;
using System.Data;
using Newtonsoft.Json;
using Biblioteca.Models;


namespace Biblioteca.Controllers
{
    public class ECommerceController : Controller
    {
        string cadena = @"server=.;database = DB_BIBLIOTECA;Trusted_Connection = True; MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";

        IEnumerable<Revista> listado()
        {
            List<Revista> revista = new List<Revista>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec usp_Revistas", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    revista.Add(new Revista()
                    {
                        idRevista = dr.GetInt32(0),
                        NombreRevista = dr.GetString(1),
                        PrecioUnidad = dr.GetDecimal(2),
                        stock = dr.GetInt16(3)



                    });


                }

            }
            return revista;

        }
        Revista Buscar(int codigo = 0)
        {
            if (codigo <= 0)
                return null;
            else
                return listado().FirstOrDefault(p => p.idRevista == codigo);



        }

        public IActionResult Portal()
        {
            if (HttpContext.Session.GetString("canasta") == null)
            {
                HttpContext.Session.SetString("canasta",
                    JsonConvert.SerializeObject(new List<Registro>()));
            }
            return View(listado());
        }
        public IActionResult Seleccionar(int id = 0)
        {

            Revista reg = Buscar(id);
            if (reg == null) { return RedirectToAction("Portal"); }

            return View(reg);
        }
        [HttpPost]
        public IActionResult Seleccionar(int codigo, int cantidad)
        {
            List<Registro> auxiliar = JsonConvert.DeserializeObject<List<Registro>>(
                 HttpContext.Session.GetString("canasta"));

            Registro It = auxiliar.FirstOrDefault(x => x.idRevista == codigo);
            if (It != null)
            {
                ViewBag.mensaje = "La revista se ha registrado";
                return View(Buscar(codigo));

            }

            Revista reg = Buscar(codigo);

            It = new Registro()
            {
                idRevista = reg.idRevista,
                NombreRevista = reg.NombreRevista,
                PrecioUnidad = reg.PrecioUnidad,
                stock = cantidad
            };



            auxiliar.Add(It);

            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(auxiliar));
            ViewBag.mensaje = "Revista Registrado";

            return View(reg);


        }

        public IActionResult Canasta()
        {

            List<Registro> auxiliar =
               JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));

            return View(auxiliar);
        }

        public IActionResult Delete(int id)
        {
            List<Registro> auxiliar =
                JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));

            Registro reg = auxiliar.FirstOrDefault(x => x.idRevista == id);

            auxiliar.Remove(reg);

            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(auxiliar));
            return RedirectToAction("Canasta");
        }
        public IActionResult Pedido()
        {
            List<Registro> auxiliar =
                JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));
            return View(auxiliar);

        }
        [HttpPost]
        public IActionResult Pedido(string dni, string nombre)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                List<Registro> auxiliar =JsonConvert.DeserializeObject<List<Registro>>(HttpContext.Session.GetString("canasta"));
                cn.Open();
                SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);
                try
                {

                    SqlCommand cmd = new SqlCommand("usp_pedido_agrega", cn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idpedido", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@monto", auxiliar.Sum(x => x.monto));
                    cmd.ExecuteNonQuery();

                    string npedido = cmd.Parameters["@idpedido"].Value.ToString();
                    auxiliar.ForEach(x => {
                        cmd = new SqlCommand("exec usp_detapedido_agregar @idpedido,@idrevista,@precio,@cantidad", cn, tr);
                        cmd.Parameters.AddWithValue("@idpedido", npedido);
                        cmd.Parameters.AddWithValue("@idrevista", x.idRevista);
                        cmd.Parameters.AddWithValue("@precio", x.PrecioUnidad);
                        cmd.Parameters.AddWithValue("@cantidad", x.stock);
                        cmd.ExecuteNonQuery();

                    });
                    auxiliar.ForEach(x => {
                        cmd = new SqlCommand("exec usp_revistas_ActualizarStock @codigo,@cantidad", cn, tr);
                        cmd.Parameters.AddWithValue("@codigo", x.idRevista);
                        cmd.Parameters.AddWithValue("@cantidad", x.stock);

                        cmd.ExecuteNonQuery();

                    });





                    tr.Commit();
                    mensaje = $"Se ha registrado el pedido de numero {npedido}";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    tr.Rollback();

                }
                finally
                {
                    cn.Close();
                }


                return RedirectToAction("ventana", new { msg = mensaje });
            }
        }
        public IActionResult ventana(string msg)
        {
            ViewBag.mensaje = msg;
            return View();
        }
    }
}
