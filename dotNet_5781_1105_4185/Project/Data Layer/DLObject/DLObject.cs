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
            return from station in DataSet.Stations select station.Clone();
        }
        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            return from station in DataSet.Stations
                   let cloned = station.Clone()
                   where predicate(cloned)
                   select cloned;
        }

        public Station GetStation(int code)
        {
            var station = DataSet.Stations.Find(s => s.Code == code);

            if (station == null) throw new BadStationCodeException(code, $"no station with code {code}");

            return station.Clone();
        }

        public void AddStation(Station station)
        {
            if (DataSet.Stations.Any(s => s.Code == station.Code))
                throw new BadStationCodeException(station.Code, $"station with code {station.Code} already exists");

            DataSet.Stations.Add(station.Clone());
        }

        public void UpdateStation(Station station)
        {
            var exists = DataSet.Stations.Find(s => s.Code == station.Code);

            if (exists == null) throw new BadStationCodeException(station.Code, $"no station with code {station.Code}");

            DataSet.Stations.Remove(exists);
            DataSet.Stations.Add(station.Clone());
        }

        public void UpdateStation(int code, Action<Station> update)
        {
            throw new NotImplementedException();

            var exists = DataSet.Stations.Find(s => s.Code == code);

            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            update(exists);
        }

        public void DeleteStation(int code)
        {
            var exists = DataSet.Stations.Find(s => s.Code == code);

            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            DataSet.Stations.Remove(exists);
        }

        public void DeleteAllStations()
        {
            DataSet.Stations.Clear();
        }

        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            return from bus in DataSet.Buses select bus.Clone();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSet.Buses
                   let cloned = bus.Clone()
                   where predicate(cloned)
                   select cloned;
        }

        public Bus GetBus(int regNum)
        {
            var bus = DataSet.Buses.Find(b => b.RegNum == regNum);

            if (bus == null) throw new BadBusRegistrationException(regNum, $"no bus with registration number {regNum}");

            return bus.Clone();
        }

        public void AddBus(Bus bus)
        {
            if (DataSet.Buses.Any(b => b.RegNum == bus.RegNum))
                throw new BadBusRegistrationException(bus.RegNum, $"bus with registration number {bus.RegNum} already exists");

            if (!(bus.RegDate.Year >= 2018 && (bus.RegNum < 100000000 && bus.RegNum > 9999999) ||
                    bus.RegDate.Year < 2018 && (bus.RegNum < 10000000 && bus.RegNum > 999999)))
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "bus registration number doesn't match the registration year");

            DataSet.Buses.Add(bus.Clone());
        }

        public void UpdateBus(Bus bus)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == bus.RegNum);

            if (exists == null) throw new BadStationCodeException(bus.RegNum, $"no bus with registratin number {bus.RegNum}");

            if (bus.RegDate.Year >= 2018 && bus.RegNum < 1000000 || bus.RegDate.Year < 2018 && bus.RegNum > 9999999)
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "bus registration number doesn't match the registration year");

            DataSet.Buses.Remove(exists);
            DataSet.Buses.Add(bus.Clone());
        }

        public void DeleteBus(int regNum)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == regNum);

            if (exists == null) throw new BadStationCodeException(regNum, $"no bus with registratin number {regNum}");

            DataSet.Buses.Remove(exists);
        }

        public void DeleteAllBuses()
        {
            DataSet.Buses.Clear();
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSet.Lines select busLine.Clone();
        }

        public IEnumerable<BusLine> GetBusLinesBy(Predicate<BusLine> predicate)
        {
            return from busLine in DataSet.Lines
                   let cloned = busLine.Clone()
                   where predicate(cloned)
                   select cloned;
        }

        public BusLine GetBusLine(Guid ID)
        {
            var busLine = DataSet.Lines.Find(b => b.ID == ID);

            if (busLine == null) throw new BadBusLineIDException(ID, $"no bus line with ID {ID}");

            return busLine.Clone();
        }

        public void AddBusLine(BusLine busLine)
        {
            if (DataSet.Lines.Any(b => b.ID == busLine.ID))
                throw new BadBusLineIDException(busLine.ID, $"bus line with ID {busLine.ID} already exists");

            // Checks for the start and end stations
            GetStation(busLine.StartStationCode);
            GetStation(busLine.EndStationCode);

            DataSet.Lines.Add(busLine.Clone());
        }

        public void UpdateBusLine(BusLine busLine)
        {
            var exists = DataSet.Lines.Find(b => b.ID == busLine.ID);

            if (exists == null) throw new BadBusLineIDException(busLine.ID, $"no bus line with ID {busLine.ID}");

            DataSet.Lines.Remove(exists);
            DataSet.Lines.Add(exists);
        }

        public void DeleteBusLine(Guid ID)
        {
            var busLine = DataSet.Lines.Find(b => b.ID == ID);

            if (busLine == null) throw new BadBusLineIDException(ID, $"no bus line with ID {ID}");

            DataSet.Lines.Remove(busLine);
        }

        public void DeleteAllBusLines()
        {
            DataSet.Lines.Clear();
        }
        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStations(Guid lineID)
        {
            return from lineStation in DataSet.LineStations
                   where lineStation.LineID == lineID
                   select lineStation.Clone();
        }

        public IEnumerable<LineStation> GetLineStationsBy(Guid lineID, Predicate<LineStation> predicate)
        {
            return from lineStation in DataSet.LineStations
                   where lineStation.LineID == lineID
                   let cloned = lineStation.Clone()
                   where predicate(cloned)
                   select cloned;
        }

        public LineStation GetLineStationByStation(Guid lineID, int stationCode)
        {
            var lineStations = GetAllLineStations(lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"no line station for line with id {lineID}");

            var lineStation = lineStations.First(
                b => b.LineID == lineID && b.StationCode == stationCode);

            if (lineStation == null) throw new BadLineStationStationCodeException(lineID, stationCode,
                $"no line station with station code {stationCode} found for line ID {lineID}");

            return lineStation.Clone();
        }

        public LineStation GetLineStationByIndex(Guid lineID, int index)
        {
            var lineStations = GetAllLineStations(lineID);
            if (lineStations.Count() == 0)
                throw new BadBusLineIDException(lineID, $"no line station for line with id {lineID}");

            var lineStation = lineStations.First(
                b => b.LineID == lineID && b.RouteIndex == index);

            if (lineStation == null) throw new BadLineStationIndexException(lineID, index,
                $"no line station with index {index} found for line ID {lineID}");

            return lineStation.Clone();
        }

        public void AddLineStation(LineStation lineStation)
        {
            // checks for bus line and station.
            GetBusLine(lineStation.LineID);
            GetStation(lineStation.StationCode);

            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void UpdateLineStationByStation(LineStation lineStation)
        {
            var exists = GetLineStationByStation(lineStation.LineID, lineStation.StationCode);

            DataSet.LineStations.Remove(exists);
            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void UpdateLineStationByIndex(LineStation lineStation)
        {
            var exists = GetLineStationByIndex(lineStation.LineID, lineStation.RouteIndex);

            DataSet.LineStations.Remove(exists);
            DataSet.LineStations.Add(lineStation.Clone());
        }

        public void DeleteLineStationByStation(Guid lineID, int stationCode)
        {
            var exists = GetLineStationByStation(lineID, stationCode);

            DataSet.LineStations.Remove(exists);
        }

        public void DeleteLineStationByIndex(Guid lineID, int index)
        {
            var exists = GetLineStationByIndex(lineID, index);

            DataSet.LineStations.Remove(exists);
        }

        public void DeleteAllLineStations(Guid lineID)
        {
            DataSet.LineStations.RemoveAll(lineStation => lineStation.LineID == lineID);
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            return from adjacent in DataSet.AdjacentStations select adjacent.Clone();
        }

        public IEnumerable<AdjacentStations> GetAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            return from agjacent in DataSet.AdjacentStations
                   let cloned = agjacent.Clone()
                   where predicate(cloned)
                   select cloned;
        }

        public AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            var adjacent = DataSet.AdjacentStations.Find(a => a.Station1Code == stationCode1 && a.Station2Code == stationCode2 || a.Station1Code == stationCode2 && a.Station2Code == stationCode1);

            if (adjacent == null) throw new BadAdjacentStationsCodeException(stationCode1, stationCode2, $"no adjacent stations matches these stations");

            return adjacent.Clone();
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            // checks for stations.
            GetStation(adjacentStations.Station1Code);
            GetStation(adjacentStations.Station2Code);

            if (DataSet.AdjacentStations.Any(a => a.Station1Code == adjacentStations.Station1Code && a.Station2Code == adjacentStations.Station2Code))
                throw new BadAdjacentStationsCodeException(adjacentStations.Station1Code, adjacentStations.Station2Code, $"adjacent stations already exists");

            DataSet.AdjacentStations.Add(adjacentStations.Clone());
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            var exists = DataSet.AdjacentStations.Find(a => a.Station1Code == adjacentStations.Station1Code && a.Station2Code == adjacentStations.Station2Code);

            if (exists == null) throw new BadAdjacentStationsCodeException(adjacentStations.Station1Code, adjacentStations.Station2Code, $"no adjacent stations matches these stations");

            DataSet.AdjacentStations.Remove(exists);
            DataSet.AdjacentStations.Add(adjacentStations.Clone());
        }

        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            var adjacent = DataSet.AdjacentStations.Find(a => a.Station1Code == stationCode1 && a.Station2Code == stationCode2);

            if (adjacent == null) throw new BadAdjacentStationsCodeException(stationCode1, stationCode2, $"no adjacent stations matches these stations");

            DataSet.AdjacentStations.Remove(adjacent);
        }

        public void DeleteAllAdjacentStations()
        {
            DataSet.AdjacentStations.Clear();
        }
        #endregion
    }
}
