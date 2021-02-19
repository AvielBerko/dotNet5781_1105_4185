using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class StationDetailsViewModel : BaseViewModel
    {
        private BO.Station _station;
        public BO.Station Station
        {
            get => _station;
            private set
            {
                _station = value;
                OnPropertyChanged(nameof(Station));
                OnPropertyChanged(nameof(Location));
            }
        }

        public string Location => Station?.Location.ToString() ?? "";

        private ObservableCollection<AdjacentStationViewModel> _adjacentStations;
        public ObservableCollection<AdjacentStationViewModel> AdjacentStations
        {
            get => _adjacentStations;
            private set
            {
                _adjacentStations = value;
                OnPropertyChanged(nameof(AdjacentStations));
            }
        }

        private ObservableCollection<BusLineViewModel> _passingLine;
        public ObservableCollection<BusLineViewModel> PassingLines
        {
            get => _passingLine;
            private set
            {
                _passingLine = value;
                OnPropertyChanged(nameof(PassingLines));
            }
        }

        public StationDetailsViewModel(int code)
        {
            _ = GetDataFromBL(code);
        }

        private async Task GetDataFromBL(int code)
        {
            await Load(async () =>
            {
                Station = (BO.Station)(await BlWorkAsync(bl => bl.GetStation(code)));
                AdjacentStations = new ObservableCollection<AdjacentStationViewModel> (
                    from adj in Station.AdjacentStations
                    select new AdjacentStationViewModel(adj, Station.Code)
                );
                PassingLines = new ObservableCollection<BusLineViewModel>(
                    from line in (IEnumerable<BO.BusLine>)(await BlWorkAsync(bl => bl.GetLinesPassingTheStation(code)))
                    select new BusLineViewModel(line)
                );
            });
        }
    }
}
