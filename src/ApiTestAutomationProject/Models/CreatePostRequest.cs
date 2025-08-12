namespace ApiTestAutomationProject.Models
{
    public class CreatePostRequest
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }

    public class CreatePostResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }
} 