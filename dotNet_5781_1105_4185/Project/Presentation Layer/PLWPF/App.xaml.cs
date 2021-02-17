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

            BaseViewModel.Context = new ViewContext();

            MainWindow window = new MainWindow();
            MainViewModel mainVM = new MainViewModel();
            window.DataContext = mainVM;
            mainVM.Shutdown += (sender) => Shutdown(0);

            window.Show();
        }
    }
}
