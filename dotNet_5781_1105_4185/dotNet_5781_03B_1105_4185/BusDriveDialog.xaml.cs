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
			Title = $"Bus {bus.Registration}";
		}

		private Bus bus;

		/// <summary>
		/// The distance input
		/// </summary>
		public uint Distance { get; set; }

		/// <summary>
		/// Called when the textbox's text changed.
		/// Checks that the input contains only digits.
		/// </summary>
		private void TextBoxDigitOnly(object sender, TextCompositionEventArgs e)
		{
			var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			bool isDigitOnly = !regex.IsMatch(e.Text);
			e.Handled = !isDigitOnly;
		}

		/// <summary>
		/// Called when KeyDown event triggred in the textbox.
		/// Closes the dialog if presesd Enter or Escape.
		/// </summary>
		private void KeyDownHandling(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (!bus.CanDriveDistance(Distance))
				{
					MessageBox.Show("Cannot drive this distance!\n" +
						$"Can drive {bus.KmToRefuel} km until fuel tank is empty",
						"", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// Closes the dialog with true as a result.
				DialogResult = true;
			}

			if (e.Key == Key.Escape)
			{
				// Closes the dialog with false as a result.
				DialogResult = false;
			}
		}
	}
}
