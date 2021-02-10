using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class BusLinesListViewModel : BaseViewModel
    {
        ObservableCollection<BusLineViewModel> _busLines;
        public ObservableCollection<BusLineViewModel> BusLines
        {
            get => _busLines;
            private set
            {
                _busLines = value;
                OnPropertyChanged(nameof(BusLines));
            }
        }

        public RelayCommand AddBusLine { get; }
        public RelayCommand RemoveAllBusLines { get; }
        public RelayCommand Refresh { get; }

        public BusLinesListViewModel()
        {
            _ = _UpdateList();

            AddBusLine = new RelayCommand(obj => _AddBusLine());
            RemoveAllBusLines = new RelayCommand(async obj => await _RemoveAllBusLines(),
                obj => BusLines.Count > 0);
            Refresh = new RelayCommand(async obj => await _UpdateList());
        }

        private async Task _UpdateList()
        {
            await Load(async () =>
            {
                var busLines = (IEnumerable<BO.BusLine>)await BlWorkAsync(bl => bl.GetAllBusLinesWithoutFullRoute());
                BusLines = new ObservableCollection<BusLineViewModel>(
                    from busLine in busLines
                    select CreateBusLineViewModel(busLine));
            });
        }

        private BusLineViewModel CreateBusLineViewModel(BO.BusLine busLine)
        {
            var vm = new BusLineViewModel(busLine);
            vm.Remove += (sender) => BusLines.Remove((BusLineViewModel)sender);
            vm.Duplicate += (sender, duplicated) => BusLines.Add(CreateBusLineViewModel(duplicated));
            vm.Update += (sender) => Refresh.Execute(null);
            return vm;
        }

        private void _AddBusLine()
        {
            var vm = new AddUpdateBusLineViewModel();
            if (DialogService.ShowAddUpdateBusLineDialog(vm) == DialogResult.Ok)
            {
                BusLines.Add(CreateBusLineViewModel(vm.BusLine));
                Refresh.Execute(null);
            }
        }

        private async Task _RemoveAllBusLines()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to removea all bus lines?",
                "Remove all bus lines") == DialogResult.Yes)
            {
                await Load(async () =>
                {
                    await BlWorkAsync(bl => bl.DeleteAllBusLines());
                    BusLines.Clear();
                });
            }
        }
    }
}
