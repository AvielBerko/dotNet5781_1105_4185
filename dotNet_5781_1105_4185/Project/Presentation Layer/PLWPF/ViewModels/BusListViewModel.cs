using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class BusListViewModel : BaseViewModel
    {
        public ObservableCollection<BusViewModel> Buses { get; }
        public AddBusViewModel AddBusViewModel { get; }
        public RelayCommand AddBus { get; }
        public RelayCommand RemoveAllBuses { get; }

        public BusListViewModel()
        {
            Buses = new ObservableCollection<BusViewModel>(
                from bus in (IEnumerable<BO.Bus>)BlWork(bl => bl.GetAllBuses())
                select CreateBusViewModel(bus));
            Buses.CollectionChanged += BusesCollectionChanged;

            AddBusViewModel = new AddBusViewModel();
            AddBusViewModel.AddedBus += (sender, bus) => Buses.Add(CreateBusViewModel(bus));

            AddBus = new RelayCommand(obj => _AddBus());
            RemoveAllBuses = new RelayCommand(obj => _RemoveAllBuses(), obj => Buses.Count > 0);
        }

        private void BusesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                var vms = (IEnumerable<BusViewModel>)e.OldItems;
                BlWork(bl => bl.DeleteListOfBuses(from busVM in vms select busVM.Bus));
            }
        }

        private BusViewModel CreateBusViewModel(BO.Bus bus)
        {
            var vm = new BusViewModel { Bus = bus };
            vm.Remove += (sender) => Buses.Remove((BusViewModel)sender);
            return vm;
        }

        private void _AddBus()
        {
            DialogService.ShowAddBusDialog(AddBusViewModel);
        }

        private void _RemoveAllBuses()
        {
            // Are You Sure ? 
            Buses.Clear();
        }
    }
}
