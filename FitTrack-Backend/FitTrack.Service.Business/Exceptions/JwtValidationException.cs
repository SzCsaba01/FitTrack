using System.Net;

namespace FitTrack.Service.Business.Exceptions;

public class JwtValidationException : ApiExceptionBase
{
    public JwtValidationException() : base(HttpStatusCode.BadRequest, "JWT validation failed") { }

    public JwtValidationException(string message) : base(HttpStatusCode.BadRequest, message, "JWT validation failed") { }

    public JwtValidationException(string message, Exception innerException) : base(HttpStatusCode.BadRequest, message, innerException, "JWT validation failed") { }
}

