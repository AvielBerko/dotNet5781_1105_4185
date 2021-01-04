using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BadUserNameException : Exception
    {
        public string Name { get; }

        public BadUserNameException(string name)
        {
            Name = name;
        }
        public BadUserNameException(string name, string message) : base(message)
        {
            Name = name;
        }
        public BadUserNameException(string name, string message, Exception innerException) : base(message, innerException)
        {
            Name = name;
        }
    }

    public class BadStationCodeException : Exception
    {
        public int Code { get; }

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
}
