using semana3.Models;

namespace semana3.Repositories;

public interface ILibroRepository
{
    List<Libro> GetAll();
    Libro Save(Libro libro);
    Libro GetById(int id);
}