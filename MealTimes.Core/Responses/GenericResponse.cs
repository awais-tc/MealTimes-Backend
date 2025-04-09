namespace MealTimes.Core.Responses
{
    public class GenericResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; } // ✅ Add this

        public static GenericResponse<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new GenericResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static GenericResponse<T> Fail(string message, int statusCode = 400)
        {
            return new GenericResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }
    }
}
