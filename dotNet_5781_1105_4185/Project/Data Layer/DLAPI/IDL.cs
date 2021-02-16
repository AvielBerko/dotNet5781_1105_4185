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

        /// <summary>
        /// Gets the user entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The user the the given name </returns>
        DO.User GetUser(string name);

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="user"></param>
        void AddUser(DO.User user);

        /// <summary>
        /// Deletes a given user
        /// </summary>
        /// <param name="user"></param>
        void DeleteUser(DO.User user);

        #endregion

        #region Station

        /// <summary>
        /// Gets all stations
        /// </summary>
        /// <returns>IEnumerable of all existing stations</returns>
        IEnumerable<DO.Station> GetAllStations();

        /// <summary>
        /// Gets all stations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all stations that obey the predicate</returns>
        IEnumerable<DO.Station> GetStationsBy(Predicate<DO.Station> predicate);

        /// <summary>
        /// Gets a station by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The station with the given code</returns>
        DO.Station GetStation(int code);

        /// <summary>
        /// Adds a new station
        /// </summary>
        /// <param name="station"></param>
        void AddStation(DO.Station station);

        /// <summary>
        /// Updates an existing station
        /// </summary>
        /// <param name="station"></param>
        void UpdateStation(DO.Station station);

        /// <summary>
        /// Deletes an existing station by code
        /// </summary>
        /// <param name="code"></param>
        void DeleteStation(int code);

        /// <summary>
        /// Deletes all existing stations
        /// </summary>
        void DeleteAllStations();

        /// <summary>
        /// Deletes all stations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteStationsBy(Predicate<DO.Station> predicate);

        #endregion

        #region Bus

        /// <summary>
        /// Gets all existing buses
        /// </summary>
        /// <returns>IEnumerable of all existing buses</returns>
        IEnumerable<DO.Bus> GetAllBuses();

        /// <summary>
        /// Gets all buses that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all buses that obey the predicate</returns>
        IEnumerable<DO.Bus> GetBusesBy(Predicate<DO.Bus> predicate);

        /// <summary>
        /// Gets a bus by registration number
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The bus with the given registration number</returns>
        DO.Bus GetBus(int regNum);

        /// <summary>
        /// Adds a new bus
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(DO.Bus bus);

        /// <summary>
        /// Updates an existing bus
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBus(DO.Bus bus);

        /// <summary>
        /// Deletes an existing bus by registration number
        /// </summary>
        /// <param name="regNum"></param>
        void DeleteBus(int regNum);

        /// <summary>
        /// Deletes all existing buses
        /// </summary>
        void DeleteAllBuses();

        /// <summary>
        /// Deletes all buses that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteBusesBy(Predicate<DO.Bus> predicate);

        #endregion

        #region BusLine

        /// <summary>
        /// Gets all existing BusLines
        /// </summary>
        /// <returns>IEnumerable of all existing BusLines</returns>
        IEnumerable<DO.BusLine> GetAllBusLines();

        /// <summary>
        /// Gets all BusLines that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all BusLines that obey the predicate</returns>
        IEnumerable<DO.BusLine> GetBusLinesBy(Predicate<DO.BusLine> predicate);

        /// <summary>
        /// Gets a BusLine by ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The BusLine with the given ID</returns>
        DO.BusLine GetBusLine(Guid ID);

        /// <summary>
        /// Adds a new BusLine
        /// </summary>
        /// <param name="busLine"></param>
        void AddBusLine(DO.BusLine busLine);

        /// <summary>
        /// Updates an existing busLine
        /// </summary>
        /// <param name="busLine"></param>
        void UpdateBusLine(DO.BusLine busLine);

        /// <summary>
        /// Deletes an existing bus by ID
        /// </summary>
        /// <param name="ID"></param>
        void DeleteBusLine(Guid ID);

        /// <summary>
        /// Deletes all existing busLines
        /// </summary>
        void DeleteAllBusLines();

        /// <summary>
        /// Deletes all busLines that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteBusLinesBy(Predicate<DO.BusLine> predicate);

        #endregion

        #region LineStation

        /// <summary>
        /// Gets all existing LineStations
        /// </summary>
        /// <returns>IEnumerable of all existing LineStations</returns>
        IEnumerable<DO.LineStation> GetAllLineStations();

        /// <summary>
        /// Gets all LineStations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all LineStations that obey the predicate</returns>
        IEnumerable<DO.LineStation> GetLineStationsBy(Predicate<DO.LineStation> predicate);

        /// <summary>
        /// Gets a LineStation by station code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The LineStation with the given ID</returns>
        DO.LineStation GetLineStationByStation(Guid lineID, int stationCode);

        /// <summary>
        /// Gets a LineStation by index
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The LineStation with the given index</returns>
        DO.LineStation GetLineStationByIndex(Guid lineID, int index);

        /// <summary>
        /// Adds a new LineStation
        /// </summary>
        /// <param name="lineStation"></param>
        void AddLineStation(DO.LineStation lineStation);

        /// <summary>
        /// Updates an existing lineStation by station
        /// </summary>
        /// <param name="lineStation"></param>
        void UpdateLineStationByStation(DO.LineStation lineStation);

        /// <summary>
        /// Updates an existing lineStation by index
        /// </summary>
        /// <param name="lineStation"></param>
        void UpdateLineStationByIndex(DO.LineStation lineStation);

        /// <summary>
        /// Deletes an existing bus by ID and station code
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="stationCode"></param>
        void DeleteLineStationByStation(Guid lineID, int stationCode);

        /// <summary>
        /// Deletes an existing bus by ID and station index
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="index"></param>
        void DeleteLineStationByIndex(Guid lineID, int index);

        /// <summary>
        /// Deletes all existing lineStations
        /// </summary>
        void DeleteLineStationsBy(Predicate<DO.LineStation> predicate);

        /// <summary>
        /// Deletes all lineStations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteAllLineStations();

        #endregion

        #region AdjacentStations

        /// <summary>
        /// Gets all existing adjacent stations
        /// </summary>
        /// <returns>IEnumerable of all existing adjacent stations</returns>
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();

        /// <summary>
        /// Gets all adjacent stations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all AdjacentStations that obey the predicate</returns>
        IEnumerable<DO.AdjacentStations> GetAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);

        /// <summary>
        /// Gets a LineStation by stationCode1 and stationCode2
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The AdjacentStations between the two given stations </returns>
        DO.AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2);

        /// <summary>
        /// Adds a new AdjacentStations
        /// </summary>
        /// <param name="adjacentStations"></param>
        void AddAdjacentStations(DO.AdjacentStations adjacentStations);

        /// <summary>
        /// Updates an existing adjacentStations
        /// </summary>
        /// <param name="adjacentStations"></param>
        void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);

        /// <summary>
        /// Adds or updates adjacentStations
        /// </summary>
        /// <param name="adjacentStations"></param>
        void AddOrUpdateAdjacentStations(DO.AdjacentStations adjacentStations);

        /// <summary>
        /// Deletes an existing bus by 2 station codes
        /// </summary>
        /// <param name="stationCode1"></param>
        /// <param name="stationCode2"></param>
        void DeleteAdjacentStations(int stationCode1, int stationCode2);

        /// <summary>
        /// Deletes all existing adjacentStations
        /// </summary>
        void DeleteAllAdjacentStations();

        /// <summary>
        /// Deletes all adjacentStations that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);

        #endregion

        #region LineTrip

        /// <summary>
        /// Gets all existing LineTrips
        /// </summary>
        /// <returns>IEnumerable of all existing LineTrips</returns>
        IEnumerable<DO.LineTrip> GetAllLineTrips();

        /// <summary>
        /// Gets all LineTrips that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable of all LineTrips that obey the predicate</returns>
        IEnumerable<DO.LineTrip> GetLineTripsBy(Predicate<DO.LineTrip> predicate);

        /// <summary>
        /// Gets a LineTrip by lineID and startTime
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The relevent LineTrip</returns>
        DO.LineTrip GetLineTrip(Guid lineID, TimeSpan startTime);

        /// <summary>
        /// Adds a new LineTrip
        /// </summary>
        /// <param name="lineTrip"></param>
        void AddLineTrip(DO.LineTrip lineTrip);

        /// <summary>
        /// Updates an existing lineTrip
        /// </summary>
        /// <param name="lineTrip"></param>
        void UpdateLineTrip(DO.LineTrip lineTrip);

        /// <summary>
        /// Deletes an existing bus by lineID and startTime
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="startTime"></param>
        void DeleteLineTrip(Guid lineID, TimeSpan startTime);

        /// <summary>
        /// Deletes all existing lineTrips
        /// </summary>
        void DeleteAllLineTrips();

        /// <summary>
        /// Deletes all lineTrips that obey a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteLineTripsBy(Predicate<DO.LineTrip> predicate);

        #endregion
    }
}
