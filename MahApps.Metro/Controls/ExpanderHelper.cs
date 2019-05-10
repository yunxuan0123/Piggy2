using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class ExpanderHelper
	{
		public readonly static DependencyProperty HeaderUpStyleProperty;

		public readonly static DependencyProperty HeaderDownStyleProperty;

		public readonly static DependencyProperty HeaderLeftStyleProperty;

		public readonly static DependencyProperty HeaderRightStyleProperty;

		static ExpanderHelper()
		{
			Class6.yDnXvgqzyB5jw();
			ExpanderHelper.HeaderUpStyleProperty = DependencyProperty.RegisterAttached("HeaderUpStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
			ExpanderHelper.HeaderDownStyleProperty = DependencyProperty.RegisterAttached("HeaderDownStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
			ExpanderHelper.HeaderLeftStyleProperty = DependencyProperty.RegisterAttached("HeaderLeftStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
			ExpanderHelper.HeaderRightStyleProperty = DependencyProperty.RegisterAttached("HeaderRightStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
		}

		[AttachedPropertyBrowsableForType(typeof(Expander))]
		[Category("MahApps.Metro")]
		public static Style GetHeaderDownStyle(UIElement element)
		{
			return (Style)element.GetValue(ExpanderHelper.HeaderDownStyleProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Expander))]
		[Category("MahApps.Metro")]
		public static Style GetHeaderLeftStyle(UIElement element)
		{
			return (Style)element.GetValue(ExpanderHelper.HeaderLeftStyleProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Expander))]
		[Category("MahApps.Metro")]
		public static Style GetHeaderRightStyle(UIElement element)
		{
			return (Style)element.GetValue(ExpanderHelper.HeaderRightStyleProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(Expander))]
		[Category("MahApps.Metro")]
		public static Style GetHeaderUpStyle(UIElement element)
		{
			return (Style)element.GetValue(ExpanderHelper.HeaderUpStyleProperty);
		}

		public static void SetHeaderDownStyle(UIElement element, Style value)
		{
			element.SetValue(ExpanderHelper.HeaderDownStyleProperty, value);
		}

		public static void SetHeaderLeftStyle(UIElement element, Style value)
		{
			element.SetValue(ExpanderHelper.HeaderLeftStyleProperty, value);
		}

		public static void SetHeaderRightStyle(UIElement element, Style value)
		{
			element.SetValue(ExpanderHelper.HeaderRightStyleProperty, value);
		}

		public static void SetHeaderUpStyle(UIElement element, Style value)
		{
			element.SetValue(ExpanderHelper.HeaderUpStyleProperty, value);
		}
	}
}