using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BadAuthenticationException : Exception
    {
        public string Name { get; }
        public string Password { get; }

        public BadAuthenticationException(string name, string password)
        {
            Name = name;
            Password = password;
        }
        public BadAuthenticationException(string name, string password, string message) : base(message)
        {
            Name = name;
            Password = password;
        }
        public BadAuthenticationException(string name, string password, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
            Password = password;
        }
    }

    public class BadSignUpException : Exception
    {
        public string Name { get; }
        public string Password { get; }

        public BadSignUpException(string name, string password)
        {
            Name = name;
            Password = password;
        }
        public BadSignUpException(string name, string password, string message) : base(message)
        {
            Name = name;
            Password = password;
        }
        public BadSignUpException(string name, string password, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
            Password = password;
        }
    }

    public class BadPasswordValidationException : Exception
    {
        public string Password { get; }

        public BadPasswordValidationException(string password)
        {
            Password = password;
        }
        public BadPasswordValidationException(string password, string message) : base(message)
        {
            Password = password;
        }
        public BadPasswordValidationException(string password, string message, Exception innerException) : base(message, innerException)
        {
            Password = password;
        }
    }
}
