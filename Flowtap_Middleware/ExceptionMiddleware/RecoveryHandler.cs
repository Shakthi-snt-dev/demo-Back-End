using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Flowtap_Domain.Exceptions;

namespace Flowtap_Middleware.ExceptionMiddleware;

public class RecoveryHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RecoveryHandler> _logger;

    public RecoveryHandler(RequestDelegate next, ILogger<RecoveryHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    public record ExceptionResponse(HttpStatusCode StatusCode, HttpResponse ErrorResponse);

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        ExceptionResponse response = exception switch
        {
            // Custom Application Exceptions
            UserAlreadyExistException userExists => new ExceptionResponse(
                HttpStatusCode.Conflict,
                new HttpResponse("USER_ALREADY_EXISTS", userExists.GetMessage())
            ),
            EntityNotFoundException notFound => new ExceptionResponse(
                HttpStatusCode.NotFound,
                new HttpResponse("ENTITY_NOT_FOUND", notFound.GetMessage())
            ),
            EmailNotVerifiedException emailNotVerified => new ExceptionResponse(
                HttpStatusCode.Forbidden,
                new HttpResponse("EMAIL_NOT_VERIFIED", emailNotVerified.GetMessage())
            ),
            AccountDeactivatedException accountDeactivated => new ExceptionResponse(
                HttpStatusCode.Forbidden,
                new HttpResponse("ACCOUNT_DEACTIVATED", accountDeactivated.GetMessage())
            ),
            UnauthorizedException unauthorized => new ExceptionResponse(
                HttpStatusCode.Unauthorized,
                new HttpResponse("UNAUTHORIZED", unauthorized.GetMessage())
            ),
            Flowtap_Domain.Exceptions.InvalidOperationException invalidOp => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("INVALID_OPERATION", invalidOp.GetMessage())
            ),
            ValidationException validation => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("VALIDATION_ERROR", validation.GetMessage())
            ),
            ApplicationException appEx => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("APPLICATION_ERROR", appEx.GetMessage())
            ),
            // Standard .NET Exceptions
            ArgumentNullException => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("ARGUMENT_NULL", exception.Message)
            ),
            ArgumentException => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("INVALID_ARGUMENT", exception.Message)
            ),
            System.InvalidOperationException => new ExceptionResponse(
                HttpStatusCode.BadRequest,
                new HttpResponse("INVALID_OPERATION", exception.Message)
            ),
            UnauthorizedAccessException => new ExceptionResponse(
                HttpStatusCode.Unauthorized,
                new HttpResponse("UNAUTHORIZED", exception.Message)
            ),
            KeyNotFoundException => new ExceptionResponse(
                HttpStatusCode.NotFound,
                new HttpResponse("NOT_FOUND", exception.Message)
            ),
            _ => new ExceptionResponse(
                HttpStatusCode.InternalServerError,
                new HttpResponse("INTERNAL_ERROR", "An unexpected error occurred. Please try again later.")
            )
        };

        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.ContentType = "application/json";

        // Format response to match standard ApiResponseDto format
        var errorResponse = new
        {
            status = response.ErrorResponse.Status,
            message = response.ErrorResponse.ErrorMessage,
            data = (object?)null,
            code = response.ErrorResponse.Code
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

