using semana3.Models;
using semana3.Repositories;

namespace semana3.Services;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;

    public LibroService(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }
    
    public List<Libro> GetAll()
    {
        return _libroRepository.GetAll();
    }

    public Libro Save(Libro libro)
    {
        return _libroRepository.Save(libro);
    }

    public Libro GetById(int id)
    {
        return _libroRepository.GetById(id);
    }
}