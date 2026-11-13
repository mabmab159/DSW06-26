using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using semana1.Models;

namespace semana1.Controllers;

public class AlumnoController : Controller
{
    private readonly IConfiguration _configuration;

    public AlumnoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    IEnumerable<Alumno> recuperarAlumnos()
    {
        List<Alumno> listAlumnos = new List<Alumno>();
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from Alumno", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listAlumnos.Add(new Alumno()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Nombre = sqlDataReader.GetString(1),
                    Apellido = sqlDataReader.GetString(2),
                    Correo = sqlDataReader.GetString(3)
                });
            }

            sqlDataReader.Close();
            sqlConnection.Close(); //Esta linea se va a ejecutar de manera implicita
        }

        return listAlumnos;
    }

    Alumno recuperaAlumno(int idAlumno)
    {
        Alumno alumno = new Alumno();
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from Alumno where Id = @id", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", idAlumno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                alumno = new Alumno()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Nombre = sqlDataReader.GetString(1),
                    Apellido = sqlDataReader.GetString(2),
                    Correo = sqlDataReader.GetString(3)
                };
            }
            sqlDataReader.Close();
        }
        return alumno;
    }
    
    string saveAlumno(Alumno alumno)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            sqlConnection.Open();
            using (SqlCommand sqlCommand =
                   new SqlCommand("insert into Alumno values(@nombre, @apellido, @correo)", sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@nombre", alumno.Nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", alumno.Apellido);
                sqlCommand.Parameters.AddWithValue("@correo", alumno.Correo);
                int cantidadFilas =
                    sqlCommand.ExecuteNonQuery(); //Este metodo devuelve la cantidad de filas modificadas
                return cantidadFilas > 0 ? "Exitoso" : "Fallo";
            }
        }
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Alumno> alumnos = recuperarAlumnos();
        return View(alumnos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Alumno alumno)
    {
        ViewBag.Mensaje = saveAlumno(alumno);
        return View(alumno);
    }

    public IActionResult Details(int id = 0)
    {
        Alumno alumno = recuperaAlumno(id);
        return View(alumno);
    }
}