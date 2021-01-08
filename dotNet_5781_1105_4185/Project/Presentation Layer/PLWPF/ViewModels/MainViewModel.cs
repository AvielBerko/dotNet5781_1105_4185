using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class MainViewModel : BaseViewModel
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

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel();
            SignUpViewModel = new SignUpViewModel();
            BusListViewModel = new BusListViewModel();
            StationListViewModel = new StationListViewModel();
            BusLinesListViewModel = new BusLinesListViewModel();

            LoginViewModel.LoggedIn += LoggedIn;
            SignUpViewModel.SignedUp += LoggedIn;

            if (DialogService.ShowLoginDialog(LoginViewModel, SignUpViewModel) != true)
            {
                OnShutdown();
            }
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
