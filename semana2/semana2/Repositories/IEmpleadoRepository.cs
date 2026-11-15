using semana2.Models;

namespace semana2.Repositories;

public interface IEmpleadoRepository
{
    IEnumerable<Empleado> recuperarDatos(string storeProcedure, Dictionary<string, dynamic> parametros);

    string EnviarDatos(string storeProcedure, Dictionary<string, dynamic> parametros);
}