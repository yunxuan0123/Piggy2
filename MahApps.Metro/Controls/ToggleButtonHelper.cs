using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public static class ToggleButtonHelper
	{
		public readonly static DependencyProperty ContentDirectionProperty;

		static ToggleButtonHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ToggleButtonHelper.ContentDirectionProperty = DependencyProperty.RegisterAttached("ContentDirection", typeof(FlowDirection), typeof(ToggleButtonHelper), new FrameworkPropertyMetadata((object)FlowDirection.LeftToRight, new PropertyChangedCallback(ToggleButtonHelper.ContentDirectionPropertyChanged)));
		}

		private static void ContentDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is ToggleButton))
			{
				throw new InvalidOperationException("The property 'ContentDirection' may only be set on ToggleButton elements.");
			}
		}

		[AttachedPropertyBrowsableForType(typeof(ToggleButton))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[Category("MahApps.Metro")]
		public static FlowDirection GetContentDirection(UIElement element)
		{
			return (FlowDirection)element.GetValue(ToggleButtonHelper.ContentDirectionProperty);
		}

		public static void SetContentDirection(UIElement element, FlowDirection value)
		{
			element.SetValue(ToggleButtonHelper.ContentDirectionProperty, value);
		}
	}
}