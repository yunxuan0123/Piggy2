using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class TabControlHelper
	{
		public readonly static DependencyProperty IsUnderlinedProperty;

		public readonly static DependencyProperty TransitionProperty;

		static TabControlHelper()
		{
			Class6.yDnXvgqzyB5jw();
			TabControlHelper.IsUnderlinedProperty = DependencyProperty.RegisterAttached("IsUnderlined", typeof(bool), typeof(TabControlHelper), new PropertyMetadata(false));
			TabControlHelper.TransitionProperty = DependencyProperty.RegisterAttached("Transition", typeof(TransitionType), typeof(TabControlHelper), new FrameworkPropertyMetadata((object)TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
		}

		[AttachedPropertyBrowsableForType(typeof(TabControl))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		[Category("MahApps.Metro")]
		public static bool GetIsUnderlined(UIElement element)
		{
			return (bool)element.GetValue(TabControlHelper.IsUnderlinedProperty);
		}

		[Category("MahApps.Metro")]
		public static TransitionType GetTransition(DependencyObject obj)
		{
			return (TransitionType)obj.GetValue(TabControlHelper.TransitionProperty);
		}

		public static void SetIsUnderlined(UIElement element, bool value)
		{
			element.SetValue(TabControlHelper.IsUnderlinedProperty, value);
		}

		public static void SetTransition(DependencyObject obj, TransitionType value)
		{
			obj.SetValue(TabControlHelper.TransitionProperty, value);
		}
	}
}