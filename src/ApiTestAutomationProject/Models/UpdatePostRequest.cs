namespace ApiTestAutomationProject.Models
{
    public class UpdatePostRequest
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }

    public class UpdatePostResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }
} 