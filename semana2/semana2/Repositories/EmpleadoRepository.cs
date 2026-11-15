using System.Data;
using Microsoft.Data.SqlClient;
using semana2.Models;

namespace semana2.Repositories;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly IConfiguration _configuration;

    public EmpleadoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Empleado> recuperarDatos(string storeProcedure, Dictionary<string, dynamic> parametros)
    {
        List<Empleado> listEmpleados = new List<Empleado>();
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:BD"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(storeProcedure, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //En caso existiesen parametros, pasarlos al store
            foreach (KeyValuePair<string, dynamic> param in parametros)
            {
                sqlCommand.Parameters.AddWithValue(param.Key, param.Value);
            }

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listEmpleados.Add(new Empleado()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Nombre = sqlDataReader.GetString(1),
                    ApellidoPaterno = sqlDataReader.GetString(2),
                    ApellidoMaterno = sqlDataReader.GetString(3),
                    Direccion = sqlDataReader.GetString(4),
                    Telefono = sqlDataReader.GetString(5),
                });
            }
        }

        return listEmpleados;
    }

    public string EnviarDatos(string storeProcedure, Dictionary<string, dynamic> parametros)
    {
        using (SqlConnection sqlConnection = new SqlConnection(_configuration["ConnectionStrings:BD"]))
        {
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(storeProcedure, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            //En caso existiesen parametros, pasarlos al store
            foreach (KeyValuePair<string, dynamic> param in parametros)
            {
                sqlCommand.Parameters.AddWithValue(param.Key, param.Value);
            }

            return sqlCommand.ExecuteNonQuery() > 0 ? "Exitoso" : "Fallo";
        }
    }
}