using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class ScrollBarHelper
	{
		public readonly static DependencyProperty VerticalScrollBarOnLeftSideProperty;

		static ScrollBarHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ScrollBarHelper.VerticalScrollBarOnLeftSideProperty = DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide", typeof(bool), typeof(ScrollBarHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
		}

		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		[Category("MahApps.Metro")]
		public static bool GetVerticalScrollBarOnLeftSide(ScrollViewer obj)
		{
			return (bool)obj.GetValue(ScrollBarHelper.VerticalScrollBarOnLeftSideProperty);
		}

		public static void SetVerticalScrollBarOnLeftSide(ScrollViewer obj, bool value)
		{
			obj.SetValue(ScrollBarHelper.VerticalScrollBarOnLeftSideProperty, value);
		}
	}
}