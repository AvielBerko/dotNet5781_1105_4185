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
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station station);
        void UpdateStation(int code, Action<DO.Station> update);
        void DeleteStation(int code);
        #endregion

        #region Bus
        IEnumerable<DO.Bus> GetAllBuses();
        IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int regNum);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        //void UpdateBus(int regNum, Action<DO.Bus> update);
        void DeleteBus(int regNum);
        #endregion
    }
}
