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
		public ObservableCollection<BO.Bus> Buses { get; }
		public AddBusViewModel AddBusViewModel { get; }
		public RelayCommand AddBus { get; }
		public RelayCommand RemoveAllBuses { get; }
		public RelayCommand TreatBus { get; }
		public RelayCommand RefuelBus { get; }
		public RelayCommand RemoveBus { get; }


		public BusListViewModel()
		{
			Buses = new ObservableCollection<BO.Bus>((IEnumerable<BO.Bus>)BlWork(bl => bl.GetAllBuses()));
			Buses.CollectionChanged += BusesCollectionChanged;
			AddBusViewModel = new AddBusViewModel();
			AddBusViewModel.AddedBus += (sender, bus) => Buses.Add(bus);
			AddBus = new RelayCommand(obj => _AddBus());
			RemoveAllBuses = new RelayCommand(obj => _RemoveAllBuses(), obj => Buses.Count > 0);
			TreatBus = new RelayCommand(obj => _TreatBus((BO.Bus)obj));
			RefuelBus = new RelayCommand(obj => _RefuelBus((BO.Bus)obj));
			RemoveBus = new RelayCommand(obj => _RemoveBus((BO.Bus)obj));
		}

		private void BusesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			BlWork(bl => bl.DeleteListOfBuses((IEnumerable<BO.Bus>)e.OldItems));
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

		private void _TreatBus(BO.Bus bus)
		{
			
		}

		private void _RefuelBus(BO.Bus bus)
		{

		}
		private void _RemoveBus(BO.Bus bus)
		{
			Buses.Remove(bus);
		}
	}
}
