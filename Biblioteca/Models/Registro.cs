namespace Biblioteca.Models
{
    public class Registro
    {
        public int idRevista { get; set; }
        public string NombreRevista { get; set; }
        public decimal PrecioUnidad { get; set; }
        public int stock { get; set; }
        public decimal monto { get { return PrecioUnidad * stock; } }
    }
}
