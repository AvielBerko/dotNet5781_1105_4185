using DLAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BO
{
    class TripOperator
    {
        #region Singleton
        static readonly Lazy<TripOperator> lazy = new Lazy<TripOperator>(() => new TripOperator());
        public static TripOperator Instance => lazy.Value;

        private TripOperator() { }
        #endregion

        private Action<BO.LineTiming> _updateTiming;
        public event Action<BO.LineTiming> UpdateTiming
        {
            add => _updateTiming = value;
            remove => _updateTiming -= value;
        }

        public void RaiseUpdateTiming(BO.LineTiming lineTiming)
        {
            _updateTiming(lineTiming);
        }

        public List<Tuple<LineTrip, TimeSpan>> NextTrips { get; set; }

        public List<BO.BusLine> BusLines { get; set; }

        public List<Thread> Threads { get; set; }

        public int? StationCode { get; set; }
    }
}
