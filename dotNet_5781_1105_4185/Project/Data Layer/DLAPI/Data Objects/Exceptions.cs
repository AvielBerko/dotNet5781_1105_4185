using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
	#region User
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
	#endregion

	#region Station
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
	#endregion

	#region Bus
	public class BadBusRegistrationException : Exception
	{
        public int RegNum { get; }
        public DateTime? RegDate { get; }
        public BadBusRegistrationException(int regNum, DateTime? regDate = null)
        {
            RegNum = regNum;
            RegDate = regDate;
        }
        public BadBusRegistrationException(int regNum, string message) : base(message)
        {
            RegNum = RegNum;
        }
        public BadBusRegistrationException(int regNum, DateTime regDate, string message) : base(message)
        {
            RegNum = RegNum;
            RegDate = regDate;
        }
        public BadBusRegistrationException(int regNum, string message, Exception innerException) : base(message, innerException)
        {
            RegNum = RegNum;
        }
        public BadBusRegistrationException(int regNum, DateTime regDate, string message, Exception innerException) : base(message, innerException)
        {
            RegNum = RegNum;
            RegDate = regDate;
        }
    }
	#endregion
}
