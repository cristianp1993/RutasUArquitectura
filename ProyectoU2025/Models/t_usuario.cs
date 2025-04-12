namespace ProyectoU2025.Models
{
    public class t_usuario
    {
        public int usu_id { get; set; }
        public string usu_google_id { get; set; }
        public string usu_email { get; set; }
        public string usu_rol { get; set; }
        public string usu_nombre { get; set; }
        public DateTime usu_fecha_registro { get; set; }
        public string usu_contrasenia { get; set; }
    }
}
