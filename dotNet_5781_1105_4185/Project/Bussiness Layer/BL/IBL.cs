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

        /// <summary>
        /// Authenticates the user (by username and password)
        /// </summary>
        /// <param name="name">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns>The user with the given credentials</returns>
        BO.User UserAuthentication(string name, string password);

        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// <param name="name">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns>The user with the given credentials</returns>
        BO.User UserSignUp(string name, string password);

        /// <summary>
        /// Checks if the name is valid (uniqe)
        /// </summary>
        /// <param name="name">User's username</param>
        void ValidateSignUpName(string name);

        /// <summary>
        /// Checks if the password is valid
        /// </summary>
        /// <param name="password">User's password</param>
        void ValidateSignUpPassword(string password);

        #endregion

        #region Station

        /// <summary>
        /// Gets all stations without the information about their adjacent stations
        /// </summary>
        /// <returns>IEnumerable of all stations</returns>
        IEnumerable<BO.Station> GetAllStationsWithoutAdjacents();

        /// <summary>
        /// Gets all stations with the information about their adjacent stations
        /// </summary>
        /// <returns>IEnumerable of all stations</returns>
        IEnumerable<BO.Station> GetAllStations();

        /// <summary>
        /// Gets all station that obey the given predicate
        /// </summary>
        /// <returns>IEnumerable of all stations that obet the predicate</returns>
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);

        /// <summary>
        /// Gets all statations but the given one
        /// </summary>
        /// <param name="stations">The given station</param>
        /// <returns>IEnumerable of all stations but the given one</returns>
        IEnumerable<BO.Station> GetRestOfStations(IEnumerable<BO.Station> stations);

        /// <summary>
        /// Gets a station by code without the information about it's adjacent stations
        /// </summary>
        /// <returns>The station with the given code</returns>
        BO.Station GetStationWithoutAdjacents(int code);

        /// <summary>
        /// Gets a station by code with the information about it's adjacent stations
        /// </summary>
        /// <returns>The station with the given code</returns>
        BO.Station GetStation(int code);

        /// <summary>
        /// Adds a new station
        /// </summary>
        /// <param name="station">The station to add</param>
        void AddStation(BO.Station station);

        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="station">The station to update</param>
        void UpdateStation(BO.Station station);

        /// <summary>
        /// Deletes a station (by it's code)
        /// </summary>
        /// <param name="code">The station code to delete</param>
        void DeleteStation(int code);

        /// <summary>
        /// Deletes all stations
        /// </summary>
        void DeleteAllStations();

        /// <summary>
        /// Validates a new station (uniqe code)
        /// </summary>
        /// <param name="station">The station to validate</param>
        void ValidateNewStation(BO.Station station);

        /// <summary>
        /// Validates a station's code
        /// </summary>
        /// <param name="code">The code to validate</param>
        void ValidateNewStationCode(int code);

        /// <summary>
        /// Validates a station's name
        /// </summary>
        /// <param name="name">The code to validate</param>
        void ValidateNewStationName(string name);

        /// <summary>
        /// Validates a station's address
        /// </summary>
        /// <param name="address">The address to validate</param>
        void ValidateNewStationAddress(string address);

        /// <summary>
        /// Validates a station's latitude
        /// </summary>
        /// <param name="location">The Latitude to validate</param>
        void ValidateNewStationLatitude(BO.Location location);

        /// <summary>
        /// Validates a station's longitude
        /// </summary>
        /// <param name="location">The longitude to validate</param>
        void ValidateNewStationLongitude(BO.Location location);

        #endregion

        #region Bus

        /// <summary>
        /// Gets all buses
        /// </summary>
        /// <returns>IEnumerable of all buses</returns>
        IEnumerable<BO.Bus> GetAllBuses();

        /// <summary>
        /// Adds a new bus
        /// </summary>
        /// <param name="bus">The bus to add</param>
        void AddBus(BO.Bus bus);

        /// <summary>
        /// Deletes a list of buses
        /// </summary>
        /// <param name="buses">IEnumerable of buses to delete</param>
        void DeleteListOfBuses(IEnumerable<BO.Bus> buses);

        /// <summary>
        /// Deletes all buses
        /// </summary>
        void DeleteAllBuses();

        /// <summary>
        /// Deletes a bus
        /// </summary>
        /// <param name="bus">The bus to delete</param>
        void DeleteBus(BO.Bus bus);

        /// <summary>
        /// Validates a bus' registration
        /// </summary>
        /// <param name="registration">The registration to validate</param>
        void ValidateRegistration(BO.Registration registration);

        /// <summary>
        /// Refuels the given bus
        /// </summary>
        /// <param name="bus">The bus to refuel</param>
        void RefuelBus(BO.Bus bus);
        #endregion

        #region BusLine, LineStations, Trips

        /// <summary>
        /// Gets a list of all buslines without information about their full route and trips
        /// </summary>
        /// <returns>IEnumerable of all buslines</returns>
        IEnumerable<BO.BusLine> GetAllBusLinesWithoutFullRouteAndTrips();

        /// <summary>
        /// Gets a list of all buslines with information about their full route and trips
        /// </summary>
        /// <returns>IEnumerable of all buslines</returns>
        IEnumerable<BO.BusLine> GetAllBusLines();

        /// <summary>
        /// Gets a list of all buslines that passes at a given station (by it's code)
        /// </summary>
        /// <param name="code">Station's code</param>
        /// <returns>IEnumerable of all buslines</returns>
        IEnumerable<BO.BusLine> GetLinesPassingTheStation(int code);

        /// <summary>
        /// Reverses the list of linestations
        /// </summary>
        /// <param name="stations">The list of stations</param>
        /// <returns>The reversed list</returns>
        IEnumerable<BO.LineStation> ReverseLineStations(IEnumerable<BO.LineStation> stations);


        IEnumerable<BO.LineTrip> CollidingTrips(IEnumerable<BO.LineTrip> trips);

        /// <summary>
        /// Gets a busline by ID
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>The bus with the given ID</returns>
        BO.BusLine GetBusLine(Guid ID);

        /// <summary>
        /// Gets a busline by ID without the infornation about it's route and trips
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>The bus with the given ID</returns>
        BO.BusLine GetBusLineWithoutRouteAndTrips(Guid ID);

        /// <summary>
        /// Duplicates a busline
        /// </summary>
        /// <param name="ID">Busline's ID</param>
        /// <returns>The duplicated busline</returns>
        BO.BusLine DuplicateBusLine(Guid ID);

        /// <summary>
        /// Checks if a busline has a full route or not
        /// </summary>
        /// <param name="ID">Busline's ID</param>
        /// <returns>True if the route is full, False otherwise</returns>
        bool BusLineHasFullRoute(Guid ID);

        /// <summary>
        /// Adds a new busline
        /// </summary>
        /// <param name="busLine">The busline to add</param>
        void AddBusLine(BO.BusLine busLine);

        /// <summary>
        /// Updates a busline
        /// </summary>
        /// <param name="busLine">The busline to adypdated</param>
        void UpdateBusLine(BO.BusLine busLine);

        /// <summary>
        /// Deletes all buslines
        /// </summary>
        void DeleteAllBusLines();

        /// <summary>
        /// Deletes a busline by ID
        /// </summary>
        /// <param name="ID">Busline's ID</param>
        void DeleteBusLi‎ne(Guid ID);

        /// <summary>
        /// Validate busline's trip frequency
        /// </summary>
        /// <param name="frequency">The frequency to validate</param>
        void ValidateLineTripFrequency(TimeSpan frequency);
        #endregion

        #region Simulation
        /// <summary>
        /// Start the simulation
        /// </summary>
        /// <param name="timeOfDay">Starting simulation time</param>
        /// <param name="rate">Ratio between real and simulation time</param>
        /// <param name="callback"></param>
        void StartSimulation(TimeSpan timeOfDay, int rate, Action<TimeSpan> callback);
        
        /// <summary>
        /// Stops the simulation
        /// </summary>
        void StopSimulation();

        /// <summary>
        /// Sets the station panel
        /// </summary>
        /// <param name="stationCode">Station's code</param>
        /// <param name="updateTiming"></param>
        void SetStationPanel(int? stationCode, Action<IEnumerable<BO.LineTiming>> updateTiming);
        #endregion
    }
}
