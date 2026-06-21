namespace ERP.Business.Common.Models;

public class ServiceResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public List<string> Errors { get; init; } = new();

    public static ServiceResult SuccessResult(string message = "Success") =>
        new() { Success = true, Message = message };

    public static ServiceResult Failure(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? new List<string>() };
}

public class ServiceResult<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public List<string> Errors { get; init; } = new();

    public static ServiceResult<T> SuccessResult(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ServiceResult<T> Failure(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? new List<string>() };
}
