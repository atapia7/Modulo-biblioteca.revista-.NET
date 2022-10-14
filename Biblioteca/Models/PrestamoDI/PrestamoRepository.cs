using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Biblioteca.Models;

namespace Biblioteca.Models.PrestamoDI
{
    public class PrestamoRepository : IPrestamo
    {
        string cadena = @"server=.;database = DB_BIBLIOTECA;Trusted_Connection = True; MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";

        public string actualizar(Prestamo reg)
        {

            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE PRESTAMO SET IdEstadoPrestamo = @IdEstadoPrestamo, FechaConfirmacionDevolucion = GETDATE(), EstadoRecibido = @estadoRecibido WHERE IdPrestamo = @idPrestamo", cn);
                    cmd.Parameters.AddWithValue("@idPrestamo", reg.idPrestamo);
                    cmd.Parameters.AddWithValue("@IdEstadoPrestamo", reg.idEstadoPrestamo);
                    cmd.Parameters.AddWithValue("@estadoRecibido", reg.estadoRecibido);
                    int p = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {p} Prestamo(s)";

                }
                catch (Exception e) { mensaje = e.Message; }
                finally { cn.Close(); }


            }
            return mensaje;
        }

        public string Agregar(Prestamo reg)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    /*SqlCommand cmd = new SqlCommand("insert PRESTAMO values(1,@idPersona,@idlibro,@fechaDevolucion,null,'EstadoEntregado',null,'1',GETDATE())", cn);
                    cmd.Parameters.AddWithValue("@idPersona", reg.IdPersona);
                    cmd.Parameters.AddWithValue("@idlibro", reg.Idlibro);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", reg.fechaDevolucion);*/
                    SqlCommand cmd = new SqlCommand("usp_insertaPrestamo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPersona", reg.idPersona);
                    cmd.Parameters.AddWithValue("@IdLibro", reg.idLibro);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", reg.fechaDevolucion);
                    cmd.Parameters.AddWithValue("@EstadoEntregado", reg.estadoEntregado);

                    int p = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha registrado {p} Prestamo(s)";

                }
                catch (Exception e) { mensaje = e.Message; }
                finally { cn.Close(); }


            }
            return mensaje;
        }

        public Prestamo Buscar(int codigo)
        {
            if (string.IsNullOrEmpty(codigo.ToString()))
                return new Prestamo();
            else
                return prestamos().FirstOrDefault(p => p.idPrestamo == codigo);
        }

        public IEnumerable<EstadoPrestamo> estados()
        {
            List<EstadoPrestamo> estadoPrestamos = new List<EstadoPrestamo>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select * from ESTADO_PRESTAMO", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    estadoPrestamos.Add(new EstadoPrestamo()
                    {
                        idEstadoPrestamo = dr.GetInt32(0),
                        desEstadoPrest = dr.GetString(1)


                    });


                }

            }
            return estadoPrestamos;
        }

        public IEnumerable<Libro> libros()
        {
            List<Libro> libro = new List<Libro>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select*from LIBRO", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    libro.Add(new Libro()
                    {
                        idLibro = dr.GetInt32(0),
                        tituloLib = dr.GetString(1)


                    });


                }

            }
            return libro;
        }

        public IEnumerable<Persona> personas()
        {
            List<Persona> persona = new List<Persona>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select*from PERSONA", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    persona.Add(new Persona()
                    {
                        idPersona = dr.GetInt32(0),
                        nombrePer = dr.GetString(1),


                    });


                }

            }
            return persona;
        }

        public IEnumerable<Prestamo> prestamos()
        {
            List<Prestamo> temporal = new List<Prestamo>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select*from PRESTAMO", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Prestamo()
                    {
                        idPrestamo = dr.GetInt32(0),
                        idEstadoPrestamo = dr.GetInt32(1),
                        idPersona = dr.GetInt32(2),
                        idLibro = dr.GetInt32(3),
                        fechaDevolucion = dr.GetDateTime(4),
                        fechaConfirmacion = dr.GetDateTime(5),
                        estadoEntregado = dr.GetString(6),
                        estadoRecibido = dr.GetString(7),
                        estado = dr.GetBoolean(8),
                        fechaCreacion = dr.GetDateTime(9),



                    });


                }

            }
            return temporal;
        }

        public string eliminar(int id)
        {
            string respuesta = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from PRESTAMO where IdPrestamo = @id", cn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = "Se ha eliminado Correctamente";

                }
                catch (Exception ex)
                {
                    respuesta = "Error al eliminar";
                }

            }

            return respuesta;
        }


    }
}
