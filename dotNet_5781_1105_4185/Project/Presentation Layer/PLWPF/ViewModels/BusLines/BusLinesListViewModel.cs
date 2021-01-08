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
        public ObservableCollection<BusLineViewModel> BusLines { get; }
        //public AddBusLineViewModel AddBusLineViewModel { get; }
        public RelayCommand AddBusLine { get; }
        public RelayCommand RemoveAllBusLines { get; }

        public BusLinesListViewModel()
        {
            //AddBusLineViewModel = new AddBusLineViewModel();

            BusLines = new ObservableCollection<BusLineViewModel>(
                from busLine in (IEnumerable<BO.BusLine>)BlWork(bl => bl.GetAllBusLinesWithoutRoute())
                select CreateBusLineViewModel(busLine));

            //AddBusLineViewModel.AddedBusLine += (sender, busLine) => BusLines.Add(CreateBusLineViewModel(busLine));

            AddBusLine = new RelayCommand(obj => _AddBusLine());
            RemoveAllBusLines = new RelayCommand(obj => _RemoveAllBusLines(), obj => BusLines.Count > 0);
        }

        private BusLineViewModel CreateBusLineViewModel(BO.BusLine busLine)
        {
            var vm = new BusLineViewModel(busLine);
            vm.Remove += (sender) => BusLines.Remove((BusLineViewModel)sender);
            return vm;
        }

        private void _AddBusLine()
        {
            //DialogService.ShowAddStationDialog(BusLineViewModel);
        }

        private void _RemoveAllBusLines()
        {
            // Are You Sure ? 
            //BlWork(bl => bl.DeleteAllStations());
            BusLines.Clear();
        }
    }
}
