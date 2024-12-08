namespace SIRS.Constantes
{
    public static class Constantes
    {
        // Constantes de cadena
        public const string ApplicationName = "SIRS";
        public const string Version = "1.0.0";

        // Constantes de configuración
        public const int MaxItemsPerPage = 50;

        // Rutas de la API
        public const string ApiBaseUrl = "http://localhost:5237/api/";
        public const string AuthControlador = "Auth/";
        public const string AccountControlador = "Account/";
        public const string EdificioControlador = "Edificio/";
        public const string SalaControlador = "Sala/";
        public const string EstadoSalaControlador = "EstadoSala/";
        public const string ReservaControlador = "Reserva/";
        public const string RolControlador = "Rol/";
        // Mensajes de la Aplicación
        public const string SuccessMessage = "Operación completada con éxito";
        public const string ErrorMessage = "Ha ocurrido un error, inténtelo de nuevo";
    }

}
