using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	/// <summary>
	/// Generic bus exception.
	/// </summary>
	class BusException : Exception
	{
		/// <summary>
		/// The bus that connected to the exception.
		/// </summary>
		public Bus Bus { get; }
		public BusException(string message, Bus bus) : base(message)
		{
			Bus = bus;
		}
	}

	/// <summary>
	/// Exception thrown when the trying to create bus with the same registration number.
	/// </summary>
	class BusExistingException : BusException
	{
		public BusExistingException(Bus existingBus)
			: base("Bus with the same registration number already existing", existingBus)
		{ }
	}

	/// <summary>
	/// Exception thrown when the digits and the date doesn't match.
	/// </summary>
	class RegistrationException : Exception
	{
		public RegistrationException(bool before2018)
			: base(before2018 ? "Before 2018 the registration number should contain 7 digits"
							  : "After 2018 the registration number should contain 8 digits")
		{ }
	}
}
