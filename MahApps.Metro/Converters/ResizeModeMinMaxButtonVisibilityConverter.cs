using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public sealed class ResizeModeMinMaxButtonVisibilityConverter : IMultiValueConverter
	{
		private static ResizeModeMinMaxButtonVisibilityConverter _instance;

		public static ResizeModeMinMaxButtonVisibilityConverter Instance
		{
			get
			{
				ResizeModeMinMaxButtonVisibilityConverter resizeModeMinMaxButtonVisibilityConverter = ResizeModeMinMaxButtonVisibilityConverter._instance;
				if (resizeModeMinMaxButtonVisibilityConverter == null)
				{
					resizeModeMinMaxButtonVisibilityConverter = new ResizeModeMinMaxButtonVisibilityConverter();
					ResizeModeMinMaxButtonVisibilityConverter._instance = resizeModeMinMaxButtonVisibilityConverter;
				}
				return resizeModeMinMaxButtonVisibilityConverter;
			}
		}

		static ResizeModeMinMaxButtonVisibilityConverter()
		{
			Class6.yDnXvgqzyB5jw();
		}

		private ResizeModeMinMaxButtonVisibilityConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visibility;
			string str = parameter as string;
			if (values == null || string.IsNullOrEmpty(str))
			{
				return Visibility.Visible;
			}
			bool flag = (values.Length == 0 ? false : (bool)values[0]);
			bool flag1 = ((int)values.Length <= 1 ? false : (bool)values[1]);
			ResizeMode resizeMode = ((int)values.Length > 2 ? (ResizeMode)values[2] : ResizeMode.CanResize);
			if (str == "CLOSE")
			{
				return (flag1 || !flag ? Visibility.Collapsed : Visibility.Visible);
			}
			switch (resizeMode)
			{
				case ResizeMode.NoResize:
				{
					return Visibility.Collapsed;
				}
				case ResizeMode.CanMinimize:
				{
					if (str != "MIN")
					{
						return Visibility.Collapsed;
					}
					return (flag1 || !flag ? Visibility.Collapsed : Visibility.Visible);
				}
				case ResizeMode.CanResize:
				case ResizeMode.CanResizeWithGrip:
				{
					visibility = (flag1 || !flag ? Visibility.Collapsed : Visibility.Visible);
					return visibility;
				}
				default:
				{
					visibility = (flag1 || !flag ? Visibility.Collapsed : Visibility.Visible);
					return visibility;
				}
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return (
				from t in (IEnumerable<Type>)targetTypes
				select DependencyProperty.UnsetValue).ToArray<object>();
		}
	}
}