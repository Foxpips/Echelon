using System;

namespace Echelon.Core.Infrastructure.Exceptions
{
    public class RegisterAccountException : Exception
    {
        public RegisterAccountException(string message) : base(message)
        {
        }
    }
}