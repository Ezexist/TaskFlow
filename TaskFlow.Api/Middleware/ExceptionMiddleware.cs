
using System.Text.Json;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex,ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,

                NotFoundException => StatusCodes.Status404NotFound,

                ConflictException => StatusCodes.Status409Conflict,

                UnauthorizedException => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
