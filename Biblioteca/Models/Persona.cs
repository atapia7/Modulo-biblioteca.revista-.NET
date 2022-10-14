using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Persona
    {
        [Display(Name = "Codigo")] public int idPersona { get; set; }
        [Display(Name = "Nombres")] public string nombrePer { get; set; }
        [Display(Name = "Apellido")] public string apellidoPer { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string codigo { get; set; }
        [Display(Name = "Persona")] public int IdtipoPersona { get; set; }
        public string persona { get; set; }
        public bool estado { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha Creacion")] public DateTime fechaCreacion { get; set; }

    }
}
