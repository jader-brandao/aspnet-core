using System;
using System.Net;

namespace Infra.Exceptions
{
    public static class HttpStatusCodeExceptionExtension
    {
        public static HttpStatusCode GetHttpStatusCode(this Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;

            if (exception is ValidationDomainException)
                status = HttpStatusCode.UnprocessableEntity;

            return status;
        }
    }
}
