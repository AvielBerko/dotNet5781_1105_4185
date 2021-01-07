using System;
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
    public class BadStationNameException : Exception
    {
        public string Name { get; }

        public BadStationNameException(string name)
        {
            Name = name;
        }
        public BadStationNameException(string name, string message) : base(message)
        {
            Name = name;
        }
        public BadStationNameException(string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
        }
    }
    public class BadStationAddressException : Exception
    {
        public string Address { get; }

        public BadStationAddressException(string address)
        {
            Address = address;
        }
        public BadStationAddressException(string address, string message) : base(message)
        {
            Address = address;
        }
        public BadStationAddressException(string address, string message, Exception innerException) : base(message, innerException)
        {
            Address = address;
        }
    }
    public class BadStationLocationException : Exception
    {
        public BO.Location Location { get; }

        public BadStationLocationException(BO.Location location)
        {
            Location = location;
        }
        public BadStationLocationException(BO.Location location, string message) : base(message)
        {
            Location = location;
        }
        public BadStationLocationException(BO.Location location, string message, Exception innerException) : base(message, innerException)
        {
            Location = location;
        }
    }
    #endregion
}
