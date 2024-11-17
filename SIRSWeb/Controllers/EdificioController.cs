using Microsoft.AspNetCore.Mvc;
using SIRS.Application.ViewModels;
using SIRS.Web.ApliClient;
using System.Threading.Tasks;

namespace SIRSWeb.Controllers
{
    public class EdificioController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public EdificioController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }
        public ActionResult Index()
        {
            return View();
        }
        // GET: /Edificio/GetAll
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var edificios = await _apiClientService.GetAsync<List<EdificioViewModel>>("edificio/GetAll");
                return View(edificios);
            }
            catch (Exception ex)
            {
                // Log the exception and display an error message to the user
                ViewBag.ErrorMessage = "Error al obtener los edificios: " + ex.Message;
                return View("Error");
            }
        }

        // GET: /Edificio/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var edificio = await _apiClientService.GetAsync<EdificioViewModel>($"edificio/GetById/{id}");
                if (edificio == null)
                {
                    return NotFound();
                }
                return View(edificio);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener los detalles del edificio: " + ex.Message;
                return View("Error");
            }
        }

        // POST: /Edificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EdificioViewModel edificio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _apiClientService.PostAsync("edificio/Add", edificio);
                    return RedirectToAction(nameof(GetAll));
                }
                return View(edificio);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al crear el edificio: " + ex.Message;
                return View("Error");
            }
        }

        // GET: /Edificio/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var edificio = await _apiClientService.GetAsync<EdificioViewModel>($"edificio/GetById/{id}");
                if (edificio == null)
                {
                    return NotFound();
                }
                return View(edificio);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener el edificio para editar: " + ex.Message;
                return View("Error");
            }
        }

        // POST: /Edificio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EdificioViewModel edificio)
        {
            try
            {
                if (id != edificio.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    await _apiClientService.PutAsync($"edificio/Update/{id}", edificio);
                    return RedirectToAction(nameof(GetAll));
                }
                return View(edificio);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al actualizar el edificio: " + ex.Message;
                return View("Error");
            }
        }

        // GET: /Edificio/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var edificio = await _apiClientService.GetAsync<EdificioViewModel>($"edificio/GetById/{id}");
                if (edificio == null)
                {
                    return NotFound();
                }
                return View(edificio);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener el edificio para eliminar: " + ex.Message;
                return View("Error");
            }
        }

        // POST: /Edificio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _apiClientService.DeleteAsync($"edificio/Delete/{id}");
                return RedirectToAction(nameof(GetAll));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al eliminar el edificio: " + ex.Message;
                return View("Error");
            }
        }

        // GET: /Edificio/SearchByName
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var edificios = await _apiClientService.GetAsync<List<EdificioViewModel>>($"edificio/Searchbyname/{name}");
                if (edificios == null || !edificios.Any())
                {
                    ViewBag.WarningMessage = "No se encontraron edificios con ese nombre.";
                    return View(new List<EdificioViewModel>());
                }
                return View(edificios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al buscar edificios: " + ex.Message;
                return View("Error");
            }
        }
    }
}