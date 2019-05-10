using System;
using System.Globalization;
using System.Windows.Data;

namespace Aries.Converters
{
	public class NullToFalseConverter : IValueConverter
	{
		public NullToFalseConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return value;
			}
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}