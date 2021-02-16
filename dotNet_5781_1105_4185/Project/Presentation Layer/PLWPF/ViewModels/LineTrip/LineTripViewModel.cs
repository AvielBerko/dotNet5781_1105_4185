using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class LineTripViewModel : BaseViewModel
    {
        public BO.LineTrip LineTrip { get; }

        public DateTime StartTime
        {
            get => LineTrip.StartTime;
            set
            {
                LineTrip.StartTime = value;

                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(LineTrip));
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        public DateTime? FinishTime
        {
            get => LineTrip.Frequncied?.FinishTime;
            set
            {
                LineTrip.Frequncied = new BO.FrequnciedTrip
                {
                    FinishTime = value ?? DateTime.Now,
                    Frequency = LineTrip.Frequncied?.Frequency ?? TimeSpan.Zero,
                };

                OnPropertyChanged(nameof(FinishTime));
                OnPropertyChanged(nameof(LineTrip));
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        public TimeSpan? Frequency
        {
            get => LineTrip.Frequncied?.Frequency;
            set
            {
                LineTrip.Frequncied = new BO.FrequnciedTrip
                {
                    Frequency = value ?? TimeSpan.Zero,
                    FinishTime = LineTrip.Frequncied?.FinishTime ?? DateTime.Now,
                };

                OnPropertyChanged(nameof(Frequency));
                OnPropertyChanged(nameof(LineTrip));
            }
        }

        public bool OneTime
        {
            get => LineTrip.Frequncied == null;
            set
            {
                if (value)
                    LineTrip.Frequncied = null;
                else
                    LineTrip.Frequncied = new BO.FrequnciedTrip();

                OnPropertyChanged(nameof(OneTime));
                OnPropertyChanged(nameof(FinishTime));
                OnPropertyChanged(nameof(Frequency));
                OnPropertyChanged(nameof(LineTrip));
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        public bool IsColliding => _isColliding(this);
        private readonly Func<LineTripViewModel, bool> _isColliding;

        public RelayCommand Remove { get; }

        public LineTripViewModel(BO.LineTrip trip, Func<LineTripViewModel, bool> isColliding)
        {
            _isColliding = isColliding;
            LineTrip = trip;
            Remove = new RelayCommand(obj => _Remove());
        }

        private void _Remove()
        {
            OnRemove();
        }

        public event Action<LineTripViewModel> RemoveLineTrip;
        protected virtual void OnRemove() => RemoveLineTrip?.Invoke(this);
    }
}
