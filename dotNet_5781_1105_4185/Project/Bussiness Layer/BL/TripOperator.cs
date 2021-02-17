using DLAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    class TripOperator
    {
        #region Singleton
        static readonly Lazy<TripOperator> lazy = new Lazy<TripOperator>(() => new TripOperator());
        public static TripOperator Instance => lazy.Value;

        private TripOperator() { }
        #endregion

        private Action<BO.LineTiming[]> _updateTiming;
        public event Action<BO.LineTiming[]> UpdateTiming
        {
            add => _updateTiming = value;
            remove => _updateTiming -= value;
        }

        public List<Tuple<BO.LineTrip, TimeSpan>> NextTrips { get; set; }

        public List<BO.BusLine> BusLines { get; set; }

        public List<Thread> Threads { get; set; }

        public int? StationCode { get; set; }

        private Dictionary<int, List<BO.LineTiming>> _stationsDigitTable = new Dictionary<int, List<BO.LineTiming>>();

        public void RaiseUpdateTiming()
        {
            if (StationCode == null) return;

            int code = StationCode ?? 0;
            if (!_stationsDigitTable.ContainsKey(code))
            {
                _stationsDigitTable.Add(code, new List<BO.LineTiming>());
            }

            var listToSend = (
                from lt in _stationsDigitTable[code]
                select (BO.LineTiming)lt.CopyPropertiesToNew(typeof(BO.LineTiming))
            ).ToArray();

            _updateTiming(listToSend);
        }

        public void ClearTable()
        {
            _stationsDigitTable.Clear();
        }

        private object lockObject‎ = new object();
        public void AddLineTiming(int stationCode, BO.LineTiming lineTiming)
        {
            lock (lockObject)
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
                    var listToSend = (
                        from lt in list
                        select (BO.LineTiming)lt.CopyPropertiesToNew(typeof(BO.LineTiming))
                    ).ToArray();

                    _updateTiming(listToSend);
                }
            }
        }
    }
}
