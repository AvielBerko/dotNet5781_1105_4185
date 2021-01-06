﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	#region User
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

    public class BadNameValidationException : Exception
    {
        public string Name { get; }

        public BadNameValidationException(string name)
        {
            Name = name;
        }
        public BadNameValidationException(string name, string message) : base(message)
        {
            Name = name;
        }
        public BadNameValidationException(string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
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
	#endregion

	#region Bus
    public class BadBusRegistrationException : Exception
	{
        public Registration Registration { get; }

        public BadBusRegistrationException(Registration registration)
        {
            Registration = registration;
        }
        public BadBusRegistrationException(Registration registration, string message) : base(message)
        {
            Registration = registration;
        }
        public BadBusRegistrationException(Registration registration, string message, Exception innerException) : base(message, innerException)
        {
            Registration = registration;
        }
    }
    #endregion

    #region Station
    public class BadStationCodeException : Exception
    {
        public int Code{ get; }

        public BadStationCodeException(int code)
        {
            Code = code;
        }
        public BadStationCodeException(int code, string message) : base(message)
        {
            Code = code;
        }
        public BadStationCodeException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
    #endregion
}
