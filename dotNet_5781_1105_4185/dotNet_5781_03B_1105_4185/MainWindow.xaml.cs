using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
		// List of all information windows.
		// used in order to close all the windows
		// when their bus is removed.
		List<BusInformation> informationWindows = new List<BusInformation>();
		public MainWindow()
		{
			GenerateFirstBuses();
			InitializeComponent();

			lstBuses.DataContext = Bus.Buses;
		}

		/// <summary>
		/// Called when a click event is called on Add Bus button.
		/// </summary>
		private void AddBusClick(object sender, RoutedEventArgs e)
		{
			var dialog = new AddBusDialog();
			dialog.Owner = this;
			dialog.ShowDialog();
		}

		/// <summary>
		/// Called when a double click event is called on the list.
		/// </summary>
		private void ListDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var bus = ((FrameworkElement)e.OriginalSource).DataContext as Bus;
			if (bus != null)
			{
				var information = new BusInformation(bus);
				informationWindows.Add(information);

				// Remove the window from the list when it is closed.
				information.Closing += (_sender, _e) => informationWindows.Remove(information);

				information.Owner = this;
				information.Show();
			}
		}

		/// <summary>
		/// Called when a click event is called on drive click of a bus.
		/// </summary>
		private void DriveClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;

			var dialog = new BusDriveDialog(bus);
			dialog.Owner = this;

			// Shows the dialog to get the distance.
			if (dialog.ShowDialog() == true)
			{
				try
				{
					bus.Drive(dialog.Distance);
				}
				catch (BusException be)
				{
					MessageBox.Show(be.Message, "Bus Cannot Drive", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		/// <summary>
		/// Called when a click event is called on refuel click of a bus.
		/// </summary>
		private void RefuelClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;
			try
			{
				bus.Refuel();
			}
			catch (BusException be)
			{
				MessageBox.Show(be.Message, "Bus Cannot Drive", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Called when a click event is called on remove click of a bus.
		/// </summary>
		private void RemoveClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;
			var result = MessageBox.Show($"Are you sure you want to remove bus number {bus.Registration}?",
				"Remove bus", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				Bus.Buses.Remove(bus);

				// Close all information windows of this bus.
				for (int i = 0; i < informationWindows.Count; i++)
				{
					if (informationWindows[i].Bus == bus)
					{
						informationWindows[i--].Close();
					}
				}
			}
		}

		/// <summary>
		/// Called when a click event is called on Remove All button.
		/// </summary>
		private void RemoveAllClick(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show($"Are you sure you want to remove ALL buses?", "Remove All", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				Bus.Buses.Clear();
				
				// Close all inofrmation windows.
				for (int i = 0; i < informationWindows.Count; i++)
				{
					informationWindows[i--].Close();
				}
			}
		}

		void GenerateFirstBuses()
		{
			// Randomize first buses

			for (int i = 0; i < 7; i++)
			{
				Bus.Random();
			}

			Bus.Random(needTreatmentForTime: true);
			Bus.Random(needTreatmentForDistance: true);
			Bus.Random(lowFuel: true);

			Bus.Shuffle();
		}

	}
}
