using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class TreeViewMarginConverter : IValueConverter
	{
		public double Length
		{
			get;
			set;
		}

		public TreeViewMarginConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TreeViewItem treeViewItem = value as TreeViewItem;
			if (treeViewItem == null)
			{
				return new Thickness(0);
			}
			return new Thickness(this.Length * (double)treeViewItem.GetDepth(), 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}