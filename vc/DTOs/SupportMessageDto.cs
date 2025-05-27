namespace vc.DTOs
{
    public class SupportMessageDto
    {
        public int Id { get; set; } // Optional: exclude if auto-generated and not needed on create
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
