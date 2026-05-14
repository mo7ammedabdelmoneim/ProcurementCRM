using CRM.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace CRM.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, error, details) = ex switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Validation failed",
                ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })),

            NotFoundException => (
                HttpStatusCode.NotFound,
                ex.Message,
                Enumerable.Empty<object>()),

            DomainException => (
                HttpStatusCode.UnprocessableEntity,
                ex.Message,
                Enumerable.Empty<object>()),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                Enumerable.Empty<object>()),

            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred",
                Enumerable.Empty<object>())
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Unhandled exception");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            error,
            details,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}