using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class StationDetailsViewModel : BaseDialogViewModel
	{
		public BO.Station Station { get; }
		public ObservableCollection<AdjacentStationViewModel> AdjacentStations { get; }
		public ObservableCollection<BusLineViewModel> PassingLines { get; }

		public StationDetailsViewModel(int code)
		{
			Station = (BO.Station)BlWork(bl => bl.GetStation(code));
			AdjacentStations = new ObservableCollection<AdjacentStationViewModel>
				(from ad in Station.AdjacentStations select new AdjacentStationViewModel(ad));
			PassingLines = new ObservableCollection<BusLineViewModel>(from line in (IEnumerable<BO.BusLine>)BlWork(bl => bl.GetLinesPassingTheStation(code)) select new BusLineViewModel(line));
		}
	}
}
