using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet_5781_03B_1105_4185
{
	class BusException : Exception
	{
		public Bus Bus { get; }
		public BusException(string message, Bus bus) : base(message)
		{
			Bus = bus;
		}
	}

	class BusExistingException : BusException
	{
		public BusExistingException(Bus existingBus)
			: base("Bus with the same registration number already existing", existingBus)
		{ }
	}

	class RegistrationException : Exception
	{
		public RegistrationException(bool before2018)
			: base(before2018 ? "Before 2018 the registration number should contain 7 digits"
							  : "After 2018 the registration number should contain 8 digits")
		{ }
	}
}
