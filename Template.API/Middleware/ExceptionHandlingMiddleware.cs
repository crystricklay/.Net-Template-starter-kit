using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Template.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";

            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                ValidationException => (HttpStatusCode.BadRequest, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Internal server error")
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Detailed = context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment() ? ex.ToString() : null,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}