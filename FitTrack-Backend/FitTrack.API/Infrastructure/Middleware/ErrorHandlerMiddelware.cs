using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Infrastructure.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, title) = exception switch
            {
                _ => ((int)HttpStatusCode.InternalServerError, "Unexpected Error")
            };

            response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(json);
        }
    }
}
