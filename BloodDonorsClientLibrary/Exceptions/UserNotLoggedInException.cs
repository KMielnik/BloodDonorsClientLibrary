using System;

namespace BloodDonorsClientLibrary.Exceptions
{
    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException()
        {
        }

        public UserNotLoggedInException(string message) : base(message)
        {
        }

        public UserNotLoggedInException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}