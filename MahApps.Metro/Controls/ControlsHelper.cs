using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class ControlsHelper
	{
		public readonly static DependencyProperty DisabledVisualElementVisibilityProperty;

		public readonly static DependencyProperty ContentCharacterCasingProperty;

		public readonly static DependencyProperty HeaderFontSizeProperty;

		public readonly static DependencyProperty HeaderFontStretchProperty;

		public readonly static DependencyProperty HeaderFontWeightProperty;

		public readonly static DependencyProperty ButtonWidthProperty;

		public readonly static DependencyProperty FocusBorderBrushProperty;

		public readonly static DependencyProperty MouseOverBorderBrushProperty;

		public readonly static DependencyProperty CornerRadiusProperty;

		static ControlsHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ControlsHelper.DisabledVisualElementVisibilityProperty = DependencyProperty.RegisterAttached("DisabledVisualElementVisibility", typeof(Visibility), typeof(ControlsHelper), new FrameworkPropertyMetadata((object)Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
			ControlsHelper.ContentCharacterCasingProperty = DependencyProperty.RegisterAttached("ContentCharacterCasing", typeof(CharacterCasing), typeof(ControlsHelper), new FrameworkPropertyMetadata((object)CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure), (object value) => {
				if (CharacterCasing.Normal > (CharacterCasing)value)
				{
					return false;
				}
				return (CharacterCasing)value <= CharacterCasing.Upper;
			});
			ControlsHelper.HeaderFontSizeProperty = DependencyProperty.RegisterAttached("HeaderFontSize", typeof(double), typeof(ControlsHelper), new FrameworkPropertyMetadata((object)26.67, new PropertyChangedCallback(ControlsHelper.HeaderFontSizePropertyChangedCallback))
			{
				Inherits = true
			});
			ControlsHelper.HeaderFontStretchProperty = DependencyProperty.RegisterAttached("HeaderFontStretch", typeof(FontStretch), typeof(ControlsHelper), new UIPropertyMetadata((object)FontStretches.Normal));
			ControlsHelper.HeaderFontWeightProperty = DependencyProperty.RegisterAttached("HeaderFontWeight", typeof(FontWeight), typeof(ControlsHelper), new UIPropertyMetadata((object)FontWeights.Normal));
			ControlsHelper.ButtonWidthProperty = DependencyProperty.RegisterAttached("ButtonWidth", typeof(double), typeof(ControlsHelper), new FrameworkPropertyMetadata((object)22, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			ControlsHelper.FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
			ControlsHelper.MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
			Type type = typeof(CornerRadius);
			Type type1 = typeof(ControlsHelper);
			CornerRadius cornerRadiu = new CornerRadius();
			ControlsHelper.CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", type, type1, new FrameworkPropertyMetadata((object)cornerRadiu, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
		}

		[Category("MahApps.Metro")]
		public static double GetButtonWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(ControlsHelper.ButtonWidthProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(DropDownButton))]
		[AttachedPropertyBrowsableForType(typeof(WindowCommands))]
		[AttachedPropertyBrowsableForType(typeof(ContentControl))]
		[Category("MahApps.Metro")]
		public static CharacterCasing GetContentCharacterCasing(UIElement element)
		{
			return (CharacterCasing)element.GetValue(ControlsHelper.ContentCharacterCasingProperty);
		}

		[Category("MahApps.Metro")]
		public static CornerRadius GetCornerRadius(UIElement element)
		{
			return (CornerRadius)element.GetValue(ControlsHelper.CornerRadiusProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[Category("MahApps.Metro")]
		public static Visibility GetDisabledVisualElementVisibility(UIElement element)
		{
			return (Visibility)element.GetValue(ControlsHelper.DisabledVisualElementVisibilityProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(CheckBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[Category("MahApps.Metro")]
		public static Brush GetFocusBorderBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(ControlsHelper.FocusBorderBrushProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(MetroTabItem))]
		[AttachedPropertyBrowsableForType(typeof(GroupBox))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		[Category("MahApps.Metro")]
		public static double GetHeaderFontSize(UIElement element)
		{
			return (double)element.GetValue(ControlsHelper.HeaderFontSizeProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(MetroTabItem))]
		[AttachedPropertyBrowsableForType(typeof(GroupBox))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		[Category("MahApps.Metro")]
		public static FontStretch GetHeaderFontStretch(UIElement element)
		{
			return (FontStretch)element.GetValue(ControlsHelper.HeaderFontStretchProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(MetroTabItem))]
		[AttachedPropertyBrowsableForType(typeof(GroupBox))]
		[AttachedPropertyBrowsableForType(typeof(TabItem))]
		[Category("MahApps.Metro")]
		public static FontWeight GetHeaderFontWeight(UIElement element)
		{
			return (FontWeight)element.GetValue(ControlsHelper.HeaderFontWeightProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Tile))]
		[AttachedPropertyBrowsableForType(typeof(CheckBox))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[Category("MahApps.Metro")]
		public static Brush GetMouseOverBorderBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(ControlsHelper.MouseOverBorderBrushProperty);
		}

		private static void HeaderFontSizePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is double)
			{
				MetroTabItem thickness = dependencyObject as MetroTabItem;
				if (thickness == null)
				{
					return;
				}
				if (thickness.closeButton == null)
				{
					thickness.ApplyTemplate();
				}
				if (thickness.closeButton != null && thickness.contentSite != null)
				{
					double newValue = (double)e.NewValue;
					double num = Math.Ceiling(newValue * thickness.FontFamily.LineSpacing);
					double num1 = Math.Round(num) / 2.8;
					Thickness padding = thickness.Padding;
					double top = num1 - padding.Top;
					padding = thickness.Padding;
					double bottom = top - padding.Bottom;
					padding = thickness.contentSite.Margin;
					double top1 = bottom - padding.Top;
					padding = thickness.contentSite.Margin;
					double bottom1 = top1 - padding.Bottom;
					Thickness margin = thickness.closeButton.Margin;
					thickness.newButtonMargin = new Thickness(margin.Left, bottom1, margin.Right, margin.Bottom);
					thickness.closeButton.Margin = thickness.newButtonMargin;
					thickness.closeButton.UpdateLayout();
				}
			}
		}

		public static void SetButtonWidth(DependencyObject obj, double value)
		{
			obj.SetValue(ControlsHelper.ButtonWidthProperty, value);
		}

		public static void SetContentCharacterCasing(UIElement element, CharacterCasing value)
		{
			element.SetValue(ControlsHelper.ContentCharacterCasingProperty, value);
		}

		public static void SetCornerRadius(UIElement element, CornerRadius value)
		{
			element.SetValue(ControlsHelper.CornerRadiusProperty, value);
		}

		public static void SetDisabledVisualElementVisibility(UIElement element, Visibility value)
		{
			element.SetValue(ControlsHelper.DisabledVisualElementVisibilityProperty, value);
		}

		public static void SetFocusBorderBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(ControlsHelper.FocusBorderBrushProperty, value);
		}

		public static void SetHeaderFontSize(UIElement element, double value)
		{
			element.SetValue(ControlsHelper.HeaderFontSizeProperty, value);
		}

		public static void SetHeaderFontStretch(UIElement element, FontStretch value)
		{
			element.SetValue(ControlsHelper.HeaderFontStretchProperty, value);
		}

		public static void SetHeaderFontWeight(UIElement element, FontWeight value)
		{
			element.SetValue(ControlsHelper.HeaderFontWeightProperty, value);
		}

		public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(ControlsHelper.MouseOverBorderBrushProperty, value);
		}
	}
}