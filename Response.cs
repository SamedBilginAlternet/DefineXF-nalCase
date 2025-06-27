namespace DefineXFinalCase
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public Response() { }
        public Response(T data, string? message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }
        public Response(string message, List<string>? errors = null)
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}
