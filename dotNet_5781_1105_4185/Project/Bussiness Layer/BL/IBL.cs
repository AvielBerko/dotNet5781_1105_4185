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
        void DeleteStation(int code);
        #endregion

        #region Bus
        IEnumerable<BO.Bus> GetAllBuses();
        void DeleteListOfBuses(IEnumerable<BO.Bus> buses);
		#endregion
	}
}
