using Microsoft.AspNetCore.Mvc;
using semana3.Models;
using semana3.Services;

namespace semana3.Controllers;

[ApiController]
[Route("[controller]")]
public class LibroController : ControllerBase
{
    private readonly ILibroService _libroService;

    public LibroController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    [HttpGet]
    public IEnumerable<Libro> Get()
    {
        return _libroService.GetAll();
    }

    [HttpPost]
    public Libro Post([FromBody] Libro libro)
    {
        return _libroService.Save(libro);
    }

    [HttpGet("{id}")]
    public Libro Get([FromRoute] int id)
    {
        return _libroService.GetById(id);
    }
}