using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public static class ButtonHelper
	{
		[Obsolete("This property will be deleted in the next release. You should use ContentCharacterCasing attached property located in ControlsHelper.")]
		public readonly static DependencyProperty PreserveTextCaseProperty;

		[Obsolete("This property will be deleted in the next release. You should use CornerRadius attached property located in ControlsHelper.")]
		public readonly static DependencyProperty CornerRadiusProperty;

		static ButtonHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ButtonHelper.PreserveTextCaseProperty = DependencyProperty.RegisterAttached("PreserveTextCase", typeof(bool), typeof(ButtonHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(ButtonHelper.PreserveTextCasePropertyChangedCallback)));
			Type type = typeof(CornerRadius);
			Type type1 = typeof(ButtonHelper);
			CornerRadius cornerRadiu = new CornerRadius();
			ButtonHelper.CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", type, type1, new FrameworkPropertyMetadata((object)cornerRadiu, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
		}

		[AttachedPropertyBrowsableForType(typeof(Button))]
		[AttachedPropertyBrowsableForType(typeof(ToggleButton))]
		[Category("MahApps.Metro")]
		public static CornerRadius GetCornerRadius(UIElement element)
		{
			return ControlsHelper.GetCornerRadius(element);
		}

		[AttachedPropertyBrowsableForType(typeof(Button))]
		[Category("MahApps.Metro")]
		public static bool GetPreserveTextCase(UIElement element)
		{
			return (bool)element.GetValue(ButtonHelper.PreserveTextCaseProperty);
		}

		private static void PreserveTextCasePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is bool)
			{
				Button button = dependencyObject as Button;
				if (button != null)
				{
					ControlsHelper.SetContentCharacterCasing(button, ((bool)e.NewValue ? CharacterCasing.Normal : CharacterCasing.Upper));
				}
			}
		}

		public static void SetCornerRadius(UIElement element, CornerRadius value)
		{
			ControlsHelper.SetCornerRadius(element, value);
		}

		public static void SetPreserveTextCase(UIElement element, bool value)
		{
			element.SetValue(ButtonHelper.PreserveTextCaseProperty, value);
		}
	}
}