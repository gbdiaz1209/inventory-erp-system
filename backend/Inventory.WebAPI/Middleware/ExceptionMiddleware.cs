using System.Net;
using System.Text.Json;

namespace Inventory.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Intenta seguir con la petición
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error no controlado.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = "Error interno en el servidor de Inventory.system",
                Details = exception.Message // En producción, esto debería ser más discreto
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}