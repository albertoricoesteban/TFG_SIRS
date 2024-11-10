using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using System.Threading.Tasks;

[Route("api/[controller]")]
public class EdificioController : ControllerBase
{
    private readonly ApiClientService _apiClientService;

    public EdificioController(ApiClientService apiClientService)
    {
        _apiClientService = apiClientService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var edificios = await _apiClientService.GetAsync<IEnumerable<EdificioViewModel>>("https://api-url/api/edificio/getall");
        return Ok(edificios);
    }

    [HttpGet("GetEdificiosPorFiltro")]
    public async Task<IActionResult> GetEdificiosPorFiltro(string nombre, string direccion)
    {
        var uri = $"https://api-url/api/edificio/getbyfilter?nombre={nombre}&direccion={direccion}";
        var edificios = await _apiClientService.GetAsync<IEnumerable<EdificioViewModel>>(uri);
        return Ok(edificios);
    }
}

