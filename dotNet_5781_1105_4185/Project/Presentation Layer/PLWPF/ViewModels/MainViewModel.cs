using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    class MainViewModel : BaseViewModel
    {
        Uri mainPage;
        Uri MainPage
        {
            get => mainPage;
            set
            {
                mainPage = value;
                OnPropertyChanged(nameof(mainPage));
            }
        }

        public LoginViewModel LoginViewModel { get; }
        public SignUpViewModel SignUpViewModel { get; }
        public BusListViewModel BusListViewModel { get; }
        public StationListViewModel StationListViewModel { get; }
        public BusLinesListViewModel BusLinesListViewModel { get; }
        public SimulationViewModel SimulationViewModel { get; set; }

        public RelayCommand Close { get; set; }

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel();
            SignUpViewModel = new SignUpViewModel();
            BusListViewModel = new BusListViewModel();
            StationListViewModel = new StationListViewModel();
            BusLinesListViewModel = new BusLinesListViewModel();
            SimulationViewModel = new SimulationViewModel();

            Close = new RelayCommand(obj => _Close());

            LoginViewModel.LoggedIn += LoggedIn;
            SignUpViewModel.SignedUp += LoggedIn;

            if (DialogService.ShowLoginDialog(LoginViewModel, SignUpViewModel) != DialogResult.Ok)
            {
                OnShutdown();
            }
        }

        private void _Close()
        {
            SimulationViewModel.StopSimulation.Execute(null);
        }

        private void LoggedIn(object sender, BO.User user)
        {
            MainPage = new Uri("Pages/AdminPage.xaml", UriKind.Relative);
        }

        public delegate void ShutdownEventHandler(object sender);
        public event ShutdownEventHandler Shutdown;
        protected virtual void OnShutdown() => Shutdown?.Invoke(this);
    }
}
