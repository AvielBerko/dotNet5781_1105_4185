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
                    Name = "Admin",
                    Password = "Admin",
                    Role = DO.Roles.Admin
                }
            };
        }
    }
}
