using semana2.Models;
using semana2.Repositories;

namespace semana2.Services;

public class EmpleadoServices : IEmpleadoServices
{
    private readonly IEmpleadoRepository _iEmpleadoRepository;

    public EmpleadoServices(IEmpleadoRepository iEmpleadoRepository)
    {
        _iEmpleadoRepository = iEmpleadoRepository;
    }

    public IEnumerable<Empleado> recuperarEmpleados()
    {
        return _iEmpleadoRepository.recuperarDatos("usp_ObtenerEmpleados",
            new Dictionary<string, dynamic>());
    }

    public string guardarEmpleado(Empleado empleado)
    {
        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add("@Nombre", empleado.Nombre);
        parametros.Add("@ApellidoPaterno", empleado.ApellidoPaterno);
        parametros.Add("@ApellidoMaterno", empleado.ApellidoMaterno);
        parametros.Add("@Direccion", empleado.Direccion);
        parametros.Add("@Telefono", empleado.Telefono);
        return _iEmpleadoRepository.EnviarDatos("usp_GuardarEmpleado", parametros);
    }
}