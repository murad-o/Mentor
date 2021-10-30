using System;

namespace MentorCore.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException()
        {
        }
    }
}
