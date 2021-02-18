using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class MainViewModel : BaseViewModel
    {
        private Uri _mainPage;
        public Uri MainPage
        {
            get => _mainPage;
            set
            {
                _mainPage = value;
                OnPropertyChanged(nameof(MainPage));
            }
        }

        public BusListViewModel BusListViewModel { get; }
        public StationListViewModel StationListViewModel { get; }
        public BusLinesListViewModel BusLinesListViewModel { get; }
        public SimulationViewModel SimulationViewModel { get; set; }

        public RelayCommand Close { get; set; }

        public MainViewModel()
        {
            BusListViewModel = new BusListViewModel();
            StationListViewModel = new StationListViewModel();
            BusLinesListViewModel = new BusLinesListViewModel();
            SimulationViewModel = new SimulationViewModel();

            Close = new RelayCommand(obj => _Close());
        }

        public bool Login()
        {
            var vm = new LoginViewModel();
            if (DialogService.ShowLoginDialog(vm) == DialogResult.Ok)
            {
                if (vm.User.Role == BO.Roles.Admin)
                {
                    MainPage = new Uri("Pages/AdminPage.xaml", UriKind.Relative);
                }
                else
                {
                    MainPage = new Uri("Pages/UserPage.xaml", UriKind.Relative);
                }
                return true;
            }

            return false;
        }

        private void _Close()
        {
            SimulationViewModel.StopSimulation.Execute(null);
        }
    }
}
