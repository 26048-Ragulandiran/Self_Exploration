namespace ItineraryManagementSystem.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public object? Meta { get; set; }
    }
}