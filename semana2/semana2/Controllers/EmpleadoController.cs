using Microsoft.AspNetCore.Mvc;
using semana2.Models;
using semana2.Services;

namespace semana2.Controllers;

public class EmpleadoController : Controller
{
    private readonly IEmpleadoServices iEmpleadoServices;

    public EmpleadoController(IEmpleadoServices iEmpleadoServices)
    {
        this.iEmpleadoServices = iEmpleadoServices;
    }
    
    public IActionResult Index()
    {
        IEnumerable<Empleado> listEmpleados = iEmpleadoServices.recuperarEmpleados();
        return View(listEmpleados);
    }
    
    //Details -> Para los alumnos

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Empleado empleado)
    {
        @ViewBag.Mensaje = iEmpleadoServices.guardarEmpleado(empleado);
        return View(empleado);
    }
    
    //Update -> Para los alumnos
    
    //Delete -> Para los alumnos
}