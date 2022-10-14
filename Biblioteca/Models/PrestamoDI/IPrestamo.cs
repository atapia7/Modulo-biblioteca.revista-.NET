using Microsoft.Data.SqlClient;
using Biblioteca.Models.PrestamoDI;
namespace Biblioteca.Models.PrestamoDI
{
    public interface IPrestamo
    {
        IEnumerable<Libro> libros();
        IEnumerable<Persona> personas();
        IEnumerable<EstadoPrestamo> estados();
        IEnumerable<Prestamo> prestamos();

        Prestamo Buscar(int codigo);
        string Agregar(Prestamo reg);

        string actualizar(Prestamo reg);

        string eliminar(int id);

    }
}
