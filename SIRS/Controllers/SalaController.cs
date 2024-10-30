using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIRS.Data.Repositorio;

namespace SIRS.Controllers
{
    public class SalaController : Controller
    {
        // GET: SalaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalaController/Add
        public ActionResult Add()
        {
            return View();
        }
        public IActionResult GetSalas(string edificio, string nombreSala, int? capacidad)
        {
            // Aquí iría la consulta a la base de datos, ejemplo:
            var salas = ObtenerTodasLasSalas(); // Método ficticio para obtener datos

            // Filtrar según el edificio, nombre y capacidad
            if (!string.IsNullOrEmpty(edificio))
            {
                salas = salas.Where(s => s.Edificio == edificio).ToList();
            }
            if (!string.IsNullOrEmpty(nombreSala))
            {
                salas = salas.Where(s => s.NombreSala.Contains(nombreSala)).ToList();
            }
            if (capacidad.HasValue)
            {
                salas = salas.Where(s => s.Capacidad == capacidad.Value).ToList();
            }

            return Json(salas);
        }

        private List<Sala> ObtenerTodasLasSalas()
        {
            // Datos simulados, reemplaza con lógica para obtener datos de la BD
            return new List<Sala>
        {
            new Sala { Id = 1, NombreSala = "Sala A", Descripcion = "Sala de reuniones", Capacidad = 20, Edificio = "Edificio Central" },
            new Sala { Id = 2, NombreSala = "Sala B", Descripcion = "Auditorio", Capacidad = 100, Edificio = "Edificio Norte" }
        };
        }
    }

}
public class Sala
{
    public int Id { get; set; }
    public string NombreSala { get; set; }
    public string Descripcion { get; set; }
    public int Capacidad { get; set; }
    public string Edificio { get; set; }
}
