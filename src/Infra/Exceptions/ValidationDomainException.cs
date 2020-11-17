using System;

namespace Infra.Exceptions
{
    public class ValidationDomainException : Exception
    {
        public ValidationDomainException(string message) : base(message) 
        {

        }
    }
}
