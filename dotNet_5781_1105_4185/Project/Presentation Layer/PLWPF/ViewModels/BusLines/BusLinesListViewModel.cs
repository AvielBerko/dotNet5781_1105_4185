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
        ObservableCollection<BusLineViewModel> busLines;
        public ObservableCollection<BusLineViewModel> BusLines
        {
            get => busLines;
            private set
            {
                busLines = value;
                OnPropertyChanged(nameof(BusLines));
            }
        }

        //public AddBusLineViewModel AddBusLineViewModel { get; }
        public RelayCommand AddBusLine { get; }
        public RelayCommand RemoveAllBusLines { get; }
        public RelayCommand Refresh { get; }

        public BusLinesListViewModel()
        {
            //AddBusLineViewModel = new AddBusLineViewModel();

            UpdateList();

            //AddBusLineViewModel.AddedBusLine += (sender, busLine) => BusLines.Add(CreateBusLineViewModel(busLine));

            AddBusLine = new RelayCommand(obj => _AddBusLine());
            RemoveAllBusLines = new RelayCommand(obj => _RemoveAllBusLines(), obj => BusLines.Count > 0);
            Refresh = new RelayCommand(obj => UpdateList());
        }

        private void UpdateList()
        {
            BusLines = new ObservableCollection<BusLineViewModel>(
                from busLine in (IEnumerable<BO.BusLine>)BlWork(bl => bl.GetAllBusLinesWithoutFullRoute())
                select CreateBusLineViewModel(busLine));
        }

        private BusLineViewModel CreateBusLineViewModel(BO.BusLine busLine)
        {
            var vm = new BusLineViewModel(busLine);
            vm.Remove += (sender) => BusLines.Remove((BusLineViewModel)sender);
            vm.Duplicate += (sender, duplicated) => BusLines.Add(CreateBusLineViewModel(duplicated));
            return vm;
        }

        private void _AddBusLine()
        {
            //DialogService.ShowAddStationDialog(BusLineViewModel);
        }

        private void _RemoveAllBusLines()
        {
            // Are You Sure ? 
            BlWork(bl => bl.DeleteAllBusLines());
            BusLines.Clear();
        }
    }
}
