using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class FontSizeOffsetConverter : IValueConverter
	{
		private static FontSizeOffsetConverter _instance;

		public static FontSizeOffsetConverter Instance
		{
			get
			{
				FontSizeOffsetConverter fontSizeOffsetConverter = FontSizeOffsetConverter._instance;
				if (fontSizeOffsetConverter == null)
				{
					fontSizeOffsetConverter = new FontSizeOffsetConverter();
					FontSizeOffsetConverter._instance = fontSizeOffsetConverter;
				}
				return fontSizeOffsetConverter;
			}
		}

		static FontSizeOffsetConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		private FontSizeOffsetConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is double) || !(parameter is double))
			{
				return value;
			}
			double num = (double)parameter;
			return Math.Round((double)value + num);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}