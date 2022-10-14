using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Prestamo
    {
        [Display(Name = "Id Prestamo")] public int idPrestamo { get; set; }
        [Display(Name = "Id Estado Prestamo")] public int idEstadoPrestamo { get; set; }
        public string EstadoPrestamo { get; set; }
        [Display(Name = "Id Persona")] public int idPersona { get; set; }
         public string NPersona { get; set; }
        [Display(Name = "Id Libro")] public int idLibro { get; set; }
        public string NLibro { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha Devolucion")] public DateTime fechaDevolucion { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha Confirmacion")] public DateTime fechaConfirmacion { get; set; }
        [Display(Name = "Estado entregado")] public string estadoEntregado { get; set; }
        [Display(Name = "Estado recibido")] public string estadoRecibido { get; set; }
        public bool estado { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha Creacion")] public DateTime fechaCreacion { get; set; }
    }
}
