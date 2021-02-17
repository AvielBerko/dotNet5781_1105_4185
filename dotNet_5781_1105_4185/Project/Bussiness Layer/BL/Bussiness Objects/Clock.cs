using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BO
{
    class Clock
    {
        #region Singleton
        static readonly Lazy<Clock> lazy = new Lazy<Clock>(() => new Clock());
        public static Clock Instance => lazy.Value;

        private Clock() { }
        #endregion

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                _updateTime(_time);
            }
        }

        public int Rate { get; set; }

        private Action<TimeSpan> _updateTime;
        public event Action<TimeSpan> UpdateTime
        {
            add => _updateTime = value;
            remove => _updateTime -= value;
        }
    }
}
