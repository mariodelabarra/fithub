using System.Net;

namespace FitHub.Platform.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        private static readonly string DefaultMessage = "The item with ID {0} was not found.";
        public HttpStatusCode StatusCode { get; }

        public NotFoundException(string entityId)
            : base(string.Format(DefaultMessage, entityId))
        {
            StatusCode = HttpStatusCode.NotFound;
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
