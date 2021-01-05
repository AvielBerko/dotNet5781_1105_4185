using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainViewModel mainVM = new MainViewModel();
            MainWindow window = new MainWindow(mainVM);

            var login = new LoginDialog(mainVM.LoginViewModel, mainVM.SignUpViewModel);

            mainVM.LoginViewModel.LoggedIn += (_, user) => login.DialogResult = true;
            mainVM.SignUpViewModel.SignedUp += (_, user) => login.DialogResult = true;

            if (login.ShowDialog() != true)
            {
                Shutdown(1);
            }

            window.Show();
        }
    }
}
