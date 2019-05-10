using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public sealed class IsNullConverter : IValueConverter
	{
		private static IsNullConverter _instance;

		public static IsNullConverter Instance
		{
			get
			{
				IsNullConverter isNullConverter = IsNullConverter._instance;
				if (isNullConverter == null)
				{
					isNullConverter = new IsNullConverter();
					IsNullConverter._instance = isNullConverter;
				}
				return isNullConverter;
			}
		}

		static IsNullConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		private IsNullConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}