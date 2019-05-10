using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	[Obsolete("This converter will be deleted in the next release.")]
	public class OffOnConverter : IValueConverter
	{
		public OffOnConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)parameter;
			bool? isChecked = toggleSwitch.IsChecked;
			if ((isChecked.GetValueOrDefault() ? !isChecked.HasValue : true))
			{
				return toggleSwitch.OffLabel;
			}
			return toggleSwitch.OnLabel;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}