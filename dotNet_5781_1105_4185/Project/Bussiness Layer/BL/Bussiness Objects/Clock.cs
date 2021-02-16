using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BO
{
    static class Clock
    {
        private static TimeSpan _time;
        public static TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                _updateTime(_time);
            }
        }

        public static int Rate { get; set; }

        private static Action<TimeSpan> _updateTime;
        public static event Action<TimeSpan> UpdateTime
        {
            add => _updateTime = value;
            remove => _updateTime -= value;
        }
    }
}
