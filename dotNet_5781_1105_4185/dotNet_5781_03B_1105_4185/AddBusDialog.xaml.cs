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
		public Bus Bus => new Bus(new Registration(RegNum, RegDate), Kilometrage);

		public uint RegNum { get; set; }
		public DateTime RegDate { get; set; }
		public uint Kilometrage { get; set; }

		private void OkButtonClicked(object sender, RoutedEventArgs e)
		{
			if (!IsValid()) return;

			DialogResult = true;
		}

		private bool IsValid()
		{
			try
			{
				new Registration(RegNum, RegDate);
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Cannot create bus", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
		}
		private void TextBoxDigitOnly(object sender, TextCompositionEventArgs e)
		{
			var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			bool isDigitOnly = !regex.IsMatch(e.Text);
			e.Handled = !isDigitOnly;
		}
	}
}
