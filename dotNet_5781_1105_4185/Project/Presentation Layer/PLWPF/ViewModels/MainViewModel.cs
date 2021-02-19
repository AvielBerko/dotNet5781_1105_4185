using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    /// <summary>
    /// View model in charge of open the login dialog and manage the main page.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// The uri of the main page that will be shown.
        /// </summary>
        public Uri MainPage
        {
            get => _mainPage;
            set
            {
                _mainPage = value;
                OnPropertyChanged(nameof(MainPage));
            }
        }
        private Uri _mainPage;

        /// <summary>
        /// The view model in charge of the buses' list.
        /// </summary>
        public BusListViewModel BusListViewModel { get; }

        /// <summary>
        /// The view model in charge of the stations' list.
        /// </summary>
        public StationListViewModel StationListViewModel { get; }

        /// <summary>
        /// The view model in charge of the bus lines' list.
        /// </summary>
        public BusLinesListViewModel BusLinesListViewModel { get; }

        /// <summary>
        /// The view model in charge of the simulation.
        /// </summary>
        public SimulationViewModel SimulationViewModel { get; }

        /// <summary>
        /// Command to call when closing the main window. <br />
        /// This command is in charge of freeing all resources needed to free before close.
        /// </summary>
        public RelayCommand Close { get; set; }

        public MainViewModel()
        {
            BusListViewModel = new BusListViewModel();
            StationListViewModel = new StationListViewModel();
            BusLinesListViewModel = new BusLinesListViewModel();
            SimulationViewModel = new SimulationViewModel();

            Close = new RelayCommand(obj => _Close());
        }

        /// <summary>
        /// Opens the login dialog and sets the main page according to the user authorization.
        /// </summary>
        /// <returns>Returns true if successfuly loged in. Else returns false.</returns>
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
            // Stops the simulation before closing in order to close all used threads.
            SimulationViewModel.StopSimulation.Execute(null);
        }
    }
}
