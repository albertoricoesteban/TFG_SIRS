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
        public async Task<string> GetUsuarioDataAsync()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControloador}usuario/data");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return null;
        }
        public async Task<T> GetAsync<T>(string uri)
        {
            try
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception ex)
            {
                // Maneja el error aquí (por ejemplo, log o lanzar excepción personalizada)
                throw new Exception($"Error al realizar GET en {uri}: {ex.Message}", ex);
            }
        }

        public async Task PostAsync(string uri, object data)
        {
            try
            {
                // Serializar los datos a JSON
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                var response = await _httpClient.PostAsync(uri, content);

                // Verificar si la respuesta fue exitosa
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpEx)
            {
                // Manejo específico para errores relacionados con HTTP
                throw new Exception($"Error de conexión o solicitud HTTP fallida al realizar POST en {uri}: {httpEx.Message}", httpEx);
            }
            catch (TaskCanceledException timeoutEx)
            {
                // Manejo de errores de tiempo de espera
                throw new Exception($"La solicitud POST a {uri} se agotó. Por favor, verifica tu conexión de red o el servidor.", timeoutEx);
            }
            catch (JsonSerializationException jsonEx)
            {
                // Manejo de errores en caso de que la serialización falle
                throw new Exception($"Error al serializar JSON al realizar POST en {uri}: {jsonEx.Message}", jsonEx);
            }
            catch (Exception ex)
            {
                // Manejo general de otros errores inesperados
                throw new Exception($"Ocurrió un error inesperado al realizar POST en {uri}: {ex.Message}", ex);
            }
        }
        public async Task<T> PostAsync<T>(string uri, object data)
        {
            try
            {
                // Serializar los datos a JSON
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                var response = await _httpClient.PostAsync(uri, content);

                // Verificar si la respuesta fue exitosa
                response.EnsureSuccessStatusCode();

                // Leer la respuesta y deserializarla en el tipo solicitado
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"Error de conexión o solicitud HTTP fallida al realizar POST en {uri}: {httpEx.Message}", httpEx);
            }
            catch (TaskCanceledException timeoutEx)
            {
                throw new Exception($"La solicitud POST a {uri} se agotó. Por favor, verifica tu conexión de red o el servidor.", timeoutEx);
            }
            catch (JsonSerializationException jsonEx)
            {
                throw new Exception($"Error al serializar JSON al realizar POST en {uri}: {jsonEx.Message}", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error inesperado al realizar POST en {uri}: {ex.Message}", ex);
            }
        }

        public async Task PutAsync(string uri, object data)
        {
            try
            {
                // Serializar los datos a JSON
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT
                var response = await _httpClient.PutAsync(uri, content);

                // Verificar si la respuesta fue exitosa
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpEx)
            {
                // Manejo específico para errores relacionados con HTTP
                // Puedes registrar el error o manejarlo de manera específica
                throw new Exception($"Error al realizar la solicitud PUT: {httpEx.Message}", httpEx);
            }
            catch (TaskCanceledException timeoutEx)
            {
                // Manejo de errores en caso de que la solicitud se agote
                throw new Exception("La solicitud PUT se agotó. Por favor, verifica tu conexión de red o el servidor.", timeoutEx);
            }
            catch (JsonSerializationException jsonEx)
            {
                // Manejo de errores en caso de que la serialización falle
                throw new Exception("Error al serializar los datos a JSON: " + jsonEx.Message, jsonEx);
            }
            catch (Exception ex)
            {
                // Manejo general de otros errores inesperados
                throw new Exception("Ocurrió un error inesperado al realizar la solicitud PUT: " + ex.Message, ex);
            }
        }


        public async Task DeleteAsync(string uri)
        {
            try
            {
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
