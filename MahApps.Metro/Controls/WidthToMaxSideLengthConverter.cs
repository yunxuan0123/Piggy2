using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Controls
{
	internal class WidthToMaxSideLengthConverter : IValueConverter
	{
		public WidthToMaxSideLengthConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double))
			{
				return null;
			}
			double num = (double)value;
			return (num <= 20 ? 20 : num);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}