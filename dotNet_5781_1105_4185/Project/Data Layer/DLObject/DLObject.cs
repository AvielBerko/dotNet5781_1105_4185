using System;
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
            return DataSet.Users.Find(user => user.Name == name).Clone();
        }

        public void AddUser(DO.User user)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
