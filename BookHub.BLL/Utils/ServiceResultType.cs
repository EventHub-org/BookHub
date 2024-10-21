namespace BookHub.BLL.Utils
{
    public class ServiceResultType<T>
    {
        public T Data { get; set; } 
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static ServiceResultType<T> SuccessResult(T data)
            => new ServiceResultType<T> { Data = data, Success = true };

        public static ServiceResultType<T> ErrorResult(string errorMessage)
            => new ServiceResultType<T> { Success = false, ErrorMessage = errorMessage };
    }
}
