
namespace UrPOS.Core.Entities
{
    public class ServiceResult
    {
        public bool isSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        public static ServiceResult Success() => new ServiceResult { isSuccess = true };
        public static ServiceResult Failure(string message) => new ServiceResult { isSuccess = false, ErrorMessage= message };

    }

    public class ServiceResult<T>
    {
        public bool isSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { isSuccess = true, Data = data };
        public static ServiceResult<T> Failure(string message) => new ServiceResult<T> { isSuccess = false, ErrorMessage = message };
    }
}
