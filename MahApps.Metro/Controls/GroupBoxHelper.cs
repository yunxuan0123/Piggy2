using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class GroupBoxHelper
	{
		public readonly static DependencyProperty HeaderForegroundProperty;

		static GroupBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			GroupBoxHelper.HeaderForegroundProperty = DependencyProperty.RegisterAttached("HeaderForeground", typeof(Brush), typeof(GroupBoxHelper), new UIPropertyMetadata(Brushes.White));
		}

		[AttachedPropertyBrowsableForType(typeof(Expander))]
		[AttachedPropertyBrowsableForType(typeof(GroupBox))]
		[Category("MahApps.Metro")]
		public static Brush GetHeaderForeground(UIElement element)
		{
			return (Brush)element.GetValue(GroupBoxHelper.HeaderForegroundProperty);
		}

		public static void SetHeaderForeground(UIElement element, Brush value)
		{
			element.SetValue(GroupBoxHelper.HeaderForegroundProperty, value);
		}
	}
}