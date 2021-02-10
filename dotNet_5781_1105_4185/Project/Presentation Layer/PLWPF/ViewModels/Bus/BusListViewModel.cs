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
        private ObservableCollection<BusViewModel> _buses;
        public ObservableCollection<BusViewModel> Buses
        {
            get => _buses;
            private set
            {
                _buses = value;
                OnPropertyChanged(nameof(_buses));
            }
        }

        public AddBusViewModel AddBusViewModel { get; }
        public RelayCommand AddBus { get; }
        public RelayCommand RemoveAllBuses { get; }

        public BusListViewModel()
        {
            Buses = new ObservableCollection<BusViewModel>();
            _ = LoadBusesFromBL();

            AddBusViewModel = new AddBusViewModel();
            AddBusViewModel.AddedBus += (sender, bus) => Buses.Add(CreateBusViewModel(bus));

            AddBus = new RelayCommand(obj => _AddBus());
            RemoveAllBuses = new RelayCommand(async obj => await _RemoveAllBuses(), obj => Buses.Count > 0);
        }

        private async Task LoadBusesFromBL()
        {
            await Load(async () =>
            {
                var buses = (IEnumerable<BO.Bus>)await BlWorkAsync(bl => bl.GetAllBuses());
                Buses = new ObservableCollection<BusViewModel>(
                    from bus in buses
                    select CreateBusViewModel(bus));
            });
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

        private async Task _RemoveAllBuses()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to removea all buses?", "Remove all buses") == DialogResult.Yes)
            {
                await Load(async () =>
                {
                    await BlWorkAsync(bl => bl.DeleteAllBuses());
                    Buses.Clear();
                });
            }
        }
    }
}
