using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MahApps.Metro.Behaviours
{
	public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
	{
		private static bool IsUpdating;

		public readonly static DependencyProperty PasswordProperty;

		static PasswordBoxBindingBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			PasswordBoxBindingBehavior.PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBindingBehavior), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(PasswordBoxBindingBehavior.OnPasswordPropertyChanged)));
		}

		public PasswordBoxBindingBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		[Category("MahApps.Metro")]
		public static string GetPassword(DependencyObject dpo)
		{
			return (string)dpo.GetValue(PasswordBoxBindingBehavior.PasswordProperty);
		}

		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox newValue = sender as PasswordBox;
			if (newValue != null)
			{
				newValue.PasswordChanged -= new RoutedEventHandler(PasswordBoxBindingBehavior.PasswordBox_PasswordChanged);
				if (!PasswordBoxBindingBehavior.IsUpdating)
				{
					newValue.Password = (string)e.NewValue;
				}
				newValue.PasswordChanged += new RoutedEventHandler(PasswordBoxBindingBehavior.PasswordBox_PasswordChanged);
			}
		}

		private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			PasswordBoxBindingBehavior.IsUpdating = true;
			PasswordBoxBindingBehavior.SetPassword(passwordBox, passwordBox.Password);
			PasswordBoxBindingBehavior.IsUpdating = false;
		}

		public static void SetPassword(DependencyObject dpo, string value)
		{
			dpo.SetValue(PasswordBoxBindingBehavior.PasswordProperty, value);
		}
	}
}