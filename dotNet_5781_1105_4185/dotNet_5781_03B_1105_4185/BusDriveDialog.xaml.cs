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
	/// Interaction logic for BusDriveDialog.xaml
	/// </summary>
	public partial class BusDriveDialog : Window
	{
		public BusDriveDialog(Bus bus)
		{
			InitializeComponent();

			DataContext = this;

			this.bus = bus;
			Title = bus.Registration.ToString();
		}

		private Bus bus;
		public uint Distance { get; set; }

		private void OkButtonClick(object sender, RoutedEventArgs e)
		{
			if (!bus.CanDrive(Distance))
			{
				MessageBox.Show("Cannot drive this distance!\n" +
					$"Can drive {bus.KmToRefuel} km until fuel tank is empty",
					"", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			DialogResult = true;
		}

		private void TextBoxDigitOnly(object sender, TextCompositionEventArgs e)
		{
			var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			bool isDigitOnly = !regex.IsMatch(e.Text);
			e.Handled = !isDigitOnly;
		}
	}
}
