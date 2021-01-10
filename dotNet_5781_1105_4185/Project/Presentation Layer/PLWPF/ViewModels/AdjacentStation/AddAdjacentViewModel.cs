using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class AddAdjacentViewModel : BaseDialogViewModel
	{
		public ObservableCollection<SelectableStationsViewModel> Stations { get; set; }
		public BO.Station[] SelectedStations => (from st in Stations where st.IsSelected select st.Station).ToArray();
		public RelayCommand Ok { get; }
		public RelayCommand Cancel { get; }

		public AddAdjacentViewModel(IEnumerable<BO.Station> selectableStations)
		{
			Stations = new ObservableCollection<SelectableStationsViewModel>(from st in selectableStations select new SelectableStationsViewModel(st));
			Ok = new RelayCommand(_Ok, obj => SelectedStations.Length > 0);
			Cancel = new RelayCommand(_Cancel);
		}
		private void _Ok(object window)
		{
			/*BlWork(bl => bl.AddStation(station));
			OnAddedStation(station);*/
			CloseDialog(window, true);
		}

		private void _Cancel(object window)
		{
			CloseDialog(window, false);
		}

		public delegate void AddedStationEventHandler(object sender, BO.Station station);
		public event AddedStationEventHandler AddedStaion;
		protected virtual void OnAddedStation(BO.Station station) => AddedStaion?.Invoke(this, station);
	}
}
