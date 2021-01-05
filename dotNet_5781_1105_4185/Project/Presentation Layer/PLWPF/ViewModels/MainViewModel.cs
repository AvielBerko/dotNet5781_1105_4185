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

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel();
            SignUpViewModel = new SignUpViewModel();

            LoginViewModel.LoggedIn += LoggedIn;
            SignUpViewModel.SignedUp += LoggedIn;
        }

        private void LoggedIn(object sender, BO.User user)
        {
            MainPage = new Uri("Pages/AdminPage.xaml", UriKind.Relative);
        }
    }
}
