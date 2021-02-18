using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// A singleton class in charge of the simulation time
    /// and updating the observerer about changin in time.
    /// </summary>
    class Clock
    {
        #region Singleton
        static readonly Lazy<Clock> lazy = new Lazy<Clock>(() => new Clock());
        public static Clock Instance => lazy.Value;

        private Clock() { }
        #endregion

        /// <summary>
        /// The current time of the clock.
        /// When it gets updated the observerer gets updated about the time.
        /// </summary>
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                _updateTime(_time);
            }
        }
        private TimeSpan _time;

        /// <summary>
        /// The speed rate of the simulation clock's time.
        /// Every 1 second in real time passing Rate seconds in simulation time.
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Observerer of the clock's time. <br />
        /// Only one observerer can exist.
        /// </summary>
        public event Action<TimeSpan> UpdateTime
        {
            add => _updateTime = value;
            remove => _updateTime -= value;
        }
        private Action<TimeSpan> _updateTime;
    }
}
