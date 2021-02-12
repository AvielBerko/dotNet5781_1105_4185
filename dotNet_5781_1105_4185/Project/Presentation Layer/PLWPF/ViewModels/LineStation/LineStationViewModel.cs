using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class LineStationViewModel : BaseViewModel
    {
        public BO.LineStation LineStation { get; }

        public double? Distance
        {
            get => LineStation.NextStationRoute?.Distance;
            set
            {
                if (value == null)
                {
                    LineStation.NextStationRoute = null;
                }
                else
                {
                    LineStation.NextStationRoute = new BO.NextStationRoute
                    {
                        Distance = value ?? 0,
                        DrivingTime = LineStation.NextStationRoute?.DrivingTime ?? TimeSpan.Zero
                    };
                }

                OnPropertyChanged(nameof(LineStation));
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(DrivingTime));
            }
        }

        public TimeSpan? DrivingTime
        {
            get => LineStation.NextStationRoute?.DrivingTime;
            set
            {
                if (value == null)
                {
                    LineStation.NextStationRoute = null;
                }
                else
                {
                    LineStation.NextStationRoute = new BO.NextStationRoute
                    {
                        Distance = LineStation.NextStationRoute?.Distance ?? 0,
                        DrivingTime = value ?? TimeSpan.Zero
                    };
                }

                OnPropertyChanged(nameof(LineStation));
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(DrivingTime));
            }
        }

        private bool _isLast;
        public bool IsLast
        {
            get => _isLast;
            set
            {
                _isLast = value;
                OnPropertyChanged(nameof(IsLast));
            }
        }

        public RelayCommand Remove { get; }
        public RelayCommand InverseAdjacents { get; }
        public RelayCommand Insert { get; }

        public LineStationViewModel(BO.LineStation lineStation)
        {
            LineStation = lineStation;
            Remove = new RelayCommand(obj => OnRemove());
            InverseAdjacents = new RelayCommand(obj => _InverseAdjacents());
            Insert = new RelayCommand(obj => OnInsertStation());
        }

        private void _InverseAdjacents()
        {
            if (LineStation.NextStationRoute == null)
            {
                LineStation.NextStationRoute = new BO.NextStationRoute();
            }
            else
            {
                LineStation.NextStationRoute = null;
            }

            OnPropertyChanged(nameof(LineStation));
            OnPropertyChanged(nameof(Distance));
            OnPropertyChanged(nameof(DrivingTime));
        }

        public event Action<LineStationViewModel> InsertStation;
        protected virtual void OnInsertStation() => InsertStation?.Invoke(this);

        public event Action<LineStationViewModel> RemoveLineStation;
        protected virtual void OnRemove() => RemoveLineStation?.Invoke(this);
    }
}
