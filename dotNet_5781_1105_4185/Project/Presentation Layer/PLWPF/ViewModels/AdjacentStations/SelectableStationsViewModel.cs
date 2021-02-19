using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
	public class SelectableStationsViewModel : BaseViewModel
	{
		public BO.Station Station { get; set; }
		public bool IsSelected { get; set; }

		public SelectableStationsViewModel(BO.Station station)
		{
			Station = station;
		}
	}
}
