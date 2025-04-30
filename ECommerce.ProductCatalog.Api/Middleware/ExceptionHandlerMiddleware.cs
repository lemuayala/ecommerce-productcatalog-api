using System.Net;
using System.Text.Json;

namespace ECommerce.ProductCatalog.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Excepción no controlada: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        var errorResponse = new { message = "Ocurrió un error interno en el servidor." };

        switch (exception)
        {
            case Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException:
                if (dbUpdateException.InnerException is Microsoft.Data.SqlClient.SqlException sqlException)
                {
                    if (sqlException.Number == 547) // Código de error para violación de restricción de clave foránea
                    {
                        statusCode = HttpStatusCode.BadRequest;
                        errorResponse = new { message = "Error de validación: La categoría especificada no existe." };
                    }
                }
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        await httpContext.Response.WriteAsync(jsonResponse);
    }
}