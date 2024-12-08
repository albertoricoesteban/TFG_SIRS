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

                var response = await _httpClient.GetAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControlador}usuario/data");

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
                // Serializa los datos en JSON
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Realiza la solicitud POST
                var response = await _httpClient.PostAsync(uri, content);

                // Verifica si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)responseData; // Devuelve el string directamente si el tipo genérico es string
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(responseData); // Deserializa según el tipo genérico
                    }
                }
                else
                {
                    // Si la respuesta no es exitosa, lanzar una excepción opcional o manejar el error
                    return default(T);
                }
            }
            catch (HttpRequestException ex)
            {
                // Maneja excepciones HTTP, por ejemplo, errores de conexión o 401 Unauthorized
                return default(T);
            }
        }
        public async Task<T> PutAsync<T>(string url, T data)
        {
            try
            {
                // Serializamos el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Realizamos la solicitud PUT al servidor
                var response = await _httpClient.PutAsync(url, content);

                // Verificamos si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Si la respuesta es exitosa, deserializamos la respuesta en el tipo esperado
                    var responseData = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(responseData);
                    return result;
                }
                else
                {
                    // Si la respuesta no fue exitosa, lanzar una excepción o manejar el error de alguna manera
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error en la solicitud PUT: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Capturamos y lanzamos cualquier excepción que ocurra
                throw new Exception($"Hubo un problema al realizar la solicitud PUT: {ex.Message}");
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
