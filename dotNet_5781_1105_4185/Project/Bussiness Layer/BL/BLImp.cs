﻿using System;
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
        private BO.AdjacentStations AdjacentsDoToBo(DO.AdjacentStations doAdjacntStations)
        {
            BO.AdjacentStations result = (BO.AdjacentStations)doAdjacntStations.CopyPropertiesToNew(typeof(BO.AdjacentStations));
            result.Station1 = GetStationWithoutAdjacents(doAdjacntStations.Station1Code);
            result.Station2 = GetStationWithoutAdjacents(doAdjacntStations.Station2Code);

            return result;
        }
        private DO.AdjacentStations AdjacentsBoToDo(BO.AdjacentStations boAdjacntStations)
        {
            DO.AdjacentStations result = (DO.AdjacentStations)boAdjacntStations.CopyPropertiesToNew(typeof(DO.AdjacentStations));
            result.Station1Code = boAdjacntStations.Station1.Code;
            result.Station2Code = boAdjacntStations.Station2.Code;

            return result;
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

            var doAdjacents = dl.GetAdjacentStationsBy(
                adjacent => adjacent.Station1Code == result.Code ||
                            adjacent.Station2Code == result.Code);
            result.AdjacentStations = from doAdjacent in doAdjacents
                                      select AdjacentsDoToBo(doAdjacent);

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
            ValidateNewStation(station);

            dl.AddStation(StationBoToDo(station));
            foreach (var ad in station.AdjacentStations)
            {
                dl.AddAdjacentStations(AdjacentsBoToDo(ad));
            }
        }

        public void DeleteStation(int code)
        {
            try
            {
                // Removing start and end stations from buslines.
                var lines = dl.GetBusLinesBy(l => l.StartStationCode == code || l.EndStationCode == code).ToArray();
                foreach (var line in lines)
                {
                    line.HasFullRoute = false;

                    // Start station and end station could be the same.
                    if (line.StartStationCode == code)
                        line.StartStationCode = null;
                    if (line.EndStationCode == code)
                        line.EndStationCode = null;
                    dl.UpdateBusLine(line);
                }

                dl.DeleteAdjacentStationsBy(a =>
                    a.Station1Code == code ||
                    a.Station2Code == code);
                DeleteLineStationsBy(ls => ls.StationCode == code);
                dl.DeleteStation(code);

                UpdateAllBusLinesFullRoute();
            }
            catch (DO.BadStationCodeException e)
            {
                throw new BO.BadStationCodeException(code, e.Message);
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
            dl.DeleteAllLineStations();
            dl.DeleteAllStations();
        }

        public IEnumerable<BO.Station> GetRestOfStations(IEnumerable<BO.Station> stations)
        {
            var restStations = dl.GetStationsBy(st => !stations.Any(st2 => st.Code == st2.Code));

            return from st in restStations select StationDoToBoWithoutAdjacents(st);
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
                dl.DeleteAdjacentStationsBy(a =>
                    a.Station1Code == station.Code ||
                    a.Station2Code == station.Code);

                foreach (var ad in station.AdjacentStations)
                {
                    dl.AddAdjacentStations(AdjacentsBoToDo(ad));
                }

                dl.UpdateStation(StationBoToDo(station));

                UpdateAllBusLinesFullRoute();
            }
            catch (DO.BadStationCodeException e)
            {
                throw new BO.BadStationCodeException(station.Code, e.Message);
            }
        }

        public void ValidateNewStation(BO.Station station)
        {
            ValidateNewStationCode(station.Code);
            ValidateNewStationName(station.Name);
            ValidateNewStationAddress(station.Address);
            ValidateNewStationLatitude(station.Location);
            ValidateNewStationLongitude(station.Location);
        }

        public void ValidateNewStationCode(int code)
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
        public void ValidateNewStationName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new BO.BadStationNameException(name, "name cannot be empty");
        }
        public void ValidateNewStationAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new BO.BadStationAddressException(address, "address cannot be empty");
        }
        public void ValidateNewStationLatitude(BO.Location location)
        {
            if (location.Latitude < -90 || location.Latitude > 90)
                throw new BO.BadLocationLatitudeException(location, "Latitude should be between -90 and 90");
        }
        public void ValidateNewStationLongitude(BO.Location location)
        {
            if (location.Longitude < -180 || location.Longitude > 180)
                throw new BO.BadLocationLongitudeException(location, "Longitude should be between - 180 and 180");
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

        #region BusLine, LineStation, LineTrip
        private BO.LineStation LineStationDoToBo(DO.LineStation doLineStation, BO.LineStation prevStation = null)
        {
            if (prevStation != null)
            {
                try
                {
                    var doAdjStations = dl.GetAdjacentStations(prevStation.Station.Code, doLineStation.StationCode);
                    prevStation.NextStationRoute = new BO.NextStationRoute() { Distance = doAdjStations.Distance, DrivingTime = doAdjStations.DrivingTime, };
                }
                catch (DO.BadAdjacentStationsCodeException) { }
            }
            return new BO.LineStation
            {
                Station = GetStationWithoutAdjacents(doLineStation.StationCode),
                NextStationRoute = null,
            };
        }

        private BO.LineTrip LineTripDoToBo(DO.LineTrip doLineTrip)
        {
            var result = new BO.LineTrip { StartTime = doLineTrip.StartTime };
            if (doLineTrip.Frequency != null && doLineTrip.FinishTime != null)
            {
                result.Frequncied = new BO.FrequnciedTrip
                {
                    Frequency = doLineTrip.Frequency ?? TimeSpan.Zero,
                    FinishTime = doLineTrip.FinishTime ?? DateTime.Now
                };
            }

            return result;
        }

        private DO.LineTrip LineTripBoToDo(BO.LineTrip boLineTrip, Guid lineID)
        {
            return new DO.LineTrip
            {
                LineID = lineID,
                StartTime = boLineTrip.StartTime,
                Frequency = boLineTrip.Frequncied?.Frequency,
                FinishTime = boLineTrip.Frequncied?.FinishTime,
            };
        }

        private BO.BusLine BusLineDoToBoWithoutFullRouteAndTrips(DO.BusLine doBusLine)
        {
            var result = (BO.BusLine)doBusLine.CopyPropertiesToNew(typeof(BO.BusLine));
            var startEnd = new List<BO.LineStation>();

            if (doBusLine.StartStationCode != null)
            {
                var doStart = dl.GetLineStationByStation(result.ID, doBusLine.StartStationCode ?? 0);
                startEnd.Add(LineStationDoToBo(doStart));
            }
            if (doBusLine.EndStationCode != null)
            {
                var doEnd = dl.GetLineStationByStation(result.ID, doBusLine.EndStationCode ?? 0);
                startEnd.Add(LineStationDoToBo(doEnd));
            }

            result.Route = startEnd;

            return result;
        }

        private BO.BusLine BusLineDoToBo(DO.BusLine doBusLine)
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

            busLine.Trips = from lt in dl.GetLineTripsBy(lt => lt.LineID == busLine.ID)
                            select LineTripDoToBo(lt);

            return busLine;
        }

        private DO.BusLine BusLineBoToDo(BO.BusLine boBusLine)
        {
            var result = (DO.BusLine)boBusLine.CopyPropertiesToNew(typeof(DO.BusLine));
            result.RouteLength = boBusLine.Route.Count();
            if (boBusLine.Route.Count() > 0)
            {
                result.HasFullRoute = !boBusLine.Route.Where(b => b != boBusLine.Route.Last()).Any(ls => ls.NextStationRoute == null);
                result.StartStationCode = boBusLine.Route.First().Station.Code;
                result.EndStationCode = boBusLine.Route.Last().Station.Code;
            }

            return result;
        }

        private struct RouteAdjacentStations
        {
            public BO.LineStation left;
            public BO.LineStation right;
        }
        private IEnumerable<RouteAdjacentStations> GetRouteAdjacentStations(IEnumerable<BO.LineStation> stations)
        {

            return (from left in stations
                    where left != stations.Last()
                    select left
                    ).Zip(
                    from right in stations
                    where right != stations.First()
                    select right,
                    (left, right) => new RouteAdjacentStations { left = left, right = right });
        }

        public IEnumerable<BO.BusLine> GetAllBusLinesWithoutFullRouteAndTrips()
        {
            return from doBusLine in dl.GetAllBusLines()
                   select BusLineDoToBoWithoutFullRouteAndTrips(doBusLine);
        }

        public IEnumerable<BO.BusLine> GetAllBusLines()
        {
            return from doBusLine in dl.GetAllBusLines()
                   select BusLineDoToBo(doBusLine);
        }

        public IEnumerable<BO.BusLine> GetLinesPassingTheStation(int code)
        {
            var busLinesIDs = (from st in dl.GetLineStationsBy(st => st.StationCode == code)
                               select st.LineID).Distinct();

            return from busLine in dl.GetBusLinesBy(
                   bl => bl.HasFullRoute &&
                         busLinesIDs.Any(id => bl.ID == id))
                   select BusLineDoToBoWithoutFullRouteAndTrips(busLine);
        }

        public BO.BusLine GetBusLine(Guid ID)
        {
            try
            {
                var doBusLine = dl.GetBusLine(ID);
                return BusLineDoToBo(doBusLine);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public BO.BusLine GetBusLineWithoutRouteAndTrips(Guid ID)
        {
            try
            {
                var doBusLine = dl.GetBusLine(ID);
                return BusLineDoToBoWithoutFullRouteAndTrips(doBusLine);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public BO.BusLine DuplicateBusLine(Guid ID)
        {
            try
            {
                var busLine = dl.GetBusLine(ID);
                busLine.ID = Guid.NewGuid();
                dl.AddBusLine(busLine);

                var lineStations = from st in dl.GetLineStationsBy(ls => ls.LineID == ID)
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

                return BusLineDoToBoWithoutFullRouteAndTrips(busLine);
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

        public void AddBusLine(BO.BusLine busLine)
        {
            try
            {
                dl.AddBusLine(BusLineBoToDo(busLine));

                if (busLine.Route.Count() == 0) return;

                int index = 0;
                busLine.Route.Last().NextStationRoute = null;

                foreach (var routeAdj in GetRouteAdjacentStations(busLine.Route))
                {
                    dl.AddLineStation(new DO.LineStation()
                    {
                        LineID = busLine.ID,
                        RouteIndex = index++,
                        StationCode = routeAdj.left.Station.Code,
                    });

                    if (routeAdj.left.NextStationRoute != null)
                    {
                        var adj = new DO.AdjacentStations()
                        {
                            Station1Code = routeAdj.left.Station.Code,
                            Station2Code = routeAdj.right.Station.Code,
                            Distance = routeAdj.left.NextStationRoute?.Distance ?? 0,
                            DrivingTime = routeAdj.left.NextStationRoute?.DrivingTime ?? TimeSpan.Zero
                        };

                        dl.AddOrUpdateAdjacentStations(adj);
                    }
                    else
                    {
                        try
                        {
                            dl.DeleteAdjacentStations(routeAdj.left.Station.Code, routeAdj.right.Station.Code);
                        }
                        catch (DO.BadAdjacentStationsCodeException) { }
                    }
                }

                dl.AddLineStation(new DO.LineStation()
                {
                    LineID = busLine.ID,
                    RouteIndex = index++,
                    StationCode = busLine.Route.Last().Station.Code,
                });

                UpdateAllBusLinesFullRoute();
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(busLine.ID, e.Message);
            }
        }

        public void UpdateBusLine(BO.BusLine busLine)
        {
            DeleteBusLine(busLine.ID);
            AddBusLine(busLine);
        }

        public void DeleteBusLine(Guid ID)
        {
            try
            {
                dl.DeleteLineStationsBy(ls => ls.LineID == ID);
                dl.DeleteBusLine(ID);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        public void DeleteAllBusLines()
        {
            dl.DeleteAllLineStations();
            dl.DeleteAllBusLines();
        }

        public IEnumerable<BO.LineStation> ReverseLineStations(IEnumerable<BO.LineStation> stations)
        {
            var reversed = stations.Reverse();

            foreach (var routeAdj in GetRouteAdjacentStations(reversed))
            {
                routeAdj.left.NextStationRoute = routeAdj.right.NextStationRoute;
            }
            reversed.Last().NextStationRoute = null;
            return reversed;
        }
        private void UpdateBusLineFullRoute(Guid ID)
        {
            try
            {
                DO.BusLine line = dl.GetBusLine(ID);

                DO.LineStation prevLs = null;
                int i = 0;
                for (; i < line.RouteLength; i++)
                {
                    var ls = dl.GetLineStationByIndex(line.ID, i);

                    if (i == 0)
                    {
                        prevLs = ls;
                        continue;
                    }

                    try
                    {
                        dl.GetAdjacentStations(ls.StationCode, prevLs.StationCode);
                    }
                    catch (DO.BadAdjacentStationsCodeException)
                    {
                        line.HasFullRoute = false;
                        break;
                    }

                    prevLs = ls;
                }

                // If all the line staions in the route are adjacents.
                // set to have full route.
                if (i == line.RouteLength)
                {
                    line.HasFullRoute = line.RouteLength > 1;
                }

                dl.UpdateBusLine(line);
            }
            catch (DO.BadBusLineIDException e)
            {
                throw new BO.BadBusLineIDException(ID, e.Message);
            }
        }

        private void UpdateAllBusLinesFullRoute()
        {
            var ids = (from line in dl.GetAllBusLines() select line.ID).ToArray();
            foreach (var id in ids)
            {
                UpdateBusLineFullRoute(id);
            }
        }

        public void DeleteLineStationsBy(Predicate<DO.LineStation> predicate)
        {
            foreach (var toDelete in dl.GetLineStationsBy(predicate).ToArray())
            {
                dl.DeleteLineStationByIndex(toDelete.LineID, toDelete.RouteIndex);

                var doLine = dl.GetBusLine(toDelete.LineID);
                for (int i = toDelete.RouteIndex + 1; i < doLine.RouteLength; i++)
                {
                    var doLs = dl.GetLineStationByIndex(toDelete.LineID, i);
                    doLs.RouteIndex -= 1;
                    dl.UpdateLineStationByIndex(doLs);
                }
                doLine.RouteLength -= 1;
                dl.UpdateBusLine(doLine);
            }
        }

        public IEnumerable<BO.LineTrip> CollidingTrips(IEnumerable<BO.LineTrip> trips)
        {
            var pairs = trips.Zip(trips.Skip(1), (a,b) => new BO.LineTrip[]{ a, b });

            var colliding = from pair in pairs
                            where TripsColliding(pair[0], pair[1])
                            from trip in pair
                            select trip;

            return colliding.Distinct();
        }

        private bool TripsColliding(BO.LineTrip a, BO.LineTrip b)
        {
            return false;
                //// bStart < aStart < bFinish
                //a.StartTime < b.FinishTime && a.StartTime > b.StartTime ||
                //// bStart < aFinish < bFinish
                //a.FinishTime < b.FinishTime && a.FinishTime > b.StartTime ||
                //// aStart < bStart < aFinish
                //b.StartTime < a.FinishTime && b.StartTime > a.StartTime ||
                //// aStart < bFinish < aFinish
                //b.FinishTime < a.FinishTime && b.FinishTime > a.StartTime;
        }
        #endregion
    }
}
