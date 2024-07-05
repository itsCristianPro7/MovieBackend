namespace Movies.Api.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhrase { get; set; }
        public DateTime Timestamp { get; set; }
        public List<String> Errors { get; set; } = [];
    }
}
