namespace BookHub.BLL.Utils
{
    public class ServiceResultType
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static ServiceResultType SuccessResult()
            => new ServiceResultType { Success = true };

        public static ServiceResultType ErrorResult(string errorMessage)
            => new ServiceResultType { Success = false, ErrorMessage = errorMessage };
    }
}
