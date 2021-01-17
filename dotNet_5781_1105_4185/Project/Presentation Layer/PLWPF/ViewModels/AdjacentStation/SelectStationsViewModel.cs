using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class SelectStationsViewModel : BaseViewModel
	{
		public ObservableCollection<SelectableStationsViewModel> Stations { get; set; }
		public BO.Station[] SelectedStations => (from st in Stations where st.IsSelected select st.Station).ToArray();
		public RelayCommand Ok { get; }
		public RelayCommand Cancel { get; }

		public SelectStationsViewModel(IEnumerable<BO.Station> selectableStations)
		{
			Stations = new ObservableCollection<SelectableStationsViewModel>(from st in selectableStations select new SelectableStationsViewModel(st));
			Ok = new RelayCommand(_Ok, obj => SelectedStations.Length > 0);
			Cancel = new RelayCommand(_Cancel);
		}
		private void _Ok(object window)
		{
			DialogService.CloseDialog(window, DialogResult.Ok);
		}

		private void _Cancel(object window)
		{
			DialogService.CloseDialog(window, DialogResult.Cancel);
		}
	}
}
