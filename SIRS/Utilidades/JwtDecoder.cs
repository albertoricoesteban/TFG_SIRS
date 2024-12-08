using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SIRS.Utilidades
{
    public class JwtDecoder
    {
        public static JwtPayload DecodeJwtToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                throw new ArgumentException("El JWT no puede ser nulo o vacío.", nameof(jwtToken));

            // Dividir el token en partes
            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("El JWT no tiene el formato esperado.", nameof(jwtToken));

            // Decodificar el payload (segunda parte del JWT)
            var payloadBase64 = parts[1];

            // Ajustar Base64url a Base64
            payloadBase64 = payloadBase64.Replace('-', '+').Replace('_', '/');
            switch (payloadBase64.Length % 4)
            {
                case 2: payloadBase64 += "=="; break;
                case 3: payloadBase64 += "="; break;
            }

            // Convertir Base64 a string JSON
            byte[] payloadBytes = Convert.FromBase64String(payloadBase64);
            string payloadJson = Encoding.UTF8.GetString(payloadBytes);

            // Deserializar JSON al objeto JwtPayload
            var payloadObject = JsonConvert.DeserializeObject<JwtPayload>(payloadJson);

            if (payloadObject == null)
                throw new Exception("No se pudo deserializar el payload del JWT.");

            return payloadObject;
        }
    }
}
