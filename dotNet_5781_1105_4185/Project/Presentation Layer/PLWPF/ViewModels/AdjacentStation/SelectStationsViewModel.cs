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
        private ObservableCollection<SelectableStationsViewModel> _stations;
        public ObservableCollection<SelectableStationsViewModel> Stations
        { 
            get => _stations;
            set
            {
                _stations = value;
                OnPropertyChanged(nameof(Stations));
            }
        }
        public BO.Station[] SelectedStations => (from st in Stations
                                                 where st.IsSelected
                                                 select st.Station).ToArray();

        public RelayCommand Ok { get; }
        public RelayCommand Cancel { get; }

        public SelectStationsViewModel(IEnumerable<BO.Station> unselectableStations)
        {
            Stations = new ObservableCollection<SelectableStationsViewModel>();
            _ = GetStationsFromBL(unselectableStations);

            Ok = new RelayCommand(_Ok, obj => SelectedStations.Length > 0);
            Cancel = new RelayCommand(_Cancel);
        }

        private async Task GetStationsFromBL(IEnumerable<BO.Station> unselectableStations)
        {
            await Load(async () =>
            {
                var selectable = (IEnumerable<BO.Station>)(
                    await BlWorkAsync(bl => bl.GetRestOfStations(unselectableStations)
                ));
                Stations = new ObservableCollection<SelectableStationsViewModel>(
                    from st in selectable
                    select new SelectableStationsViewModel(st)
                );
            });
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
