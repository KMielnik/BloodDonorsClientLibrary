using System;

namespace BloodDonorsClientLibrary.Exceptions
{
    public class InvalidLoginCredentialsException : Exception
    {
        public InvalidLoginCredentialsException()
        {
        }

        public InvalidLoginCredentialsException(string message) : base(message)
        {
        }

        public InvalidLoginCredentialsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}