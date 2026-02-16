using System.Text.Json;
using CryptoProj.Domain.Exceptions;

namespace CryptoProj.API.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                InvalidCredentialsException => StatusCodes.Status400BadRequest,
                EmailAlreadyTakenException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.Body.FlushAsync();
            var error = new Error(statusCode, ex.Message, ex.StackTrace);
            var errorJson = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(errorJson);
        }
    }

    record Error(int StatusCode, string Message, string? Detail = null);
}