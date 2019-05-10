using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Behaviours
{
	public static class ReloadBehavior
	{
		public static DependencyProperty OnDataContextChangedProperty;

		public static DependencyProperty OnSelectedTabChangedProperty;

		public readonly static DependencyProperty MetroContentControlProperty;

		static ReloadBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			ReloadBehavior.OnDataContextChangedProperty = DependencyProperty.RegisterAttached("OnDataContextChanged", typeof(bool), typeof(ReloadBehavior), new PropertyMetadata(new PropertyChangedCallback(ReloadBehavior.OnDataContextChanged)));
			ReloadBehavior.OnSelectedTabChangedProperty = DependencyProperty.RegisterAttached("OnSelectedTabChanged", typeof(bool), typeof(ReloadBehavior), new PropertyMetadata(new PropertyChangedCallback(ReloadBehavior.OnSelectedTabChanged)));
			ReloadBehavior.MetroContentControlProperty = DependencyProperty.RegisterAttached("MetroContentControl", typeof(ContentControl), typeof(ReloadBehavior), new PropertyMetadata(null));
		}

		private static IEnumerable<DependencyObject> Ancestors(DependencyObject obj)
		{
			for (DependencyObject i = VisualTreeHelper.GetParent(obj); i != null; i = VisualTreeHelper.GetParent(obj))
			{
				yield return i;
				obj = i;
			}
		}

		[Category("MahApps.Metro")]
		public static ContentControl GetMetroContentControl(UIElement element)
		{
			return (ContentControl)element.GetValue(ReloadBehavior.MetroContentControlProperty);
		}

		[Category("MahApps.Metro")]
		public static bool GetOnDataContextChanged(MetroContentControl element)
		{
			return (bool)element.GetValue(ReloadBehavior.OnDataContextChangedProperty);
		}

		[Category("MahApps.Metro")]
		public static bool GetOnSelectedTabChanged(ContentControl element)
		{
			return (bool)element.GetValue(ReloadBehavior.OnDataContextChangedProperty);
		}

		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MetroContentControl)d).DataContextChanged += new DependencyPropertyChangedEventHandler(ReloadBehavior.ReloadDataContextChanged);
		}

		private static void OnSelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentControl)d).Loaded += new RoutedEventHandler(ReloadBehavior.ReloadLoaded);
		}

		private static void ReloadDataContextChanged(MetroContentControl sender, DependencyPropertyChangedEventArgs e)
		{
			((MetroContentControl)sender).Reload();
		}

		private static void ReloadLoaded(ContentControl sender, RoutedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)sender;
			TabControl tabControl = ReloadBehavior.Ancestors(contentControl).OfType<TabControl>().FirstOrDefault<TabControl>();
			if (tabControl == null)
			{
				return;
			}
			ReloadBehavior.SetMetroContentControl(tabControl, contentControl);
			tabControl.SelectionChanged -= new SelectionChangedEventHandler(ReloadBehavior.ReloadSelectionChanged);
			tabControl.SelectionChanged += new SelectionChangedEventHandler(ReloadBehavior.ReloadSelectionChanged);
		}

		private static void ReloadSelectionChanged(TabControl sender, SelectionChangedEventArgs e)
		{
			if (e.OriginalSource != sender)
			{
				return;
			}
			ContentControl metroContentControl = ReloadBehavior.GetMetroContentControl((TabControl)sender);
			MetroContentControl metroContentControl1 = metroContentControl as MetroContentControl;
			if (metroContentControl1 != null)
			{
				metroContentControl1.Reload();
			}
			TransitioningContentControl transitioningContentControl = metroContentControl as TransitioningContentControl;
			if (transitioningContentControl != null)
			{
				transitioningContentControl.ReloadTransition();
			}
		}

		public static void SetMetroContentControl(UIElement element, ContentControl value)
		{
			element.SetValue(ReloadBehavior.MetroContentControlProperty, value);
		}

		public static void SetOnDataContextChanged(MetroContentControl element, bool value)
		{
			element.SetValue(ReloadBehavior.OnDataContextChangedProperty, value);
		}

		public static void SetOnSelectedTabChanged(ContentControl element, bool value)
		{
			element.SetValue(ReloadBehavior.OnDataContextChangedProperty, value);
		}
	}
}