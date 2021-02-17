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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = (TabControl)sender;

            if (tabControl.SelectedItem != SimulationTab)
            {
                var vm = (SimulationViewModel)SimulationTab.DataContext;
                if (vm != null && vm.StopSimulation.CanExecute(null))
                {
                    vm.StopSimulation.Execute(null);
                }
            }

            if (tabControl.SelectedItem == LineListTab)
            {
                var vm = (BusLinesListViewModel)LineListTab.DataContext;
                if (vm != null)
                {
                    vm.Refresh.Execute(null);
                }
            }
        }
    }
}
