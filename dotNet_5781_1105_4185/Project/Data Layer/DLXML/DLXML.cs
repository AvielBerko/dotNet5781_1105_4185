using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;

namespace DLXML
{
    sealed class DLXML : IDL
    {
        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }

        public Station GetStation(int code)
        {
            throw new NotImplementedException();
        }

        public void AddStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(Station station)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int code)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllStations()
        {
            throw new NotImplementedException();
        }

        public void DeleteStationsBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void AddBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void AddBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }

        public void AddLineStation(LineStation lineStation)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllAdjacentStations()
        {
            throw new NotImplementedException();
        }

        public void DeleteAllBuses()
        {
            throw new NotImplementedException();
        }

        public void DeleteAllBusLines()
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(int regNum)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLine(Guid ID)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStationByIndex(Guid lineID, int index)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStationByStation(Guid lineID, int stationCode)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStationsBy(Predicate<LineStation> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(int regNum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(Guid ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public LineStation GetLineStationByIndex(Guid lineID, int index)
        {
            throw new NotImplementedException();
        }

        public LineStation GetLineStationByStation(Guid lineID, int stationCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetLineStationsBy(Predicate<LineStation> predicate)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStationByIndex(LineStation lineStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStationByStation(LineStation lineStation)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllLineStations()
        {
            throw new NotImplementedException();
        }
    }
}
