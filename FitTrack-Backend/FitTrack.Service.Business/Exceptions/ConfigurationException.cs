using System.Net;

namespace FitTrack.Service.Business.Exceptions;

public class ConfigurationException : ApiExceptionBase
{
    public ConfigurationException()
        : base(HttpStatusCode.InternalServerError, "Configuration Error") { }

    public ConfigurationException(string message)
        : base(HttpStatusCode.InternalServerError, message, "Configuration Error") { }

    public ConfigurationException(string message, Exception innerException)
        : base(HttpStatusCode.InternalServerError, message, innerException, "Configuration Error") { }
}
