using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(IValueConverter))]
	public class ToUpperConverter : MarkupConverter
	{
		private static ToUpperConverter _instance;

		static ToUpperConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		public ToUpperConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = value as string;
			if (str == null)
			{
				return value;
			}
			return str.ToUpper();
		}

		protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			ToUpperConverter toUpperConverter = ToUpperConverter._instance;
			if (toUpperConverter == null)
			{
				toUpperConverter = new ToUpperConverter();
				ToUpperConverter._instance = toUpperConverter;
			}
			return toUpperConverter;
		}
	}
}