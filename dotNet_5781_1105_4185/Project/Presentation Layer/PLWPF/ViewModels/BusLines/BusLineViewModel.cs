using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class BusLineViewModel : BaseViewModel
    {
        private BO.BusLine busLine;
        public BO.BusLine BusLine
        {
            get => busLine;
            set
            {
                busLine = value;
                OnPropertyChanged(nameof(BusLine));
            }
        }
        public RelayCommand RemoveBusLine { get; }
        public RelayCommand UpdateBusLine { get; }
        public RelayCommand DuplicateBusLine { get; }

        public BusLineViewModel(BO.BusLine busLine)
        {
            BusLine = busLine;

            RemoveBusLine = new RelayCommand(obj => { }/*{ BlWork(bl => bl.DeleteStation(Station)); OnRemove(); }*/);
            UpdateBusLine = new RelayCommand(obj => _Update());
            DuplicateBusLine = new RelayCommand(obj => { });
        }
        private void _Update()
		{
            //var updateVM = new UpdateStationViewModel { Station = station };
            //if (DialogService.ShowUpdateStationDialog(updateVM) == true) 
            //{
            //    OnPropertyChanged(nameof(Station));
            //}
        }

        public delegate void RemoveBusEventHandler(object sender);
        public event RemoveBusEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
