using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class TipoPersona
    {
        [Display(Name = "Fecha Creacion")] public int IdtipoPersona { get; set; }
        [Display(Name = "Fecha Creacion")] public string descripcion { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha Creacion")] public DateTime fechaCreacion { get; set; }
    }
}
