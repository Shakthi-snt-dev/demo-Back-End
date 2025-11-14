using System.Text.Json.Serialization;

namespace Flowtap_Domain.DtoModel;

public class ApiResponseDto<T>
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("message")]
    public object? Message { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    public static ApiResponseDto<T> Success(T data, string? message = null)
    {
        return new ApiResponseDto<T>
        {
            Status = true,
            Message = message ?? "Operation completed successfully",
            Data = data
        };
    }

    public static ApiResponseDto<T> Failure(string message, T? data = default)
    {
        return new ApiResponseDto<T>
        {
            Status = false,
            Message = message,
            Data = data
        };
    }
}

// Non-generic version for cases where no data is returned
public class ApiResponseDto
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("message")]
    public object? Message { get; set; }

    [JsonPropertyName("data")]
    public object? Data { get; set; }

    public static ApiResponseDto Success(string? message = null, object? data = null)
    {
        return new ApiResponseDto
        {
            Status = true,
            Message = message ?? "Operation completed successfully",
            Data = data
        };
    }

    public static ApiResponseDto Failure(string message, object? data = null)
    {
        return new ApiResponseDto
        {
            Status = false,
            Message = message,
            Data = data
        };
    }
}

