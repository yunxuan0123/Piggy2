using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(IValueConverter))]
	public abstract class MarkupConverter : MarkupExtension, IValueConverter
	{
		protected MarkupConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		object System.Windows.Data.IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object unsetValue;
			try
			{
				unsetValue = this.Convert(value, targetType, parameter, culture);
			}
			catch
			{
				unsetValue = DependencyProperty.UnsetValue;
			}
			return unsetValue;
		}

		object System.Windows.Data.IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object unsetValue;
			try
			{
				unsetValue = this.ConvertBack(value, targetType, parameter, culture);
			}
			catch
			{
				unsetValue = DependencyProperty.UnsetValue;
			}
			return unsetValue;
		}
	}
}