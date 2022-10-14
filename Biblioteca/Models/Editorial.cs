using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Editorial
    { 
        public int IdEditorial { get;set;}
        public string Nombre { get;set;}
        [DataType(DataType.Date), Display(Name = "Fecha ")] public DateTime fecha { get; set; }
        public int IdEstado { get;set;}
        public string Estado { get; set; }
        
    }
}
