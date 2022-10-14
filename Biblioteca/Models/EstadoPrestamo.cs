using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class EstadoPrestamo
    {
       public int idEstadoPrestamo { get; set; }
        public string desEstadoPrest { get; set; }
        public int estadoPrest { get; set; }
        public DateTime fechaCreacion{get;set;}
    }
}
