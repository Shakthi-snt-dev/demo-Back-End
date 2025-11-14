using System.Text.Json.Serialization;

namespace Flowtap_Middleware.ExceptionMiddleware;

public class HttpResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public bool Status { get; set; }

    public HttpResponse()
    {
        Status = false;
    }

    public HttpResponse(string code, string description)
    {
        Code = code;
        Status = false;
        ErrorMessage = description;
    }
}

