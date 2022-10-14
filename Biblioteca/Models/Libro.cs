using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Libro
    {
        [Display(Name = "Id LIbro")] public int idLibro { get; set; }
        [Display(Name = "titulo de libro")] public string tituloLib { get; set; }
        [Display(Name = "nombre de la Portada")] public string nombrePortada { get; set; }
        public int idAutor { get; set; }
        public string nomAutor { get; set; }
        public int idCategoria { get; set; }
        public string nomCategoria { get; set; }
        public int idEditorial { get; set; }
        public string nomEditorial { get; set; }
        public string ubicacionLib { get; set; }
        public int ejemplares { get; set; }
        public string estado { get; set; }
    }
}
