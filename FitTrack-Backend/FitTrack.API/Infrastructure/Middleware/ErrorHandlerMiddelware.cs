using System.Net;
using System.Text.Json;
using FitTrack.Service.Business.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Infrastructure.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, title, detail) = exception switch
            {
                ApiExceptionBase apiEx => ((int)apiEx.StatusCode, apiEx.Title, apiEx.Message),
                _ => ((int)HttpStatusCode.InternalServerError, "Unexpected Error", exception.Message)
            };

            _logger.LogError(exception, "Unhandled exception caught by middleware. Status: {StatusCode}, Path: {Path}", statusCode, context.Request.Path);

            response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(json);
        }
    }
}
