using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        #region User
        BO.User UserAuthentication(string name, string password);
        BO.User UserSignUp(string name, string password);
        void ValidateSignUpName(string name);
        void ValidateSignUpPassword(string password);
        #endregion

        #region Station
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);
        BO.Station GetStation(int code);
        void AddStation(BO.Station station);
        void UpdateStation(BO.Station station);
        void UpdateStation(int code, Action<BO.Station> update);
        void DeleteStation(BO.Station station);
        void DeleteAllStations();
        void ValidateStationCode(int code);
        void ValidateStationName(string name);
        void ValidateStationAddress(string address);
        void ValidateStationLocation(BO.Location location);

        #endregion

        #region Bus
        IEnumerable<BO.Bus> GetAllBuses();
        void AddBus(BO.Bus bus);
        void DeleteListOfBuses(IEnumerable<BO.Bus> buses);
        void DeleteAllBuses();
        void DeleteBus(BO.Bus bus);
        void ValidateRegistration(BO.Registration registration);
        void RefuelBus(BO.Bus bus);
        #endregion

        #region BusLine, LineStation, AdjacentLines
        IEnumerable<BO.BusLine> GetAllBusLinesWithoutFullRoute();
        IEnumerable<BO.BusLine> GetAllBusLines();
        #endregion
    }
}
