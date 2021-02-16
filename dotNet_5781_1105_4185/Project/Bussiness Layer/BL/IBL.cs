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

        #region AdjacentStation
        #endregion

        #region Station
        IEnumerable<BO.Station> GetAllStationsWithoutAdjacents();
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);
        IEnumerable<BO.Station> GetRestOfStations(IEnumerable<BO.Station> stations);
        BO.Station GetStationWithoutAdjacents(int code);
        BO.Station GetStation(int code);
        void AddStation(BO.Station station);
        void UpdateStation(BO.Station station);
        void DeleteStation(int code);
        void DeleteAllStations();
        void ValidateNewStation(BO.Station station);
        void ValidateNewStationCode(int code);
        void ValidateNewStationName(string name);
        void ValidateNewStationAddress(string address);
        void ValidateNewStationLatitude(BO.Location location);
        void ValidateNewStationLongitude(BO.Location location);
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

        #region BusLine, LineStations, Trips
        IEnumerable<BO.BusLine> GetAllBusLinesWithoutFullRouteAndTrips();
        IEnumerable<BO.BusLine> GetAllBusLines();
        IEnumerable<BO.BusLine> GetLinesPassingTheStation(int code);
        IEnumerable<BO.LineStation> ReverseLineStations(IEnumerable<BO.LineStation> stations);
        IEnumerable<BO.LineTrip> CollidingTrips(IEnumerable<BO.LineTrip> trips);
        BO.BusLine GetBusLine(Guid ID);
        BO.BusLine GetBusLineWithoutRouteAndTrips(Guid ID);
        BO.BusLine DuplicateBusLine(Guid ID);
        bool BusLineHasFullRoute(Guid ID);
        void AddBusLine(BO.BusLine busLine);
        void UpdateBusLine(BO.BusLine busLine);
        void DeleteAllBusLines();
        void DeleteBusLi‎ne(Guid ID);
        void ValidateLineTripFrequency(TimeSpan frequency);
        #endregion

        #region Simulation
        void StartSimulation(TimeSpan timeOfDay, int rate, Action<TimeSpan> callback);
        void StopSimulation();
        #endregion
    }
}
