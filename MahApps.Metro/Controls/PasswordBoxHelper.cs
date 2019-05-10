using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class PasswordBoxHelper
	{
		public readonly static DependencyProperty CapsLockIconProperty;

		public readonly static DependencyProperty CapsLockWarningToolTipProperty;

		static PasswordBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			PasswordBoxHelper.CapsLockIconProperty = DependencyProperty.RegisterAttached("CapsLockIcon", typeof(object), typeof(PasswordBoxHelper), new PropertyMetadata("!", new PropertyChangedCallback(PasswordBoxHelper.ShowCapslockWarningChanged)));
			PasswordBoxHelper.CapsLockWarningToolTipProperty = DependencyProperty.RegisterAttached("CapsLockWarningToolTip", typeof(object), typeof(PasswordBoxHelper), new PropertyMetadata("Caps lock is on"));
		}

		public PasswordBoxHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private static FrameworkElement FindCapsLockIndicator(Control pb)
		{
			return pb.Template.FindName("PART_CapsLockIndicator", pb) as FrameworkElement;
		}

		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[Category("MahApps.Metro")]
		public static object GetCapsLockIcon(PasswordBox element)
		{
			return element.GetValue(PasswordBoxHelper.CapsLockIconProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[Category("MahApps.Metro")]
		public static object GetCapsLockWarningToolTip(PasswordBox element)
		{
			return element.GetValue(PasswordBoxHelper.CapsLockWarningToolTipProperty);
		}

		private static void HandlePasswordBoxLostFocus(Control sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = PasswordBoxHelper.FindCapsLockIndicator((Control)sender);
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = Visibility.Collapsed;
			}
		}

		private static void RefreshCapslockStatus(Control sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = PasswordBoxHelper.FindCapsLockIndicator((Control)sender);
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = (Keyboard.IsKeyToggled(Key.Capital) ? Visibility.Visible : Visibility.Collapsed);
			}
		}

		public static void SetCapsLockIcon(PasswordBox element, object value)
		{
			element.SetValue(PasswordBoxHelper.CapsLockIconProperty, value);
		}

		public static void SetCapsLockWarningToolTip(PasswordBox element, object value)
		{
			element.SetValue(PasswordBoxHelper.CapsLockWarningToolTipProperty, value);
		}

		private static void ShowCapslockWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			passwordBox.KeyDown -= new KeyEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
			passwordBox.GotFocus -= new RoutedEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
			passwordBox.LostFocus -= new RoutedEventHandler(PasswordBoxHelper.HandlePasswordBoxLostFocus);
			if (e.NewValue != null)
			{
				passwordBox.KeyDown += new KeyEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
				passwordBox.GotFocus += new RoutedEventHandler(PasswordBoxHelper.RefreshCapslockStatus);
				passwordBox.LostFocus += new RoutedEventHandler(PasswordBoxHelper.HandlePasswordBoxLostFocus);
			}
		}
	}
}