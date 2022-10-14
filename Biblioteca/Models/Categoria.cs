using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Categoria
    {
       public int IdCategoria { get; set; }
       public string  Nombre   {get;set;}
        public int IdEstado { get; set; }
        public string  Estado {get;set;}
    }
}
