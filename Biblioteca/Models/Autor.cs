using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
 {
    public class Autor
    {
     public int IdAutor { get; set; }
     public string Descripcion  { get; set; }
     public int IdEstado { get; set; }
     public string NomEstado { get; set; }
        
    }
}
