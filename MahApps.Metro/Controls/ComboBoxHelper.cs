using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ComboBoxHelper
	{
		public readonly static DependencyProperty EnableVirtualizationWithGroupingProperty;

		public readonly static DependencyProperty MaxLengthProperty;

		public readonly static DependencyProperty CharacterCasingProperty;

		static ComboBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ComboBoxHelper.EnableVirtualizationWithGroupingProperty = DependencyProperty.RegisterAttached("EnableVirtualizationWithGrouping", typeof(bool), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ComboBoxHelper.EnableVirtualizationWithGroupingPropertyChangedCallback)));
			ComboBoxHelper.MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(ComboBoxHelper), new FrameworkPropertyMetadata((object)0), new ValidateValueCallback(ComboBoxHelper.MaxLengthValidateValue));
			ComboBoxHelper.CharacterCasingProperty = DependencyProperty.RegisterAttached("CharacterCasing", typeof(CharacterCasing), typeof(ComboBoxHelper), new FrameworkPropertyMetadata((object)CharacterCasing.Normal), new ValidateValueCallback(ComboBoxHelper.CharacterCasingValidateValue));
		}

		public ComboBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private static bool CharacterCasingValidateValue(object value)
		{
			if (CharacterCasing.Normal > (CharacterCasing)value)
			{
				return false;
			}
			return (CharacterCasing)value <= CharacterCasing.Upper;
		}

		private static void EnableVirtualizationWithGroupingPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = dependencyObject as ComboBox;
			if (comboBox != null && e.NewValue != e.OldValue)
			{
				comboBox.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, e.NewValue);
				comboBox.SetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, e.NewValue);
				comboBox.SetValue(ScrollViewer.CanContentScrollProperty, e.NewValue);
			}
		}

		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[Category("MahApps.Metro")]
		public static CharacterCasing GetCharacterCasing(UIElement obj)
		{
			return (CharacterCasing)obj.GetValue(ComboBoxHelper.CharacterCasingProperty);
		}

		[Category("MahApps.Metro")]
		public static bool GetEnableVirtualizationWithGrouping(DependencyObject obj)
		{
			return (bool)obj.GetValue(ComboBoxHelper.EnableVirtualizationWithGroupingProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[Category("MahApps.Metro")]
		public static int GetMaxLength(UIElement obj)
		{
			return (int)obj.GetValue(ComboBoxHelper.MaxLengthProperty);
		}

		private static bool MaxLengthValidateValue(object value)
		{
			return (int)value >= 0;
		}

		public static void SetCharacterCasing(UIElement obj, CharacterCasing value)
		{
			obj.SetValue(ComboBoxHelper.CharacterCasingProperty, value);
		}

		public static void SetEnableVirtualizationWithGrouping(DependencyObject obj, bool value)
		{
			obj.SetValue(ComboBoxHelper.EnableVirtualizationWithGroupingProperty, value);
		}

		public static void SetMaxLength(UIElement obj, int value)
		{
			obj.SetValue(ComboBoxHelper.MaxLengthProperty, value);
		}
	}
}