using semana3.Models;

namespace semana3.Services;

public interface ILibroService
{
    List<Libro> GetAll();
    Libro Save(Libro libro);
    Libro GetById(int id);
}