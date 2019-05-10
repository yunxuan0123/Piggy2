using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class ThicknessBindingConverter : IValueConverter
	{
		public IgnoreThicknessSideType IgnoreThicknessSide
		{
			get;
			set;
		}

		public ThicknessBindingConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Thickness))
			{
				return new Thickness();
			}
			if (parameter as IgnoreThicknessSideType != IgnoreThicknessSideType.None)
			{
				this.IgnoreThicknessSide = (IgnoreThicknessSideType)parameter;
			}
			Thickness thickness = (Thickness)value;
			switch (this.IgnoreThicknessSide)
			{
				case IgnoreThicknessSideType.Left:
				{
					return new Thickness(0, thickness.Top, thickness.Right, thickness.Bottom);
				}
				case IgnoreThicknessSideType.Top:
				{
					return new Thickness(thickness.Left, 0, thickness.Right, thickness.Bottom);
				}
				case IgnoreThicknessSideType.Right:
				{
					return new Thickness(thickness.Left, thickness.Top, 0, thickness.Bottom);
				}
				case IgnoreThicknessSideType.Bottom:
				{
					return new Thickness(thickness.Left, thickness.Top, thickness.Right, 0);
				}
				default:
				{
					return thickness;
				}
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}