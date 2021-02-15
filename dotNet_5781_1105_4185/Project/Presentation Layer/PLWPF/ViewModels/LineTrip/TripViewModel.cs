using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class TripViewModel : BaseViewModel
    {
        private BO.Trip _trip;
        public BO.Trip Trip
        {
            get => _trip;
            set
            {
                _trip = value;

                OnPropertyChanged(nameof(Trip));
                OnPropertyChanged(nameof(OneTime));
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        private bool _oneTime;
        public bool OneTime
        {
            get => _oneTime;
            set
            {
                _oneTime = value;

                if (_oneTime)
                {
                    _trip.FinishTime = null;
                    _trip.Frequency = null;
                }
                else
                {
                    _trip.FinishTime = DateTime.Now;
                    _trip.Frequency = TimeSpan.Zero;
                }

                OnPropertyChanged(nameof(Trip));
                OnPropertyChanged(nameof(OneTime));
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        public bool IsColliding => _isColliding(this);
        private readonly Func<TripViewModel, bool> _isColliding;

        public RelayCommand Remove { get; }

        public TripViewModel(BO.Trip trip, Func<TripViewModel, bool> isColliding)
        {
            _isColliding = isColliding;
            Trip = trip;
            OneTime = trip.Frequency == null;
            Remove = new RelayCommand(obj => _Remove());
        }

        private void _Remove()
        {
            OnRemove();
        }

        public event Action<TripViewModel> RemoveTrip;
        protected virtual void OnRemove() => RemoveTrip?.Invoke(this);
    }
}
