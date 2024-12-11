using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SIRS.ApliClient
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método privado para establecer el encabezado de autorización con el token JWT
        private void SetAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Método para el login (sin necesidad de encabezado Bearer)
        public async Task<string> PostAsyncLogin(string uri, object loginData)
        {
            try
            {
                // Serializar los datos del login
                var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                // Realizar la solicitud POST para el login
                var response = await _httpClient.PostAsync(uri, content);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Obtener el token del cuerpo de la respuesta (suponiendo que el servidor lo devuelve en el cuerpo)
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody; // Aquí devolvemos el token como string
                }

                // Si la respuesta no fue exitosa, devolver null
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al realizar POST en {uri}: {ex.Message}", ex);
            }
        }

        // Método para obtener datos del usuario (requiere Bearer)
        public async Task<string> GetUsuarioDataAsync()
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControlador}usuario/data");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        // Método genérico GET (requiere Bearer)
        public async Task<T> GetAsync<T>(string uri)
        {
            try
            {
                SetAuthorizationHeader();

                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar GET en {uri}: {ex.Message}", ex);
            }
        }

        // Método POST genérico (requiere Bearer)
        public async Task PostAsyncWithId(string uri, int id, int loggedInUserId)
        {
            try
            {
                SetAuthorizationHeader();

                string requestUri = $"{uri}?id={id}&loggedInUserId={loggedInUserId}";
                var response = await _httpClient.PostAsync(requestUri, null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al realizar POST en {uri}: {ex.Message}", ex);
            }
        }

        // Método POST (requiere Bearer)
        public async Task PostAsync(string uri, object data)
        {
            try
            {
                SetAuthorizationHeader();

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al realizar POST en {uri}: {ex.Message}", ex);
            }
        }

        // Método POST genérico (requiere Bearer)
        public async Task<T> PostAsync<T>(string uri, object data)
        {
            try
            {
                SetAuthorizationHeader();

                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseData);
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la solicitud POST en {uri}: {ex.Message}", ex);
            }
        }

        // Método PUT (requiere Bearer)
        public async Task<T> PutAsync<T>(string url, T data)
        {
            try
            {
                SetAuthorizationHeader();

                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseData);
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error en la solicitud PUT: {errorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Hubo un problema al realizar la solicitud PUT: {ex.Message}");
            }
        }

        // Método PUT genérico (requiere Bearer)
        public async Task PutAsync(string uri, object data)
        {
            try
            {
                SetAuthorizationHeader();

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al realizar la solicitud PUT: {ex.Message}", ex);
            }
        }

        // Método DELETE (requiere Bearer)
        public async Task DeleteAsync(string uri)
        {
            try
            {
                SetAuthorizationHeader();

                var response = await _httpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar DELETE en {uri}: {ex.Message}", ex);
            }
        }
    }
}
