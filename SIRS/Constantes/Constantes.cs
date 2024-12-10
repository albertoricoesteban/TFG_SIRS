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
        public const string AccountControlador = "Accounts/";
        public const string EdificioControlador = "Edificios/";
        public const string SalaControlador = "Salas/";
        public const string EstadoSalaControlador = "EstadoSalas/";
        public const string ReservaControlador = "Reservas/";
        public const string RolControlador = "Roles/";
        public const string UsuarioControlador = "Usuarios/";
        // Mensajes de la Aplicación
        public const string SuccessMessage = "Operación completada con éxito";
        public const string ErrorMessage = "Ha ocurrido un error, inténtelo de nuevo";
    }

}
