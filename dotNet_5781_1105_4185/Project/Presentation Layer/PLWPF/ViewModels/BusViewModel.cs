using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class BusViewModel : BaseViewModel
    {
        private BO.Bus bus;
        public BO.Bus Bus
        {
            get => bus;
            set
            {
                bus = value;
                OnPropertyChanged(nameof(Bus));
            }
        }

        public RelayCommand RefuelBus { get; }
        public RelayCommand RemoveBus { get; }

        public BusViewModel()
        {
            RefuelBus = new RelayCommand(obj => BlWork(bl => bl.RefuelBus(bus)));
            RemoveBus = new RelayCommand(obj => OnRemove());
        }

        public delegate void RemoveBusEventHandler(object sender);
        public event RemoveBusEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
