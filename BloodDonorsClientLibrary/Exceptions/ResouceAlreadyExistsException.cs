using System;
using System.Runtime.Serialization;

namespace BloodDonorsClientLibrary.Exceptions
{
    public class ResouceAlreadyExistsException : Exception
    {
        public ResouceAlreadyExistsException()
        {
        }

        public ResouceAlreadyExistsException(string message) : base(message)
        {
        }

        public ResouceAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResouceAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}