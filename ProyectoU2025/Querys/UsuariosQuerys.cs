namespace ProyectoU2025.Querys
{
    public class UsuariosQuerys
    {
        public static string GetByEmail = @"
        SELECT * FROM t_usuario
        WHERE usu_email = @usu_email";  

        public static string GetByGoogleId = @"
        SELECT * FROM t_usuario
        WHERE usu_google_id = @usu_google_id";

        public static string Insert = "sp_InsertUsuario";      
        public static string Update = "sp_UpdateUsuario";

    }
}
