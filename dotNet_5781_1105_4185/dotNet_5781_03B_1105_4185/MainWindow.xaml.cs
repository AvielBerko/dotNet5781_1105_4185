using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace dotNet_5781_03B_1105_4185
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        List<Bus> buses = new List<Bus>();
        public MainWindow()
        {
			GenerateFirstBuses();
            InitializeComponent();
            this.DataContext = buses;
        }
		void GenerateFirstBuses()
		{
            // Randomize first buses

            buses.Add(new Bus(new Registration(11111111, DateTime.Now)));
            buses.Add(new Bus(new Registration(22222222, DateTime.Now)));
            buses.Add(new Bus(new Registration(33333333, DateTime.Now)));
            buses.Add(new Bus(new Registration(44444444, DateTime.Now)));
            buses.Add(new Bus(new Registration(1234567, DateTime.Now.AddYears(-4))));
            buses.Add(new Bus(new Registration(2345678, DateTime.Now.AddYears(-4))));
            buses.Add(new Bus(new Registration(3456789, DateTime.Now.AddYears(-4))));
        }
    }
}
