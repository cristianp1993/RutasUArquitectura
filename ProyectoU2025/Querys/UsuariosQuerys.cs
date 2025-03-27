namespace ProyectoU2025.Querys
{
    public class UsuariosQuerys
    {
        public static string GetByEmail => @"
        SELECT usu_id as Id, usu_email as Email, usu_rol as Rol 
        FROM t_usuarios 
        WHERE usu_email = @Email";
    }
}
