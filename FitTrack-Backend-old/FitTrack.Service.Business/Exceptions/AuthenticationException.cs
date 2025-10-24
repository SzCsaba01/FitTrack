using System.Net;

namespace FitTrack.Service.Business.Exceptions;

public class AuthenticationException : ApiExceptionBase
{
    public AuthenticationException()
    : base(HttpStatusCode.Unauthorized, "Authentication Error") { }

    public AuthenticationException(string message)
        : base(HttpStatusCode.Unauthorized, message, "Authentication Error") { }

    public AuthenticationException(string message, Exception innerException)
        : base(HttpStatusCode.Unauthorized, message, innerException, "Authentication Error") { }

}
