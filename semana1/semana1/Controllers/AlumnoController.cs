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
    
    IEnumerable<Alumno> recuperarAlumnosFiltrados(string nombre)
    {
        List<Alumno> listAlumnos = new List<Alumno>();
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("select * from Alumno where Nombre=@nombre", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@nombre", nombre);
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

    string ActualizarAlumno(Alumno alumno)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("usp_ActualizarAlumno", sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", alumno.Id);
            sqlCommand.Parameters.AddWithValue("@Nombre", alumno.Nombre);
            sqlCommand.Parameters.AddWithValue("@Apellido", alumno.Apellido);
            sqlCommand.Parameters.AddWithValue("@Correo", alumno.Correo);
            int cantidadFilas = sqlCommand.ExecuteNonQuery(); //No me devuelve ningun valor como vista
            return cantidadFilas > 0 ? "Exitoso" : "Fallo";
        }
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Alumno> alumnos = recuperarAlumnos();
        return View(alumnos);
    }

    public async Task<IActionResult> Paginar(int pagina = 0)
    {
        IEnumerable<Alumno> alumnos = recuperarAlumnos();
        //Manejar paginacion en memoria
        //Cantidad de items por fila
        int cantidadItems = 5;
        //Cantidad dde elementos total
        int cantidadElementos = alumnos.Count(); //11
        //Calcular la cantidad de paginas total
        int cantidadPaginas = cantidadElementos % cantidadItems == 0
            ? cantidadElementos / cantidadItems
            : cantidadElementos / cantidadItems + 1;
        ViewBag.paginaActual = pagina;
        ViewBag.cantidadPaginas = cantidadPaginas;
        //Manejo de datos en caché
        return View(await Task.Run(() => alumnos.Skip(cantidadItems * pagina).Take(cantidadItems)));
    }

    public async Task<IActionResult> Filtrar(int pagina = 0, string nombre = "")
    {
        //A. Recuperar todos los elementos y filtrar luego
        //IEnumerable<Alumno> alumnos = recuperarAlumnos().ToList().Where(a => a.Nombre.Contains(nombre));
        //B. Recuperar los elementos filtrados a traves de un query
        if (string.IsNullOrEmpty(nombre))
        {
            return View(recuperarAlumnos());
        }
        IEnumerable<Alumno> alumnos = recuperarAlumnosFiltrados(nombre);
        //Manejar paginacion en memoria
        //Cantidad de items por fila
        int cantidadItems = 5;
        //Cantidad dde elementos total
        int cantidadElementos = alumnos.Count(); //11
        //Calcular la cantidad de paginas total
        int cantidadPaginas = cantidadElementos % cantidadItems == 0
            ? cantidadElementos / cantidadItems
            : cantidadElementos / cantidadItems + 1;
        ViewBag.paginaActual = pagina;
        ViewBag.cantidadPaginas = cantidadPaginas;
        ViewBag.nombre = nombre;
        //Manejo de datos en caché
        return View(await Task.Run(() => alumnos.Skip(cantidadItems * pagina).Take(cantidadItems)));
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

    //Actualizar - Update

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Alumno alumno = recuperaAlumno(id);
        return View(alumno);
    }

    [HttpPost]
    public IActionResult Edit(Alumno alumno)
    {
        ViewBag.Mensaje = ActualizarAlumno(alumno);
        return View(alumno);
    }

    //Eliminar - Delete : Hasta=13:00
    // Reto rapido - Completar el delete utilizando store procedure
    // 1. Recuperar o valida que exista un elemento con el id
    // 2. Ejecutar su query (Store procedure)
    // 3. Redirigir a la vista Index
}