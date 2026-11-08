using Microsoft.AspNetCore.Mvc;
using semana1.Models;
using System.Diagnostics;

namespace semana1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Busqueda(int id = 0)
        {
            ViewBag.Id = id;
            ViewData["id"] = id; //Map <-> [llave, valor] el casteo es inmediato | Casteo forzoso get, set
            return View();
        }

        [HttpGet]
        public IActionResult Guardar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(string id = "", string name = "")
        {
            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }

        public IActionResult VistaCSS()
        {
            return View();
        }

        public IActionResult VistaCSS2()
        {
            ViewData["Titulo"] = "Pagina principal";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
