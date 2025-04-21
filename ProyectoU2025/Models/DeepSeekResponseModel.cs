namespace ProyectoU2025.Models
{
    public class DeepSeekResponseModel
    {
        public List<DeepSeekChoice> Choices { get; set; }
    }

    public class DeepSeekChoice
    {
        public DeepSeekMessage Message { get; set; }
    }

    public class DeepSeekMessage
    {
        public string Content { get; set; }
    }
}
