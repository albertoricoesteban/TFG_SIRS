using System.Text.RegularExpressions;

namespace SIRS.Utilidades
{
    public class Utilities
    {
        public static bool EsNIFNIEValido(string nifNie)
        {
            if (string.IsNullOrEmpty(nifNie))
                return false;

            // Eliminamos posibles espacios o guiones
            nifNie = nifNie.Trim().ToUpper();

            // Validación de formato para NIF y NIE
            string nifPattern = @"^\d{8}[A-Z]$"; // Ejemplo: 12345678A
            string niePattern = @"^[XYZ]\d{7}[A-Z]$"; // Ejemplo: X1234567L

            if (!Regex.IsMatch(nifNie, nifPattern) && !Regex.IsMatch(nifNie, niePattern))
                return false;

            // Si es NIE, convertimos la letra inicial (X, Y, Z) en un número
            if (nifNie.StartsWith("X"))
                nifNie = "0" + nifNie.Substring(1);
            else if (nifNie.StartsWith("Y"))
                nifNie = "1" + nifNie.Substring(1);
            else if (nifNie.StartsWith("Z"))
                nifNie = "2" + nifNie.Substring(1);

            // Extraemos el número y la letra
            string numero = nifNie.Substring(0, 8);
            char letra = nifNie[8];

            // Calculamos la letra correcta usando el número
            string letrasValidas = "TRWAGMYFPDXBNJZSQVHLCKE";
            char letraCalculada = letrasValidas[int.Parse(numero) % 23];

            // Comparamos la letra calculada con la proporcionada
            return letra == letraCalculada;
        }
    }
}
