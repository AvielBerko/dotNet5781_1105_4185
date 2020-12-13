using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
		ObservableCollection<Bus> buses = new ObservableCollection<Bus>();

		public MainWindow()
		{
			GenerateFirstBuses();
			InitializeComponent();

			lstBuses.DataContext = buses;
		}

		private void AddBusClick(object sender, RoutedEventArgs e)
		{
			var dialog = new AddBusDialog();
			dialog.Owner = this;
			if (dialog.ShowDialog() == true)
			{
				buses.Add(dialog.Bus);
			}
		}

		private void ListDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var bus = ((FrameworkElement)e.OriginalSource).DataContext as Bus;
			if (bus != null)
			{
				var information = new BusInformation(bus);
				information.Owner = this;
				information.Show();
			}
		}

		private void DriveClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;

			var dialog = new BusDriveDialog(bus);
			dialog.Owner = this;
			if (dialog.ShowDialog() == true)
			{
				bus.Drive(dialog.Distance);
			}
		}

		private void RefuelClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;
			bus.Refuel();
		}

		private void RemoveClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;
			buses.Remove(bus);
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
