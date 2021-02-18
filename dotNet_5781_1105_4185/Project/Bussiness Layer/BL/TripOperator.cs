using DLAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// This class is a singleton in charge of the trip operation memory
    /// and is observed by the selected station of the simulation.
    /// </summary>
    class TripOperator
    {
        #region Singleton
        static readonly Lazy<TripOperator> lazy = new Lazy<TripOperator>(() => new TripOperator());
        public static TripOperator Instance => lazy.Value;

        private TripOperator() { }
        #endregion

        /// <summary>
        /// Observerer of a given station's digital panel. <br />
        /// Only one observerer can exist.
        /// </summary>
        public event Action<IEnumerable<BO.LineTiming>> UpdateTiming
        {
            add => _updateTiming = value;
            remove => _updateTiming -= value;
        }
        private Action<IEnumerable<BO.LineTiming>> _updateTiming;

        /// <summary>
        /// The observerer's station's code.
        /// </summary>
        public int? StationCode { get; set; }

        /// <summary>
        /// List of the all the next trips and their start time ordered by their start time.
        /// </summary>
        public List<Tuple<BO.LineTrip, TimeSpan>> NextTrips { get; set; }

        /// <summary>
        /// List of all the buslines that will drive.
        /// </summary>
        public List<BO.BusLine> BusLines { get; set; }

        /// <summary>
        /// List of all the threads used by the operation
        /// in order to clean them at the end of the operation.
        /// </summary>
        public List<Thread> Threads { get; set; }

        /// <summary>
        /// Dictionary that gets a station and returns the information of the station's digital panel.
        /// </summary>
        private Dictionary<int, List<BO.LineTiming>> _stationsDigitTable = new Dictionary<int, List<BO.LineTiming>>();

        /// <summary>
        /// Updates the observerer about its digital panel's timing.
        /// </summary>
        public void RaiseUpdateTiming()
        {
            if (StationCode == null) return;

            int code = StationCode ?? 0;

            // Locks the dictionary so other threads won't change its data.
            lock (_stationsDigitTable)
            {
                if (!_stationsDigitTable.ContainsKey(code))
                {
                    _stationsDigitTable.Add(code, new List<BO.LineTiming>());
                }

                var listToSend = from lt in _stationsDigitTable[code]
                                 select (BO.LineTiming)lt.CopyPropertiesToNew(typeof(BO.LineTiming));

                _updateTiming(listToSend);
            }
        }

        /// <summary>
        /// Clears all the data of the digital panels.
        /// </summary>
        public void ClearTable()
        {
            _stationsDigitTable.Clear();
        }

        public void AddLineTiming(int stationCode, BO.LineTiming lineTiming)
        {
            // Locks the dictionary so other threads won't change its data.
            lock (_stationsDigitTable)
            {
                if (!_stationsDigitTable.ContainsKey(stationCode))
                {
                    _stationsDigitTable.Add(stationCode, new List<BO.LineTiming>());
                }

                var list = _stationsDigitTable[stationCode];
                var existing = list.FirstOrDefault(lt =>
                   lt.LineID == lineTiming.LineID && lt.CurrentStartTime == lineTiming.CurrentStartTime);
                if (existing != null)
                {
                    list.Remove(existing);
                }

                if (lineTiming.ArrivalTime >= TimeSpan.Zero)
                {
                    list.Add(lineTiming);
                }

                if (StationCode == stationCode)
                {
                    var listToSend = from lt in list
                                     select (BO.LineTiming)lt.CopyPropertiesToNew(typeof(BO.LineTiming));

                    _updateTiming(listToSend);
                }
            }
        }
    }
}
