namespace BookHub.BLL.Services.Implementations
{
    public class ServiceResult
    {
        public object Data { get; set; } 
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static ServiceResult SuccessResult(object data = null)
            => new ServiceResult { Data = data, Success = true };

        public static ServiceResult ErrorResult(string errorMessage)
            => new ServiceResult { Success = false, ErrorMessage = errorMessage };
    }
}
