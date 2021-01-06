using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class StationViewModel : BaseViewModel
	{
        private BO.Station station;
        public BO.Station Station
        {
            get => station;
            set
            {
                station = value;
                OnPropertyChanged(nameof(Station));
            }
        }
        public RelayCommand RemoveStation { get; }

        public StationViewModel()
        {
            RemoveStation = new RelayCommand(obj => { BlWork(bl => bl.DeleteStation(Station)); OnRemove(); });
        }

        public delegate void RemoveBusEventHandler(object sender);
        public event RemoveBusEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
