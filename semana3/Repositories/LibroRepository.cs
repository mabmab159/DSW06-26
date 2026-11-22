using Microsoft.Data.SqlClient;
using semana3.Models;

namespace semana3.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly IConfiguration _configuration;
    
    public LibroRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public List<Libro> GetAll()
    {
        List<Libro> libros = new List<Libro>();
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:BD"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM libro", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                libros.Add(new Libro()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Nombre = sqlDataReader.GetString(1),
                    Autor = sqlDataReader.GetString(2),
                    Editorial = sqlDataReader.GetString(3)
                });
            }
        }
        return libros;
    }

    public Libro Save(Libro libro)
    {
        throw new NotImplementedException();
    }

    public Libro GetById(int id)
    {
        throw new NotImplementedException();
    }
}