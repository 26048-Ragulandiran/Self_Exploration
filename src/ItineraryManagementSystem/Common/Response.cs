namespace ItineraryManagementSystem.Common
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }

        public object? Query { get; set; }
    }
}
