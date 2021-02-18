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
            BaseViewModel.DialogService = new ViewDialogService();

            var vm = new MainViewModel();
            var window = new MainWindow(vm);

            if (!vm.Login())
            {
                Shutdown(0);
            }

            window.Show();
        }
    }
}
