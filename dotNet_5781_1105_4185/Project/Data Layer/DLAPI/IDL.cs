using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI
{
    public interface IDL
    {
        #region User
        DO.User GetUser(string name);
        void AddUser(DO.User user);
        void DeleteUser(DO.User user);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        void DeleteStation(int code);
        void DeleteAllStations();
        void DeleteStationsBy(Predicate<DO.Station> predicate);
        #endregion

        #region Bus
        IEnumerable<DO.Bus> GetAllBuses();
        IEnumerable<DO.Bus> GetBusesBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int regNum);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void DeleteBus(int regNum);
        void DeleteAllBuses();
        void DeleteBusesBy(Predicate<DO.Bus> predicate);
        #endregion

        #region BusLine
        IEnumerable<DO.BusLine> GetAllBusLines();
        IEnumerable<DO.BusLine> GetBusLinesBy(Predicate<DO.BusLine> predicate);
        DO.BusLine GetBusLine(Guid ID);
        void AddBusLine(DO.BusLine busLine);
        void UpdateBusLine(DO.BusLine busLine);
        void DeleteBusLine(Guid ID);
        void DeleteAllBusLines();
        void DeleteBusLinesBy(Predicate<DO.BusLine> predicate);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStations();
        IEnumerable<DO.LineStation> GetLineStationsBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStationByStation(Guid lineID, int stationCode);
        DO.LineStation GetLineStationByIndex(Guid lineID, int index);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStationByStation(DO.LineStation lineStation);
        void UpdateLineStationByIndex(DO.LineStation lineStation);
        void DeleteLineStationByStation(Guid lineID, int stationCode);
        void DeleteLineStationByIndex(Guid lineID, int index);
        void DeleteLineStationsBy(Predicate<DO.LineStation> predicate);
        void DeleteAllLineStations();
        #endregion

        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        IEnumerable<DO.AdjacentStations> GetAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2);
        void AddAdjacentStations(DO.AdjacentStations adjacentStations);
        void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        void AddOrUpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        void DeleteAdjacentStations(int stationCode1, int stationCode2);
        void DeleteAllAdjacentStations();
        void DeleteAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        #endregion
    }
}
