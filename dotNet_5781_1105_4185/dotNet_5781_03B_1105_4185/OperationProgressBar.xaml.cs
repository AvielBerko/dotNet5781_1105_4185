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

namespace dotNet_5781_03B_1105_4185
{
	/// <summary>
	/// Interaction logic for OperationProgressBar.xaml
	/// </summary>
	public partial class OperationProgressBar : UserControl
	{
		public OperationProgressBar()
		{
			InitializeComponent();
		}

		public Bus Bus
		{
			get => (Bus)GetValue(BusProperty);
			set => SetValue(BusProperty, value);
		}

		public static readonly DependencyProperty BusProperty =
			DependencyProperty.Register("Bus", typeof(Bus), typeof(OperationProgressBar));
	}
}
