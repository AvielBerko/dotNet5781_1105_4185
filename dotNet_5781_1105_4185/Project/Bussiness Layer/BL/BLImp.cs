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

        #region AdjacentStation
        private BO.AdjacentStation AdjacentDoToBo(DO.AdjacentStations doAdjacntStation, int toStationCode)
        {
            BO.AdjacentStation result = (BO.AdjacentStation)doAdjacntStation.CopyPropertiesToNew(typeof(BO.AdjacentStation));
            result.ToStation = GetStationWithoutAdjacents(toStationCode);

            return result;
        }
        private DO.AdjacentStations AdjacentBoToDo(BO.AdjacentStation boAdjacntStation, int fromStationCode)
        {
            DO.AdjacentStations result = (DO.AdjacentStations)boAdjacntStation.CopyPropertiesToNew(typeof(DO.AdjacentStations));
            result.Station1Code = fromStationCode;
            result.Station2Code = boAdjacntStation.ToStation.Code;

            return result;
        }

        public void DeleteAdjacent(BO.AdjacentStation adjacent, int fromStationCode)
        {
            try
            {
                dl.DeleteAdjacentStations(fromStationCode, adjacent.ToStation.Code);
            }
            catch (DO.BadAdjacentStationsCodeException e)
            {
                throw new BO.BadAdjacentStationsCodeException(fromStationCode, adjacent.ToStation.Code, e.Message);
            }
        }

        public IEnumerable<BO.Station> GetRestOfStations(IEnumerable<BO.Station> stations)
        {
            var restStations = dl.GetStationsBy(st => !stations.Any(st2 => st.Code == st2.Code));

            return from st in restStations select StationDoToBoWithoutAdjacents(st);
        }
        #endregion

        #region Station
        private BO.Station StationDoToBoWithoutAdjacents(DO.Station doStation)
        {
            BO.Station result = (BO.Station)doStation.CopyPropertiesToNew(typeof(BO.Station));
            result.Location = new BO.Location(doStation.Longitude, doStation.Latitude);

            return result;
        }
        private BO.Station StationDoToBo(DO.Station doStation)
        {
            BO.Station result = StationDoToBoWithoutAdjacents(doStation);

            var doAdjacents = dl.GetAdjacentStationsBy(adjacent => adjacent.Station1Code == result.Code || adjacent.Station2Code == result.Code);
            result.AdjacentStations = from doAdjacent in doAdjacents select AdjacentDoToBo(doAdjacent, doAdjacent.Station1Code == result.Code ? doAdjacent.Station2Code : doAdjacent.Station1Code);

            return result;
        }
        private DO.Station StationBoToDo(BO.Station boStation)
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

            dl.AddStation(StationBoToDo(station));
            foreach (var ad in station.AdjacentStations)
                dl.AddAdjacentStations(AdjacentBoToDo(ad, station.Code));
        }

        public void DeleteStation(BO.Station station)
        {
            try
            {
                // Removing start and end stations from buslines.
                var lines = dl.GetBusLinesBy(l => l.StartStationCode == station.Code || l.EndStationCode == station.Code).ToArray();
                foreach (var line in lines)
                {
                    line.HasFullRoute = false;
                    if (line.StartStationCode == station.Code)
                        line.StartStationCode = null;
                    else
                        line.EndStationCode = null;
                    dl.UpdateBusLine(line);
                }

                dl.DeleteStationAdjacents(station.Code);
                dl.DeleteLineStationByStation(station.Code);
                dl.DeleteStation(station.Code);
            }
            catch (DO.BadStationCodeException e)
            {
                throw new BO.BadStationCodeException(station.Code, e.Message);
            }
        }

        public void DeleteAllStations()
        {
            foreach (var line in dl.GetAllBusLines().ToArray())
            {
                line.HasFullRoute = false;
                line.StartStationCode = null;
                line.EndStationCode = null;

                dl.UpdateBusLine(line);
            }

            dl.DeleteAllAdjacentStations();
            dl.DeleteLineStationsBy(ls => true);
            dl.DeleteAllStations();
        }

        public IEnumerable<BO.Station> GetAllStationsWithoutAdjacents()
        {
            return (from doStation in dl.GetAllStations()
                    select StationDoToBoWithoutAdjacents(doStation));
        }
        public IEnumerable<BO.Station> GetAllStations()
        {
            return (from doStation in dl.GetAllStations()
                    select StationDoToBo(doStation));
        }

        public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        {
            IEnumerable<DO.Station> doStations = dl.GetStationsBy(doStation => predicate(StationDoToBoWithoutAdjacents(doStation)));

            return (from doStation in doStations
                    select StationDoToBoWithoutAdjacents(doStation));
        }

        public BO.Station GetStationWithoutAdjacents(int code)
        {
            try
            {
                return StationDoToBoWithoutAdjacents(dl.GetStation(code));
            }
            catch (DO.BadStationCodeException e)
            {
                throw new BO.BadStationCodeException(code, e.Message);
            }
        }
        public BO.Station GetStation(int code)
        {
            try
            {
                return StationDoToBo(dl.GetStation(code));
            }
            catch (DO.BadStationCodeException e)
            {
                throw new BO.BadStationCodeException(code, e.Message);
            }
        }

        public void UpdateStation(BO.Station station)
        {
            try
            {
                dl.DeleteStationAdjacents(station.Code);
                foreach (var ad in station.AdjacentStations)
                    dl.AddAdjacentStations(AdjacentBoToDo(ad, station.Code));
                dl.UpdateStation(StationBoToDo(station));
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
            catch (DO.BadBusRegistrationException e)
            {
                throw new BO.BadBusRegistrationException(bus.Registration, e.Message);
            }
        }
        #endregion

        #region BusLine, LineStation
        BO.LineStation LineStationDoToBo(DO.LineStation doLineStation, BO.LineStation lastStation = null)
        {
            if (lastStation != null)
            {
                try
                {
                    var doAdjStations = dl.GetAdjacentStations(lastStation.Station.Code, doLineStation.StationCode);
                    return new BO.LineStation
                    {
                        Station = GetStationWithoutAdjacents(doLineStation.StationCode),
                        LastStationRoute = new BO.LastStationRoute
                        {
                            Distance = doAdjStations.Distance,
                            DrivingTime = doAdjStations.DrivingTime,
                        },
                    };
                }
                catch (DO.BadAdjacentStationsCodeException)
                {
                    return new BO.LineStation
                    {
                        Station = GetStationWithoutAdjacents(doLineStation.StationCode),
                        LastStationRoute = null,
                    };
                }
            }

            return new BO.LineStation
            {
                Station = GetStationWithoutAdjacents(doLineStation.StationCode),
                LastStationRoute = null,
            };
        }

        private BO.BusLine BusLineDoToBoWithoutFullRoute(DO.BusLine doBusLine)
        {
            var busLine = (BO.BusLine)doBusLine.CopyPropertiesToNew(typeof(BO.BusLine));
            var startEnd = new List<BO.LineStation>();

            if (doBusLine.StartStationCode != null)
            {
                var doStart = dl.GetLineStationByStation(busLine.ID, doBusLine.StartStationCode ?? 0);
                startEnd.Add(LineStationDoToBo(doStart));
            }
            if (doBusLine.EndStationCode != null)
            {
                var doEnd = dl.GetLineStationByStation(busLine.ID, doBusLine.EndStationCode ?? 0);
                startEnd.Add(LineStationDoToBo(doEnd));
            }

            busLine.Route = startEnd;

            return busLine;
        }

        BO.BusLine BusLineDoToBo(DO.BusLine doBusLine)
        {
            var busLine = (BO.BusLine)doBusLine.CopyPropertiesToNew(typeof(BO.BusLine));

            List<BO.LineStation> route = new List<BO.LineStation>();
            for (int i = 0; i < doBusLine.RouteLength; i++)
            {
                var doLineStation = dl.GetLineStationByIndex(busLine.ID, i);
                if (i == 0)
                {
                    route.Add(LineStationDoToBo(doLineStation));
                }
                else
                {
                    route.Add(LineStationDoToBo(doLineStation, route[i - 1]));
                }
            }
            busLine.Route = route;

            return busLine;
        }

        public IEnumerable<BO.BusLine> GetAllBusLinesWithoutFullRoute()
        {
            return from doBusLine in dl.GetAllBusLines()
                   select BusLineDoToBoWithoutFullRoute(doBusLine);
        }

        public IEnumerable<BO.BusLine> GetAllBusLines()
        {
            return from doBusLine in dl.GetAllBusLines()
                   select BusLineDoToBo(doBusLine);
        }

        public IEnumerable<BO.BusLine> GetLinesPassingTheStation(int code)
        {
            var busLinesIDs = (from st in dl.GetLineStationsBy(st => st.StationCode == code) select st.LineID).Distinct();

            return from busLine in dl.GetBusLinesBy(bl => bl.HasFullRoute && busLinesIDs.Any(id => bl.ID == id)) select BusLineDoToBoWithoutFullRoute(busLine);
        }

        public BO.BusLine DuplicateBusLine(Guid ID)
        {
            try
            {
                var busLine = dl.GetBusLine(ID);
                busLine.ID = Guid.NewGuid();
                dl.AddBusLine(busLine);

                var lineStations = from st in dl.GetAllLineStations(ID)
                                   select new DO.LineStation
                                   {
                                       LineID = busLine.ID,
                                       RouteIndex = st.RouteIndex,
                                       StationCode = st.StationCode,
                                   };

                foreach (var ls in lineStations.ToArray())
                {
                    dl.AddLineStation(ls);
                }

                return BusLineDoToBoWithoutFullRoute(busLine);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public bool BusLineHasFullRoute(Guid ID)
        {
            try
            {
                return dl.GetBusLine(ID).HasFullRoute;
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public void DeleteBusLine(Guid ID)
        {
            try
            {
                dl.DeleteAllLineStations(ID);
                dl.DeleteBusLine(ID);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public void DeleteAllBusLines()
        {
            dl.DeleteLineStationsBy(st => true);
            dl.DeleteAllBusLines();
        }
        #endregion
    }
}
