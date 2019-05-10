using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(IValueConverter))]
	public class ToLowerConverter : MarkupConverter
	{
		private static ToLowerConverter _instance;

		static ToLowerConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		public ToLowerConverter()
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
			return str.ToLower();
		}

		protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			ToLowerConverter toLowerConverter = ToLowerConverter._instance;
			if (toLowerConverter == null)
			{
				toLowerConverter = new ToLowerConverter();
				ToLowerConverter._instance = toLowerConverter;
			}
			return toLowerConverter;
		}
	}
}