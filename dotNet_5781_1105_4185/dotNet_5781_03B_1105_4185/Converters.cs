﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace dotNet_5781_03B_1105_4185
{
	/// <summary>
	/// Converts from Status to string in order to show different colors for read, need refueling and in operation.
	/// </summary>
	public class StatusColorConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((Status)value)
			{
				case Status.Ready:
					return "Ready";
				case Status.NeedRefueling:
					return "Need Refueling";
				default:
					return "Else";
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Converts from status to bool if in operation and need to show the progress.
	/// </summary>
	public class ShowProgressBarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Status s = (Status)value;
			return s != Status.Ready && s != Status.NeedTreatment && s != Status.NeedRefueling;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Converts from percentage to bool if its under 30% in order to show red progress bar
	/// </summary>
	public class RedFuelBarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (double)value <= 30;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Converts from TimeSpan to string in order to format the time span as we want.
	/// </summary>
	public class TimeSpanToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var timeSpan = (TimeSpan)value;
			string result = "";

			if (timeSpan.Days > 0)
			{
				result += timeSpan.Days + " Day(s) ";
			}

			result += timeSpan.ToString(@"hh\:mm");

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Converts from Status to string in order to format the status
	/// </summary>
	public class StatusToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((Status)value)
			{
				case Status.Ready:
					return "Ready";
				case Status.NeedTreatment:
					return "Need Treatment";
				case Status.Driving:
					return "Driving";
				case Status.Refueling:
					return "Refueling";
				case Status.InTreatment:
					return "In Treatment";
				case Status.NeedRefueling:
					return "Need Refueling";
				default:
					throw new InvalidOperationException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
