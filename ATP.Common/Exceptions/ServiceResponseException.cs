using System;

namespace ATP.Common.Exceptions
{
    public class ServiceResponseException : Exception
    {
        public ServiceResponseException(string message)
            : this(new Exception(message))
        {
        }

        public ServiceResponseException(Exception inner, string message)
            : base(message, inner)
        {
        }

        public ServiceResponseException(string message, params object[] args)
            : this(new Exception(String.Format(message, args)))
        {
        }

        public ServiceResponseException(Exception inner, string message, params object[] args)
            : base(String.Format(message, args), inner)
        {
        }

        public ServiceResponseException(Exception inner)
            : base(Messages.ServiceResponseException, inner)
        {
        }
    }
}
