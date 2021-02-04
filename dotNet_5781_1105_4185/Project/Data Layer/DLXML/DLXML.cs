using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;

namespace DL
{
    sealed class DLXML : IDL
    {
        #region Singleton
        static readonly Lazy<DLXML> lazy = new Lazy<DLXML>(() => new DLXML());
        public static DLXML Instance => lazy.Value;

        private DLXML() { }
        #endregion

        private static string FileName(Type type) => type.Name + ".xml";

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));
        }

        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));
            return from station in stations
                   where predicate(station)
                   select station;
        }

        public Station GetStation(int code)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));

            var station = (from s in stations
                           where s.Code == code
                           select s).FirstOrDefault();
            if (station == null) throw new BadStationCodeException(code, $"Station with code {code} not found");

            return station;
        }

        public void AddStation(Station station)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));
            if (stations.Any(s => s.Code == station.Code))
                throw new BadStationCodeException(station.Code, $"Station with code {station.Code} already exists");

            stations.Add(station);

            XMLTools.SaveListToXMLSerializer(stations, FileName(typeof(Station)));
        }

        public void UpdateStation(Station station)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));
            var exists = stations.Find(s => s.Code == station.Code);
            if (exists == null) throw new BadStationCodeException(station.Code, $"no station with code {station.Code}");

            stations.Remove(exists);
            stations.Add(station);

            XMLTools.SaveListToXMLSerializer(stations, FileName(typeof(Station)));
        }

        public void DeleteStation(int code)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));
            var exists = stations.Find(s => s.Code == code);
            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            stations.Remove(exists);

            XMLTools.SaveListToXMLSerializer(stations, FileName(typeof(Station)));
        }

        public void DeleteAllStations()
        {
            XMLTools.SaveListToXMLSerializer(new List<Station>(), FileName(typeof(Station)));
        }

        public void DeleteStationsBy(Predicate<Station> predicate)
        {
            var stations = XMLTools.LoadListFromXMLSerializer<Station>(FileName(typeof(Station)));

            var updatedStations = from station in stations
                              where !predicate(station)
                              select station;

            XMLTools.SaveListToXMLSerializer(updatedStations.ToList(), FileName(typeof(Station)));
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
