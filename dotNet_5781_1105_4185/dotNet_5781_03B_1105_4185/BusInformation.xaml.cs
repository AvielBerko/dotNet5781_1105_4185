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
using System.Windows.Shapes;

namespace dotNet_5781_03B_1105_4185
{
	/// <summary>
	/// Interaction logic for BusInformation.xaml
	/// </summary>
	public partial class BusInformation : Window
	{
		public BusInformation(Bus bus)
		{
			InitializeComponent();

			Bus = bus;
			DataContext = bus;
		}

		public Bus Bus { get; }

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

		private void TreatmentClick(object sender, RoutedEventArgs e)
		{
			var bus = ((Button)sender).DataContext as Bus;
			try
			{
				bus.Treate();
			}
			catch (BusException be)
			{
				MessageBox.Show(be.Message, "Bus Cannot Drive", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
