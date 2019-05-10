using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class SliderHelper
	{
		public readonly static DependencyProperty ChangeValueByProperty;

		public readonly static DependencyProperty EnableMouseWheelProperty;

		static SliderHelper()
		{
			Class6.yDnXvgqzyB5jw();
			SliderHelper.ChangeValueByProperty = DependencyProperty.RegisterAttached("ChangeValueBy", typeof(MouseWheelChange), typeof(SliderHelper), new PropertyMetadata((object)MouseWheelChange.SmallChange));
			SliderHelper.EnableMouseWheelProperty = DependencyProperty.RegisterAttached("EnableMouseWheel", typeof(MouseWheelState), typeof(SliderHelper), new PropertyMetadata((object)MouseWheelState.None, new PropertyChangedCallback(SliderHelper.OnEnableMouseWheelChanged)));
		}

		public SliderHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[Category("MahApps.Metro")]
		public static MouseWheelChange GetChangeValueBy(Slider element)
		{
			return (MouseWheelChange)element.GetValue(SliderHelper.ChangeValueByProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[Category("MahApps.Metro")]
		public static MouseWheelState GetEnableMouseWheel(Slider element)
		{
			return (MouseWheelState)element.GetValue(SliderHelper.EnableMouseWheelProperty);
		}

		private static void OnEnableMouseWheelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Slider slider = d as Slider;
			if (slider != null)
			{
				SliderHelper.UnregisterEvents(slider);
				if ((MouseWheelState)e.NewValue != MouseWheelState.None)
				{
					SliderHelper.RegisterEvents(slider);
				}
			}
		}

		private static void OnPreviewMouseWheel(Slider sender, MouseWheelEventArgs e)
		{
			Slider slider = (Slider)sender;
			if (slider.IsFocused || MouseWheelState.MouseHover.Equals(slider.GetValue(SliderHelper.EnableMouseWheelProperty)))
			{
				double num = ((MouseWheelChange)slider.GetValue(SliderHelper.ChangeValueByProperty) == MouseWheelChange.LargeChange ? slider.LargeChange : slider.SmallChange);
				if (e.Delta > 0)
				{
					Slider value = slider;
					value.Value = value.Value + num;
					return;
				}
				Slider value1 = slider;
				value1.Value = value1.Value - num;
			}
		}

		private static void OnUnloaded(Slider sender, RoutedEventArgs e)
		{
			SliderHelper.UnregisterEvents((Slider)sender);
		}

		private static void RegisterEvents(Slider slider)
		{
			slider.Unloaded += new RoutedEventHandler(SliderHelper.OnUnloaded);
			slider.PreviewMouseWheel += new MouseWheelEventHandler(SliderHelper.OnPreviewMouseWheel);
		}

		public static void SetChangeValueBy(Slider element, MouseWheelChange value)
		{
			element.SetValue(SliderHelper.ChangeValueByProperty, value);
		}

		public static void SetEnableMouseWheel(Slider element, MouseWheelState value)
		{
			element.SetValue(SliderHelper.EnableMouseWheelProperty, value);
		}

		private static void UnregisterEvents(Slider slider)
		{
			slider.Unloaded -= new RoutedEventHandler(SliderHelper.OnUnloaded);
			slider.PreviewMouseWheel -= new MouseWheelEventHandler(SliderHelper.OnPreviewMouseWheel);
		}
	}
}