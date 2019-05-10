using System;
using System.ComponentModel;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public static class VisibilityHelper
	{
		public readonly static DependencyProperty IsVisibleProperty;

		public readonly static DependencyProperty IsCollapsedProperty;

		public readonly static DependencyProperty IsHiddenProperty;

		static VisibilityHelper()
		{
			Class6.yDnXvgqzyB5jw();
			VisibilityHelper.IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsVisibleChangedCallback)));
			VisibilityHelper.IsCollapsedProperty = DependencyProperty.RegisterAttached("IsCollapsed", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsCollapsedChangedCallback)));
			VisibilityHelper.IsHiddenProperty = DependencyProperty.RegisterAttached("IsHidden", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(VisibilityHelper.IsHiddenChangedCallback)));
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsCollapsed(DependencyObject element)
		{
			return (bool?)element.GetValue(VisibilityHelper.IsCollapsedProperty);
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsHidden(DependencyObject element)
		{
			return (bool?)element.GetValue(VisibilityHelper.IsHiddenProperty);
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsVisible(DependencyObject element)
		{
			return (bool?)element.GetValue(VisibilityHelper.IsVisibleProperty);
		}

		private static void IsCollapsedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement1 = frameworkElement;
			bool? newValue = (bool?)e.NewValue;
			frameworkElement1.Visibility = ((newValue.GetValueOrDefault() ? newValue.HasValue : false) ? Visibility.Collapsed : Visibility.Visible);
		}

		private static void IsHiddenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement1 = frameworkElement;
			bool? newValue = (bool?)e.NewValue;
			frameworkElement1.Visibility = ((newValue.GetValueOrDefault() ? newValue.HasValue : false) ? Visibility.Hidden : Visibility.Visible);
		}

		private static void IsVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement1 = frameworkElement;
			bool? newValue = (bool?)e.NewValue;
			frameworkElement1.Visibility = ((newValue.GetValueOrDefault() ? newValue.HasValue : false) ? Visibility.Visible : Visibility.Collapsed);
		}

		public static void SetIsCollapsed(DependencyObject element, bool? value)
		{
			element.SetValue(VisibilityHelper.IsCollapsedProperty, value);
		}

		public static void SetIsHidden(DependencyObject element, bool? value)
		{
			element.SetValue(VisibilityHelper.IsHiddenProperty, value);
		}

		public static void SetIsVisible(DependencyObject element, bool? value)
		{
			element.SetValue(VisibilityHelper.IsVisibleProperty, value);
		}
	}
}