﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;
using DS;

namespace DL
{
    sealed class DLObject : IDL
    {
        #region Singleton
        static readonly Lazy<DLObject> lazy = new Lazy<DLObject>(() => new DLObject());
        public static DLObject Instance => lazy.Value;

        private DLObject() { }
        #endregion

        #region User
        public DO.User GetUser(string name)
        {
            var user = DataSet.Users.Find(u => u.Name == name);

            if (user == null) throw new BadUserNameException(name, $"User with the name {name} not found");

            return user.Clone();
        }

        public void AddUser(DO.User user)
        {
            if (DataSet.Users.Any(u => u.Name == user.Name))
                throw new BadUserNameException(user.Name, $"User with the name {user.Name} already exists");

            DataSet.Users.Add(user.Clone());
        }

        public void DeleteUser(DO.User user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Station
        public IEnumerable<Station> GetAllStations()
        {
            return (from station in DataSet.Stations select station.Clone()).AsEnumerable();
        }
        public IEnumerable<Station> GetStationsBy(Predicate<Station> predicate)
        {
            return (from station in DataSet.Stations
                    let cloned = station.Clone()
                    where predicate(cloned)
                    select cloned).AsEnumerable();
        }

        public Station GetStation(int code)
        {
            var station = DataSet.Stations.Find(s => s.Code == code);

            if (station == null) throw new BadStationCodeException(code, $"no station with code {code}");

            return station.Clone();
        }

        public void AddStation(Station station)
        {
            if (DataSet.Stations.Any(s => s.Code == station.Code))
                throw new BadStationCodeException(station.Code, $"station with code {station.Code} already exists");

            DataSet.Stations.Add(station.Clone());
        }

        public void UpdateStation(Station station)
        {
            var exists = DataSet.Stations.Find(s => s.Code == station.Code);

            if (exists == null) throw new BadStationCodeException(station.Code, $"no station with code {station.Code}");

            DataSet.Stations.Remove(exists);
            DataSet.Stations.Add(station.Clone());
        }

        public void UpdateStation(int code, Action<Station> update)
        {
            throw new NotImplementedException();

            var exists = DataSet.Stations.Find(s => s.Code == code);

            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            update(exists);
        }

        public void DeleteStation(int code)
        {
            var exists = DataSet.Stations.Find(s => s.Code == code);

            if (exists == null) throw new BadStationCodeException(code, $"no station with code {code}");

            DataSet.Stations.Remove(exists);
        }

        public void DeleteAllStations()
		{
            DataSet.Stations.Clear();
		}

        #endregion

        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            return (from bus in DataSet.Buses select bus.Clone()).AsEnumerable();
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            return (from bus in DataSet.Buses
                    let cloned = bus.Clone()
                    where predicate(cloned)
                    select cloned).AsEnumerable();
        }

        public Bus GetBus(int regNum)
        {
            var bus = DataSet.Buses.Find(b => b.RegNum == regNum);

            if (bus == null) throw new BadBusRegistrationException(regNum, $"no bus with registration number {regNum}");

            return bus.Clone();
        }

        public void AddBus(Bus bus)
        {
            if (DataSet.Buses.Any(b => b.RegNum == bus.RegNum))
                throw new BadBusRegistrationException(bus.RegNum, $"bus with registration number {bus.RegNum} already exists");
            
            if (!(bus.RegDate.Year >= 2018 && (bus.RegNum < 100000000 && bus.RegNum > 9999999) ||
                    bus.RegDate.Year < 2018 && (bus.RegNum < 10000000 && bus.RegNum > 999999)))
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "bus registration number doesn't match the registration year");

            DataSet.Buses.Add(bus.Clone());
        }

        public void UpdateBus(Bus bus)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == bus.RegNum);

            if (exists == null) throw new BadStationCodeException(bus.RegNum, $"no bus with registratin number {bus.RegNum}");

            if (bus.RegDate.Year >= 2018 && bus.RegNum < 1000000 || bus.RegDate.Year < 2018 && bus.RegNum > 9999999)
                throw new BadBusRegistrationException(bus.RegNum, bus.RegDate, "bus registration number doesn't match the registration year");

            DataSet.Buses.Remove(exists);
            DataSet.Buses.Add(bus.Clone());
        }

        public void DeleteBus(int regNum)
        {
            var exists = DataSet.Buses.Find(b => b.RegNum == regNum);

            if (exists == null) throw new BadStationCodeException(regNum, $"no bus with registratin number {regNum}");

            DataSet.Buses.Remove(exists);
        }

		public void DeleteAllBuses()
		{
            DataSet.Buses.Clear();
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(Guid ID)
        {
            throw new NotImplementedException();
        }

        public void AddBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLine(Guid ID)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllBusLines()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStation(Guid lineID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetLineStationsBy(Guid lineID, Predicate<LineStation> predicate)
        {
            throw new NotImplementedException();
        }

        public LineStation GetLineStationByStation(Guid lineID, int stationCode)
        {
            throw new NotImplementedException();
        }

        public LineStation GetLineStationByIndex(Guid lineID, int index)
        {
            throw new NotImplementedException();
        }

        public void AddLineStation(LineStation lineSation)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStationByStation(LineStation lineSation)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStationByIndex(LineStation lineSation)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStationByStation(Guid lineID, int stationCode)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStationByIndex(Guid lineID, int index)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllLineStations(Guid lineID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAdjacentStationsBy(Predicate<AdjacentStations> predicate)
        {
            throw new NotImplementedException();
        }

        public AdjacentStations GetAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }

        public void AddAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStations(int stationCode1, int stationCode2)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllAdjacentStations()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
