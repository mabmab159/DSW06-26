using semana2.Models;

namespace semana2.Services;

public interface IEmpleadoServices
{
    IEnumerable<Empleado> recuperarEmpleados();
    
    string guardarEmpleado(Empleado empleado);
}