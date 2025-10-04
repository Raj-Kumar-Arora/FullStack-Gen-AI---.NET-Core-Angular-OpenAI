namespace WebAPI.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; } 
        public string OutputType { get; set; } = "json"; // e.g. "test-case" / "user-story"
    }
}
