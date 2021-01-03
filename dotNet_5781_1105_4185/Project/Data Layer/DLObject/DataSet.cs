using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    static class DataSet
    {
        public static List<DO.User> Users;
        public static List<DO.Bus> Buses;
        public static List<DO.BusLine> Lines;
        public static List<DO.DrivingBus> DrivingBuses;
        public static List<DO.Station> Stations;
        public static List<DO.LineStation> LineStations;
        public static List<DO.AdjacentStations> AdjacentStations;
        public static List<DO.Trip> Trips;
        public static List<DO.LineTrip> LineTrips;

        static DataSet()
        {
            InitData();
        }

        static void InitData()
        {
            Users = new List<DO.User>
            {
                new DO.User
                {
                    Name = "admin",
                    Password = "admin",
                    Role = DO.Roles.Admin
                },
                new DO.User
                {
                    Name = "user",
                    Password = "user",
                    Role = DO.Roles.Normal
                }
            };

            Buses = new List<DO.Bus>
            {

            };

            Lines = new List<DO.BusLine>
            {

            };

            ‎DrivingBuses = new List<DO.DrivingBus>
            {

            };

            Stations = new List<DO.Station>
            {

            };

            LineStations = new List<DO.LineStation>
            {

            };

            AdjacentStations = new List<DO.AdjacentStations>
            {

            };

            Trips = new List<DO.Trip>
            {

            };

            LineTrips = new List<DO.LineTrip>
            {

            };
        }
    }
}
