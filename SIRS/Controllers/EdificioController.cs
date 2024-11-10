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

        private readonly HttpClient _httpClient; public EdificioService(HttpClient httpClient) { _httpClient = httpClient; }
        public async Task<IEnumerable<EdificioViewModel>> GetAllEdificiosAsync()
        {
            var response = await _httpClient.GetAsync("https://api-url/api/edificio"); response.EnsureSuccessStatusCode(); var responseBody = await response.Content.ReadAsStringAsync(); var edificios = JsonConvert.DeserializeObject<IEnumerable<EdificioViewModel>>(responseBody); return edificios;
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
