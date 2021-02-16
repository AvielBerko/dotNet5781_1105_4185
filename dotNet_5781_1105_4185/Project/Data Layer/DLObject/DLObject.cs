using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;
using DS;

namespace DL
{
    sealed class DLObject : IDL
    {
        #region Singleton
        static readonly Lazy<DLObject> lazy = new Lazy<DLObject>(() => new DLObject());
        public static DLObject Instance => lazy.Value;

        private DLObject() { }
        #endregion

        #region User
        public DO.User GetUser(string name)
        {
            var user = DataSet.Users.Find(u => u.Name == name);

            if (user == null) throw new BadUserNameException(name, $"User with the name {name} not found");

            return user.Clone();
        }

        public void AddUser(DO.User user)
        {
            if (DataSet.Users.Any(u => u.Name == user.Name))
                throw new BadUserNameException(user.Name, $"User with the name {user.Name} already exists");

            DataSet.Users.Add(user.Clone());
        }

        public void DeleteUser(DO.User user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return from st in DataSet.Stations select st.Clone();
        }

        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            return from st in DataSet.Stations
                   where predicate(st)
                   select st.Clone();
        }

        public Station GetStation(int code)
        {
            var station = DataSet.Stations.Find(s => s.Code == code);
            if (station == null) throw new BadStationCodeException(code, $"No station with code {code}");

            return station.Clone();
        }

        public void AddStation(Station station)
        {
            if (DataSet.Stations.Any(s => s.Code == station.Code))
                throw new BadStationCodeException(station.Code, $"Station with code {station.Code} already exists");

            DataSet.Stations.Add(station.Clone());
        }

        public void UpdateStation(Station station)
        {
            var exists = DataSet.Stations.Find(s => s.Code == station.Code);
            if (exists == null) throw new BadStationCodeException(station.Code, $"No station with code {station.Code}");

            DataSet.Stations.Remove(exists);
            DataSet.Stations.Add(station.Clone());
        }

        public void DeleteStation(int code)
        {
            var exists = DataSet.Stations.Find(s => s.Code == code);
            if (exists == null) throw new BadStationCodeException(code, $"No station with code {code}");

            DataSet.Stations.Remove(exists);
        }

        public void DeleteAllStations()
        {
            DataSet.Stations.Clear();
        }

        public void DeleteStationsBy(Predicate<Station> predicate)
        {
            DataSet.Stations.RemoveAll(predicate);
        }
        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            return from b in DataSet.Buses select b.Clone();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            return from b in DataSet.Buses
                   where predicate(b)
                   select b.Clone();
        }

        public Bus GetBus(int regNum)
        {
            var bus = DataSet.Buses.Find(b => b.RegNum == regNum);
            if (bus == null) throw new BadBusRegistrationException(regNum, $"No bus with registration number {regNum}");

            return bus.Clone();
        }

        public void AddBus(Bus bus)
        {
            if (DataSet.Buses.Any(b => b.RegNum == bus.RegNum))
                throw new BadBusRegistrationException(bus.RegNum, $"Bus with registration number {bus.RegNum} already exists");

            // Validates registration length by date.
            if (!(bus.RegDate.Year >= 2018 && (bus.RegNum < 100000000 && bus.RegNum > 9999999) ||
                    bus.RegDate.Year < 2018 && (bus.RegNum < 10000000 && bus.RegNum > 999999)))
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "Bus registration number doesn't match the registration year");

            DataSet.Buses.Add(bus.Clone());
        }

        public void UpdateBus(Bus bus)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == bus.RegNum);
            if (exists == null) throw new BadStationCodeException(bus.RegNum, $"No bus with registratin number {bus.RegNum}");

            // Validates registration length by date.
            if (!(bus.RegDate.Year >= 2018 && (bus.RegNum < 100000000 && bus.RegNum > 9999999) ||
                    bus.RegDate.Year < 2018 && (bus.RegNum < 10000000 && bus.RegNum > 999999)))
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "Bus registration number doesn't match the registration year");

            DataSet.Buses.Remove(exists);
            DataSet.Buses.Add(bus.Clone());
        }

        public void DeleteBus(int regNum)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == regNum);
            if (exists == null) throw new BadStationCodeException(regNum, $"No bus with registratin number {regNum}");

            DataSet.Buses.Remove(exists);
        }

        public void DeleteAllBuses()
        {
            DataSet.Buses.Clear();
        }

        public void DeleteBusesBy(Predicate<DO.Bus> predicate)
        {
            DataSet.Buses.RemoveAll(predicate);
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from l in DataSet.Lines select l.Clone();
        }

        public IEnumerable<BusLine> GetBusLinesBy(Predicate<BusLine> predicate)
        {
            return from l in DataSet.Lines
                   where predicate(l)
                   select l.Clone();
        }

        public BusLine GetBusLine(Guid ID)
        {
            var busLine = DataSet.Lines.Find(b => b.ID == ID);
            if (busLine == null) throw new BadBusLineIDException(ID, $"No bus line with ID {ID}");

            return busLine.Clone();
        }

        public void AddBusLine(BusLine busLine)
        {
            if (DataSet.Lines.Any(b => b.ID == busLine.ID))
                throw new BadBusLineIDException(busLine.ID, $"Bus line with ID {busLine.ID} already exists");

            // Checks for the start and end stations
            if (busLine.StartStationCode != null &&
                !DataSet.Stations.Any(st => st.Code == busLine.StartStationCode))
                throw new BadStationCodeException(busLine.StartStationCode ?? 0, $"BusLine's start station with code {busLine.StartStationCode} couldn't be found.");
            if (busLine.EndStationCode != null &&
                !DataSet.Stations.Any(st => st.Code == busLine.EndStationCode))
                throw new BadStationCodeException(busLine.EndStationCode ?? 0, $"BusLine's start station with code {busLine.EndStationCode} couldn't be found.");

            if (busLine.HasFullRoute &&
                (busLine.StartStationCode == null ||
                busLine.EndStationCode == null))
                throw new InvalidOperationException("BusLine without start or stop stations cannot have full route");

            DataSet.Lines.Add(busLine.Clone());
        }

        public void UpdateBusLine(BusLine busLine)
        {
            var exists = DataSet.Lines.Find(b => b.ID == busLine.ID);
            if (exists == null) throw new BadBusLineIDException(busLine.ID, $"No bus line with ID {busLine.ID}");

            // Checks for the start and end stations
            if (busLine.StartStationCode != null &&
                !DataSet.Stations.Any(st => st.Code == busLine.StartStationCode))
                throw new BadStationCodeException(busLine.StartStationCode ?? 0, $"BusLine's start station with code {busLine.StartStationCode} couldn't be found.");
            if (busLine.EndStationCode != null &&
                !DataSet.Stations.Any(st => st.Code == busLine.EndStationCode))
                throw new BadStationCodeException(busLine.EndStationCode ?? 0, $"BusLine's start station with code {busLine.EndStationCode} couldn't be found.");

            if (busLine.HasFullRoute &&
                (busLine.StartStationCode == null ||
                busLine.EndStationCode == null))
                throw new InvalidOperationException("BusLine without start or stop stations cannot have full route");

            DataSet.Lines.Remove(exists);
            DataSet.Lines.Add(busLine);
        }

        public void DeleteBusLine(Guid ID)
        {
            var busLine = DataSet.Lines.Find(b => b.ID == ID);
            if (busLine == null) throw new BadBusLineIDException(ID, $"No bus line with ID {ID}");

            DataSet.Lines.Remove(busLine);
        }

        public void DeleteAllBusLines()
        {
            DataSet.Lines.Clear();
        }

        public void DeleteBusLinesBy(Predicate<BusLine> predicate)
        {
            DataSet.Lines.RemoveAll(predicate);
        }
        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStations()
        {
            return from ls in DataSet.LineStations select ls.Clone();
        }

        public IEnumerable<LineStation> GetLineStationsBy(Predicate<LineStation> predicate)
        {
            return from ls in DataSet.LineStations
                   where predicate(ls)
                   select ls.Clone();
        }

        public LineStation GetLineStationByStation(Guid lineID, int stationCode)
        {
            var lineStations = GetLineStationsBy(ls => ls.LineID == lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"No line station for line with id {lineID}");

            var lineStation = lineStations.FirstOrDefault(b => b.StationCode == stationCode);

            if (lineStation == null) throw new BadLineStationStationCodeException(lineID, stationCode,
                $"No line station with station code {stationCode} found for line ID {lineID}");

            return lineStation;
        }

        public LineStation GetLineStationByIndex(Guid lineID, int index)
        {
            var lineStations = GetLineStationsBy(ls => ls.LineID == lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"No line station for line with id {lineID}");

            var lineStation = lineStations.FirstOrDefault(b => b.RouteIndex == index);

            if (lineStation == null) throw new BadLineStationIndexException(lineID, index,
                $"No line station with index {index} found for line ID {lineID}");

            return lineStation;
        }

        public void AddLineStation(LineStation lineStation)
        {
            // Checks for bus line and station.
            if (!DataSet.Lines.Any(l => l.ID == lineStation.LineID))
                throw new BadBusLineIDException(lineStation.LineID, $"Couldn't find bus line with id {lineStation.LineID}");
            if (!DataSet.Stations.Any(st => st.Code == lineStation.StationCode))
                throw new BadStationCodeException(lineStation.StationCode, $"Couldn't find station with code {lineStation.StationCode}");

            // Checks that the line doesn't have line station with the same station or index.
            var lineStations = from ls in DataSet.LineStations
                               where ls.LineID == lineStation.LineID
                               select ls;
            foreach (var ls in lineStations)
            {
                if (ls.StationCode == lineStation.StationCode)
                    throw new BadLineStationStationCodeException(lineStation.LineID, lineStation.StationCode,
                                    $"Bus line with id {lineStation.LineID} already has line station with station code {lineStation.StationCode}");
                if (ls.RouteIndex == lineStation.RouteIndex)
                    throw new BadLineStationIndexException(lineStation.LineID, lineStation.StationCode,
                                    $"Bus line with id {lineStation.LineID} already has line station with index {lineStation.RouteIndex}");
            }

            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void UpdateLineStationByStation(LineStation lineStation)
        {
            var exists = DataSet.LineStations.Find(ls => lineStation.LineID == ls.LineID && lineStation.StationCode == ls.StationCode);

            if (exists == null)
                throw new BadLineStationStationCodeException(lineStation.LineID, lineStation.StationCode);

            DataSet.LineStations.Remove(exists);
            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void UpdateLineStationByIndex(LineStation lineStation)
        {
            var exists = DataSet.LineStations.Find(ls => lineStation.LineID == ls.LineID && lineStation.RouteIndex == ls.RouteIndex);

            if (exists == null)
                throw new BadLineStationIndexException(lineStation.LineID, lineStation.RouteIndex);

            DataSet.LineStations.Remove(exists);
            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void DeleteLineStationByStation(Guid lineID, int stationCode)
        {
            var exists = GetLineStationByStation(lineID, stationCode);

            DataSet.LineStations.Remove(exists);
        }

        public void DeleteLineStationByStation(int stationCode)
        {
            var exists = from ls in DataSet.LineStations where (ls.StationCode == stationCode) select ls;

            foreach (var ex in exists.ToArray())
                DataSet.LineStations.Remove(ex);
        }

        public void DeleteLineStationByIndex(Guid lineID, int index)
        {
            var exists = GetLineStationByIndex(lineID, index);

            DataSet.LineStations.Remove(exists);
        }

        public void DeleteAllLineStations()
        {
            DataSet.LineStations.Clear();
        }

        public void DeleteLineStationsBy(Predicate<LineStation> predicate)
        {
            DataSet.LineStations.RemoveAll(predicate);
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            return from a in DataSet.AdjacentStations select a.Clone();
        }

        public IEnumerable<AdjacentStations> GetAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from a in DataSet.AdjacentStations
                   where predicate(a)
                   select a.Clone();
        }

        public AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            var adjacent = DataSet.AdjacentStations.Find(a => a.Station1Code == stationCode1 && a.Station2Code == stationCode2 || a.Station1Code == stationCode2 && a.Station2Code == stationCode1);
            if (adjacent == null)
            {
                throw new BadAdjacentStationsCodeException(stationCode1, stationCode2,
                $"Stations with code {stationCode1} and {stationCode2} are not adjacents");
            }

            return adjacent.Clone();
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            // Checks for stations.
            if (!DataSet.Stations.Any(st => st.Code == adjacentStations.Station1Code))
                throw new BadStationCodeException(adjacentStations.Station1Code, $"Couldn't find station with code {adjacentStations.Station1Code}");
            if (!DataSet.Stations.Any(st => st.Code == adjacentStations.Station2Code))
                throw new BadStationCodeException(adjacentStations.Station2Code, $"Couldn't find station with code {adjacentStations.Station2Code}");

            if (DataSet.AdjacentStations.Any(a =>
                    a.Station1Code == adjacentStations.Station1Code &&
                    a.Station2Code == adjacentStations.Station2Code))
                throw new BadAdjacentStationsCodeException(adjacentStations.Station1Code, adjacentStations.Station2Code, $"Adjacent stations already exists");

            DataSet.AdjacentStations.Add(adjacentStations.Clone());
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            var exists = DataSet.AdjacentStations.Find(a => a.Station1Code == adjacentStations.Station1Code && a.Station2Code == adjacentStations.Station2Code);
            if (exists == null) throw new BadAdjacentStationsCodeException(adjacentStations.Station1Code, adjacentStations.Station2Code, $"No adjacent stations matches these stations");

            DataSet.AdjacentStations.Remove(exists);
            DataSet.AdjacentStations.Add(adjacentStations.Clone());
        }

        public void AddOrUpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            try
            {
                AddAdjacentStations(adjacentStations);
            }
            catch (BadAdjacentStationsCodeException)
            {
                UpdateAdjacentStations(adjacentStations);
            }
        }

        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            var adjacent = DataSet.AdjacentStations.Find(a => a.Station1Code == stationCode1 && a.Station2Code == stationCode2);
            if (adjacent == null) throw new BadAdjacentStationsCodeException(stationCode1, stationCode2, $"No adjacent stations matches these stations");

            DataSet.AdjacentStations.Remove(adjacent);
        }

        public void DeleteAllAdjacentStations()
        {
            DataSet.AdjacentStations.Clear();
        }

        public void DeleteAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            DataSet.AdjacentStations.RemoveAll(predicate);
        }
        #endregion

        #region LineTrip
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from lt in DataSet.LineTrips select lt.Clone();
        }

        public IEnumerable<LineTrip> GetLineTripsBy(Predicate<LineTrip> predicate)
        {
            return from lt in DataSet.LineTrips
                   where predicate(lt)
                   select lt.Clone();
        }

        public LineTrip GetLineTrip(Guid lineID, TimeSpan startTime)
        {
            var lineTrip = DataSet.LineTrips.Find(lt => lt.LineID == lineID && lt.StartTime == startTime);
            if (lineTrip == null) throw new BadLineTripIDAndTimeException(
                lineID, startTime, $"No line trip for line with id {lineID} starts at {startTime}");

            return lineTrip.Clone();
        }

        public void AddLineTrip(LineTrip lineTrip)
        {
            if (DataSet.LineTrips.Any(lt => lt.LineID == lineTrip.LineID && lt.StartTime == lineTrip.StartTime))
                throw new BadLineTripIDAndTimeException(lineTrip.LineID, lineTrip.StartTime,
                    $"Line trip for line with id {lineTrip.LineID} that starts at {lineTrip.StartTime} already exists");
            if (!DataSet.Lines.Any(bl => bl.ID == lineTrip.LineID))
                throw new BadBusLineIDException(lineTrip.LineID, $"No bus line with ID {lineTrip.LineID}");
            // XOR on bools, checks if one of them is null.
            if (lineTrip.Frequency != null ^ lineTrip.FinishTime != null)
                throw new BadLineTripFrequencyAndFinishTime(lineTrip,
                    $"Finish time of line trip have to be set only if frequency is set");

            DataSet.LineTrips.Add(lineTrip.Clone());
        }

        public void UpdateLineTrip(LineTrip lineTrip)
        {
            var exists = DataSet.LineTrips.Find(lt => lt.LineID == lineTrip.LineID && lt.StartTime == lineTrip.StartTime);
            if (exists == null) throw new BadLineTripIDAndTimeException(lineTrip.LineID, lineTrip.StartTime,
                $"No line trip for line with id {lineTrip.LineID} starts at {lineTrip.StartTime}");

            if (!DataSet.Lines.Any(bl => bl.ID == lineTrip.LineID))
                throw new BadBusLineIDException(lineTrip.LineID, $"No bus line with ID {lineTrip.LineID}");
            // XOR on bools, checks if one of them is null.
            if (lineTrip.Frequency != null ^ lineTrip.FinishTime != null)
                throw new BadLineTripFrequencyAndFinishTime(lineTrip,
                    $"Finish time of line trip have to be set only if frequency is set");

            DataSet.LineTrips.Remove(exists);
            DataSet.LineTrips.Add(lineTrip.Clone());
        }

        public void DeleteLineTrip(Guid lineID, TimeSpan startTime)
        {
            var exists = DataSet.LineTrips.Find(lt => lt.LineID == lineID);
            if (exists == null) throw new BadLineTripIDAndTimeException(
                lineID, startTime, $"No line trip for line with id {lineID} starts at {startTime}");

            DataSet.LineTrips.Remove(exists);
        }

        public void DeleteAllLineTrips()
        {
            DataSet.LineTrips.Clear();
        }

        public void DeleteLineTripsBy(Predicate<LineTrip> predicate)
        {
            DataSet.LineTrips.RemoveAll(predicate);
        }
        #endregion
    }
}
