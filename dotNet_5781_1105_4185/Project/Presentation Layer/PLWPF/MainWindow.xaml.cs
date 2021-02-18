using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            // Change frame page manuali (not working by binding)
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "MainPage")
                {
                    MainFrame.Navigate(vm.MainPage);
                }
            };
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            vm.Close.Execute(null);
        }

        private void MainFrameNavigated(object sender, NavigationEventArgs e)
        {
            // Sets the frame content DataContext.
            var content = MainFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = MainFrame.DataContext;
        }
    }
}
