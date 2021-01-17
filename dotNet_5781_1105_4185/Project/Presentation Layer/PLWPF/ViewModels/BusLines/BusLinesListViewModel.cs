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

        public RelayCommand AddBusLine { get; }
        public RelayCommand RemoveAllBusLines { get; }
        public RelayCommand Refresh { get; }

        public BusLinesListViewModel()
        {
            UpdateList();

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

        private void _RemoveAllBusLines()
        {
            if (DialogService.ShowYesNoDialog("Are you sure you want to removea all bus lines?", "Remove all bus lines") == DialogResult.Yes)
            {
                BlWork(bl => bl.DeleteAllBusLines());
                BusLines.Clear();
            }
        }
    }
}
