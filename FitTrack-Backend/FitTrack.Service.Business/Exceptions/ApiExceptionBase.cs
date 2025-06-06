using System.Net;

namespace FitTrack.Service.Business.Exceptions;

public class ApiExceptionBase : Exception
{
    public readonly HttpStatusCode StatusCode;
    public string Title { get; set; }

    public ApiExceptionBase(HttpStatusCode statusCode, string title = "Error")
    {
        StatusCode = statusCode;
        Title = title;
    }

    public ApiExceptionBase(HttpStatusCode statusCode, string message, string title = "Error") : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    public ApiExceptionBase(HttpStatusCode statusCode, string message, Exception innerException, string title = "Error") : base(message, innerException)
    {
        StatusCode = statusCode;
        Title = title;
    }
}
