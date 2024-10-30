using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SIRS.Controllers
{
    public class EdificioController : Controller
    {
        // GET: EdificioController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }

        public IActionResult GetEdificios(string nombre, string direccion)
        {
            // Aquí iría la consulta a la base de datos, ejemplo:
            var edificios = ObtenerTodosLosEdificios(); // Método ficticio para obtener datos

            // Filtrar según el nombre y dirección, si existen
            if (!string.IsNullOrEmpty(nombre))
            {
                edificios = edificios.Where(e => e.Nombre.Contains(nombre)).ToList();
            }
            if (!string.IsNullOrEmpty(direccion))
            {
                edificios = edificios.Where(e => e.Direccion.Contains(direccion)).ToList();
            }

            // Devolver los datos en formato JSON
            return Json(edificios);
        }

        private List<Edificio> ObtenerTodosLosEdificios()
        {
            // Datos simulados, reemplaza esto con tu lógica para obtener datos de la BD
            return new List<Edificio>
        {
            new Edificio { Nombre = "Edificio Central", Direccion = "Calle Falsa 123", Latitud = "40.7128", Longitud = "-74.0060" },
            new Edificio { Nombre = "Edificio Norte", Direccion = "Avenida Siempre Viva", Latitud = "34.0522", Longitud = "-118.2437" }
        };
        }
    }
     
}
// Clase de ejemplo para los edificios
public class Edificio
{
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Latitud { get; set; }
    public string Longitud { get; set; }
}
