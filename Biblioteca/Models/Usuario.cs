using System.ComponentModel.DataAnnotations;
namespace Biblioteca.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }

      [Required, StringLength(20, MinimumLength = 3)] public string login { get; set; }

       [Required, MaxLength(10)] public string clave { get; set; }

        public int intentos { get; set; }

        public DateTime fechaBloqueo { get; set; }
    }
}
