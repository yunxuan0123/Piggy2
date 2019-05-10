using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MahApps.Metro.Converters
{
	public class BackgroundToForegroundConverter : IMultiValueConverter, IValueConverter
	{
		private static BackgroundToForegroundConverter _instance;

		public static BackgroundToForegroundConverter Instance
		{
			get
			{
				BackgroundToForegroundConverter backgroundToForegroundConverter = BackgroundToForegroundConverter._instance;
				if (backgroundToForegroundConverter == null)
				{
					backgroundToForegroundConverter = new BackgroundToForegroundConverter();
					BackgroundToForegroundConverter._instance = backgroundToForegroundConverter;
				}
				return backgroundToForegroundConverter;
			}
		}

		static BackgroundToForegroundConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		private BackgroundToForegroundConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is SolidColorBrush))
			{
				return Brushes.White;
			}
			Color color = this.IdealTextColor(((SolidColorBrush)value).Color);
			SolidColorBrush solidColorBrush = new SolidColorBrush(color);
			solidColorBrush.Freeze();
			return solidColorBrush;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush;
			Brush brush1;
			if (values.Length != 0)
			{
				brush = values[0] as Brush;
			}
			else
			{
				brush = null;
			}
			Brush brush2 = brush;
			if ((int)values.Length > 1)
			{
				brush1 = values[1] as Brush;
			}
			else
			{
				brush1 = null;
			}
			Brush brush3 = brush1;
			if (brush3 != null)
			{
				return brush3;
			}
			return this.Convert(brush2, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return (
				from t in (IEnumerable<Type>)targetTypes
				select DependencyProperty.UnsetValue).ToArray<object>();
		}

		private Color IdealTextColor(Color bg)
		{
			return (255 - Convert.ToInt32((double)bg.R * 0.299 + (double)bg.G * 0.587 + (double)bg.B * 0.114) < 105 ? Colors.Black : Colors.White);
		}
	}
}