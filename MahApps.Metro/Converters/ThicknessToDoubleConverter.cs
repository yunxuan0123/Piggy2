using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class ThicknessToDoubleConverter : IValueConverter
	{
		public ThicknessToDoubleConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((Thickness)value).Left;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}