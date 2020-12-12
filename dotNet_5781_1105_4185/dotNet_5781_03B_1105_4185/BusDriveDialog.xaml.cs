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
		}

		private Bus bus;
		public uint Distance { get; set; }

		private void OkButtonClick(object sender, RoutedEventArgs e)
		{
			if (!bus.CanDrive(Distance))
			{
				MessageBox.Show("Cannot drive this distance!");
				return;
			}
			
			DialogResult = true;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
			bool isNotValid = regex.IsMatch(e.Text);
			e.Handled = isNotValid;
		}
	}
}
