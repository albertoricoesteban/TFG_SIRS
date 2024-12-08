using System.Net.Http.Headers;

namespace SIRS
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public void StoreJwtToken(string token)
        {
            // Verificar que HttpContext y Session estén disponibles
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session != null)
            {
                // Guardar el token JWT en la sesión
                session.SetString("JwtToken", token);
            }
            else
            {
                // Manejar el caso cuando la sesión no esté disponible
                Console.WriteLine("Error: La sesión no está disponible.");
            }
        }
        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginRequest = new
            {
                Username = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControlador}login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<dynamic>();
                var token = data?.Token.ToString();
                var session = _httpContextAccessor.HttpContext?.Session;

                // Almacenar el token en el almacenamiento local o en la sesión
                StoreJwtToken(token);

                // También puedes configurar el token en las cabeceras de las solicitudes
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return true;
            }

            return false;
        }
    }

}
