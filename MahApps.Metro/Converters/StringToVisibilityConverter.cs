using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(StringToVisibilityConverter))]
	[ValueConversion(typeof(string), typeof(Visibility))]
	public class StringToVisibilityConverter : MarkupExtension, IValueConverter
	{
		public Visibility FalseEquivalent
		{
			get;
			set;
		}

		public bool OppositeStringValue
		{
			get;
			set;
		}

		public StringToVisibilityConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.FalseEquivalent = Visibility.Collapsed;
			this.OppositeStringValue = false;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is string) || !(targetType == typeof(Visibility)))
			{
				return value;
			}
			if (this.OppositeStringValue)
			{
				return (((string)value).ToLower().Equals(string.Empty) ? Visibility.Visible : this.FalseEquivalent);
			}
			return (((string)value).ToLower().Equals(string.Empty) ? this.FalseEquivalent : Visibility.Visible);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Visibility))
			{
				return value;
			}
			if (this.OppositeStringValue)
			{
				if ((Visibility)value != Visibility.Visible)
				{
					return "visible";
				}
				return string.Empty;
			}
			if ((Visibility)value != Visibility.Visible)
			{
				return string.Empty;
			}
			return "visible";
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new StringToVisibilityConverter()
			{
				FalseEquivalent = this.FalseEquivalent,
				OppositeStringValue = this.OppositeStringValue
			};
		}
	}
}