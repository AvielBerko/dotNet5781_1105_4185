using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLAPI;
using DLAPI;

namespace BL
{
	class BLImp : IBL
	{
		private readonly IDL dl = DLFactory.GetDL();

		#region User
		public BO.User UserAuthentication(string name, string password)
		{
			try
			{
				var doUser = dl.GetUser(name);

				if (doUser.Password != password)
					throw new BO.BadAuthenticationException(name, password, $"User with the name {name} doesn't have the password {password}.");

				return (BO.User)doUser.CopyPropertiesToNew(typeof(BO.User));
			}
			catch (DO.BadUserNameException)
			{
				throw new BO.BadAuthenticationException(name, password, $"User with the name {name} not found.");
			}
		}

		public BO.User UserSignUp(string name, string password)
		{
			ValidateSignUpName(name);
			ValidateSignUpPassword(password);

			dl.AddUser(new DO.User { Name = name, Password = password, Role = DO.Roles.Normal });
			return new BO.User { Name = name, Password = password, Role = BO.Roles.Normal };
		}

		public void ValidateSignUpName(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new BO.BadNameValidationException(name, "Empty name is invalid");

			try
			{
				dl.GetUser(name);
				throw new BO.BadNameValidationException(name, $"Name {name} already exists");
			}
			catch (DO.BadUserNameException)
			{
			}
		}

		public void ValidateSignUpPassword(string password)
		{
			if (string.IsNullOrEmpty(password) || password.Length < 8)
				throw new BO.BadPasswordValidationException(password, "Password should contains at least 8 characters");

			bool hasLower = false;
			bool hasUpper = false;
			bool hasDigit = false;
			bool hasPanctuation = false;

			foreach (char ch in password)
			{
				if (char.IsLower(ch)) hasLower = true;
				if (char.IsUpper(ch)) hasUpper = true;
				if (char.IsDigit(ch)) hasDigit = true;
				if (char.IsPunctuation(ch)) hasPanctuation = true;
			}

			if (!(hasLower && hasUpper && hasDigit && hasPanctuation))
				throw new BO.BadPasswordValidationException(password,
					"The password should contain at least 1 lower case letter, 1 upper case letter, 1 digit and 1 panctuation");
		}
		#endregion

		#region Station
		private BO.Station StationDoBoAdapter(DO.Station doStation)
		{
			BO.Station result = (BO.Station)doStation.CopyPropertiesToNew(typeof(BO.Station));
			result.Location = new BO.Location(doStation.Longitude, doStation.Latitude);

			return result;
		}
		private DO.Station StationBoDoAdapter(BO.Station boStation)
		{
			DO.Station result = (DO.Station)boStation.CopyPropertiesToNew(typeof(DO.Station));
			result.Longitude = boStation.Location.Longitude;
			result.Latitude = boStation.Location.Latitude;

			return result;
		}
		public void AddStation(BO.Station station)
		{
			ValidateStationCode(station.Code);
			ValidateStationName(station.Name);
			ValidateStationAddress(station.Address);
			ValidateStationLocation(station.Location);

			dl.AddStation(StationBoDoAdapter(station));
		}

		public void DeleteStation(BO.Station station)
		{
			try
			{
				dl.DeleteStation(station.Code);
			}
			catch (DO.BadStationCodeException e)
			{
				throw new BO.BadStationCodeException(station.Code, e.Message);
			}
		}

		public void DeleteAllStations()
		{
			dl.DeleteAllStations();
		}

		public IEnumerable<BO.Station> GetAllStations()
		{
			return (from doStation in dl.GetAllStations()
					select StationDoBoAdapter(doStation));
		}

		public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
		{
			IEnumerable<DO.Station> doStations = dl.GetStationsBy(doStation => predicate(StationDoBoAdapter(doStation)));

			return (from doStation in doStations
					select StationDoBoAdapter(doStation));
		}

		public BO.Station GetStation(int code)
		{
			throw new NotImplementedException();
		}

		public void UpdateStation(BO.Station station)
		{
			try
            {
				dl.UpdateStation(StationBoDoAdapter(station));
            }
			catch (DO.BadStationCodeException e)
            {
				throw new BO.BadStationCodeException(station.Code, e.Message);
            }
		}

		public void UpdateStation(int code, Action<BO.Station> update)
		{
			throw new NotImplementedException();
		}

		public void ValidateStationCode(int code)
		{
			try
			{
				dl.GetStation(code);
				throw new BO.BadStationCodeException(code, "station with this code already exists");
			}
			catch (DO.BadStationCodeException)
			{
			}
		}
		public void ValidateStationName(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new BO.BadStationNameException(name, "name cannot be empty");
		}
		public void ValidateStationAddress(string address)
		{
			if (string.IsNullOrEmpty(address))
				throw new BO.BadStationAddressException(address, "address cannot be empty");
		}
		public void ValidateStationLocation(BO.Location location)
		{
			if (location.Latitude < -90 || location.Latitude > 90)
				throw new BO.BadStationLocationException(location, "Latitude should be between -90 and 90");
			if (location.Longitude < -180 || location.Longitude > 180)
				throw new BO.BadStationLocationException(location, "Longitude should be between - 180 and 180");
		}
		#endregion

		#region Bus
		private BO.Bus BusDoBoAdapter(DO.Bus doBus)
		{
			BO.Bus result = (BO.Bus)doBus.CopyPropertiesToNew(typeof(BO.Bus));
			result.Registration = new BO.Registration(doBus.RegNum, doBus.RegDate);

			return result;
		}
		private DO.Bus BusBoDoAdapter(BO.Bus boBus)
		{
			DO.Bus result = (DO.Bus)boBus.CopyPropertiesToNew(typeof(DO.Bus));
			result.RegNum = boBus.Registration.Number;
			result.RegDate = boBus.Registration.Date;

			return result;
		}
		public void AddBus(BO.Bus bus)
		{
			ValidateRegistration(bus.Registration);
			dl.AddBus(BusBoDoAdapter(bus));
		}

		public IEnumerable<BO.Bus> GetAllBuses()
		{
			return from doBus in dl.GetAllBuses() select BusDoBoAdapter(doBus);
		}

		public void DeleteListOfBuses(IEnumerable<BO.Bus> buses)
		{
			foreach (BO.Bus bus in buses)
				dl.DeleteBus(bus.Registration.Number);
		}

		public void ValidateRegistration(BO.Registration registration)
		{
			if (!(registration.Date.Year >= 2018 && (registration.Number < 100000000 && registration.Number > 9999999) ||
					registration.Date.Year < 2018 && (registration.Number < 10000000 && registration.Number > 999999)))
				throw new BO.BadBusRegistrationException(registration, "bus registration number doesn't match the registration year");

			try
			{
				dl.GetBus(registration.Number);
				throw new BO.BadBusRegistrationException(registration, "bus with this registration number already exists");
			}
			catch (DO.BadBusRegistrationException)
			{
			}
		}

		public void RefuelBus(BO.Bus bus)
		{
			bus.Status = BO.BusStatus.Refueling;
			bus.FuelLeft = 1200;
			dl.UpdateBus(BusBoDoAdapter(bus));
		}

		public void DeleteAllBuses()
		{
			dl.DeleteAllBuses();
		}

		public void DeleteBus(BO.Bus bus)
		{
			try
			{
				dl.DeleteBus(bus.Registration.Number);
			}
			catch(DO.BadBusRegistrationException e)
			{
				throw new BO.BadBusRegistrationException(bus.Registration, e.Message);
			}
		}
		#endregion

		#region BusLine
		
		#endregion
	}
}
