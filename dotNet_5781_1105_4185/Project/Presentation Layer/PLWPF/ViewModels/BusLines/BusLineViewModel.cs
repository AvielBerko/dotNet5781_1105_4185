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
        public string StartName
        {
            get
            {
                try
                {
                    return busLine.Route.First().Station.Name;
                }
                catch
                {
                    return "Not Set";
                }
            }
        }
        public string EndName
        {
            get
            {
                try
                {
                    return busLine.Route.Last().Station.Name;
                }
                catch
                {
                    return "Not Set";
                }
            }
        }
        public bool HasFullRoute => (bool)BlWork(bl => bl.BusLineHasFullRoute(busLine.ID));

        public RelayCommand RemoveBusLine { get; }
        public RelayCommand UpdateBusLine { get; }
        public RelayCommand DuplicateBusLine { get; }

        public BusLineViewModel(BO.BusLine busLine)
        {
            BusLine = busLine;

            RemoveBusLine = new RelayCommand(obj => _Remove());
            UpdateBusLine = new RelayCommand(obj => _Update());
            DuplicateBusLine = new RelayCommand(obj => _Duplicate());
        }

        private void _Update()
        {
            var vm = new AddUpdateBusLineViewModel(BusLine.ID);
            if (DialogService.ShowAddBusLineDialog(vm) == true)
            {
                OnUpdate();
            }
        }

        private void _Duplicate()
        {
            var duplicated = (BO.BusLine)BlWork(bl => bl.DuplicateBusLine(busLine.ID));
            OnDupliate(duplicated);
        }

        private void _Remove()
        {
            BlWork(bl => bl.DeleteBusLine(busLine.ID));
            OnRemove();
        }

        public delegate void DuplicateBusLineEventHandler(object sender, BO.BusLine duplicated);
        public event DuplicateBusLineEventHandler Duplicate;
        protected virtual void OnDupliate(BO.BusLine duplicated) => Duplicate?.Invoke(this, duplicated);

        public delegate void RemoveBusLineEventHandler(object sender);
        public event RemoveBusLineEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);

        public delegate void UpdateBusLineEventHandler(object sender);
        public event UpdateBusLineEventHandler Update;
        protected virtual void OnUpdate() => Update?.Invoke(this);
    }
}
