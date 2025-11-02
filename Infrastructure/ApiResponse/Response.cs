using Infrastructure.Enum;

namespace Infrastructure.ApiResponse;

public class Response<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public ErrorType ErrorType { get; set; }


    public static Response<T> Ok(T? data, string? message = null)
        => new()
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };

    public static Response<T> Fail(string message, ErrorType errorType = ErrorType.Internal)
        => new()
        {
            IsSuccess = false,
            Message = message,
            ErrorType = errorType
        };
}