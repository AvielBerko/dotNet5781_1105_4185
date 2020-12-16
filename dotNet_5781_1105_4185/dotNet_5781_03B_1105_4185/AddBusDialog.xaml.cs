using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Interaction logic for AddBusDialog.xaml
	/// </summary>
	public partial class AddBusDialog : Window
	{
		public AddBusDialog()
		{
			InitializeComponent();

			DataContext = this;
		}

		/// <summary>
		/// Creates the bus, should be called once.
		/// </summary>
		private void GenerateBus() => new Bus(new Registration(RegNum, RegDate), Kilometrage);

		/// <summary>
		/// Registration Number
		/// </summary>
		public uint RegNum { get; set; }
		/// <summary>
		/// Registration Date
		/// </summary>
		public DateTime RegDate { get; set; }
		public uint Kilometrage { get; set; }

		private void OkButtonClicked(object sender, RoutedEventArgs e)
		{
			if (!IsValid()) return;

			// Closes the dialog with true as the result
			DialogResult = true;
		}

		/// <summary>
		/// Checks whether the bus registration is valid and is not existing.
		/// Generates the bus.
		/// </summary>
		/// <returns>True if bus is vaild, else False</returns>
		private bool IsValid()
		{
			try
			{
				GenerateBus();
				return true;
			}
			catch (RegistrationException e)
			{
				MessageBox.Show(e.Message, "Cannot create bus", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			catch (BusExistingException e)
			{
				MessageBox.Show(e.Message, "Cannot create bus", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
		}

		/// <summary>
		/// Called when the textboxes' text changed.
		/// Checks that the input contains only digits.
		/// </summary>
		private void TextBoxDigitOnly(object sender, TextCompositionEventArgs e)
		{
			var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			bool isDigitOnly = !regex.IsMatch(e.Text);
			e.Handled = !isDigitOnly;
		}
	}
}
