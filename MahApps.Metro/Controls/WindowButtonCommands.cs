using MahApps.Metro.Native;
using Microsoft.Windows.Shell;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_Close", Type=typeof(Button))]
	[TemplatePart(Name="PART_Max", Type=typeof(Button))]
	[TemplatePart(Name="PART_Min", Type=typeof(Button))]
	public class WindowButtonCommands : ContentControl, INotifyPropertyChanged
	{
		public readonly static DependencyProperty LightMinButtonStyleProperty;

		public readonly static DependencyProperty LightMaxButtonStyleProperty;

		public readonly static DependencyProperty LightCloseButtonStyleProperty;

		public readonly static DependencyProperty DarkMinButtonStyleProperty;

		public readonly static DependencyProperty DarkMaxButtonStyleProperty;

		public readonly static DependencyProperty DarkCloseButtonStyleProperty;

		public readonly static DependencyProperty ThemeProperty;

		private static string minimize;

		private static string maximize;

		private static string closeText;

		private static string restore;

		private Button min;

		private Button max;

		private Button close;

		private SafeLibraryHandle user32;

		private MetroWindow _parentWindow;

		public string Close
		{
			get
			{
				if (string.IsNullOrEmpty(WindowButtonCommands.closeText))
				{
					WindowButtonCommands.closeText = this.GetCaption(905);
				}
				return WindowButtonCommands.closeText;
			}
		}

		public System.Windows.Style DarkCloseButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.DarkCloseButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.DarkCloseButtonStyleProperty, value);
			}
		}

		public System.Windows.Style DarkMaxButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.DarkMaxButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.DarkMaxButtonStyleProperty, value);
			}
		}

		public System.Windows.Style DarkMinButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.DarkMinButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.DarkMinButtonStyleProperty, value);
			}
		}

		public System.Windows.Style LightCloseButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.LightCloseButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.LightCloseButtonStyleProperty, value);
			}
		}

		public System.Windows.Style LightMaxButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.LightMaxButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.LightMaxButtonStyleProperty, value);
			}
		}

		public System.Windows.Style LightMinButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(WindowButtonCommands.LightMinButtonStyleProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.LightMinButtonStyleProperty, value);
			}
		}

		public string Maximize
		{
			get
			{
				if (string.IsNullOrEmpty(WindowButtonCommands.maximize))
				{
					WindowButtonCommands.maximize = this.GetCaption(901);
				}
				return WindowButtonCommands.maximize;
			}
		}

		public string Minimize
		{
			get
			{
				if (string.IsNullOrEmpty(WindowButtonCommands.minimize))
				{
					WindowButtonCommands.minimize = this.GetCaption(900);
				}
				return WindowButtonCommands.minimize;
			}
		}

		public MetroWindow ParentWindow
		{
			get
			{
				return this._parentWindow;
			}
			set
			{
				if (object.Equals(this._parentWindow, value))
				{
					return;
				}
				this._parentWindow = value;
				this.RaisePropertyChanged("ParentWindow");
			}
		}

		public string Restore
		{
			get
			{
				if (string.IsNullOrEmpty(WindowButtonCommands.restore))
				{
					WindowButtonCommands.restore = this.GetCaption(903);
				}
				return WindowButtonCommands.restore;
			}
		}

		public MahApps.Metro.Controls.Theme Theme
		{
			get
			{
				return (MahApps.Metro.Controls.Theme)base.GetValue(WindowButtonCommands.ThemeProperty);
			}
			set
			{
				base.SetValue(WindowButtonCommands.ThemeProperty, value);
			}
		}

		static WindowButtonCommands()
		{
			Class6.yDnXvgqzyB5jw();
			WindowButtonCommands.LightMinButtonStyleProperty = DependencyProperty.Register("LightMinButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.LightMaxButtonStyleProperty = DependencyProperty.Register("LightMaxButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.LightCloseButtonStyleProperty = DependencyProperty.Register("LightCloseButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.DarkMinButtonStyleProperty = DependencyProperty.Register("DarkMinButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.DarkMaxButtonStyleProperty = DependencyProperty.Register("DarkMaxButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.DarkCloseButtonStyleProperty = DependencyProperty.Register("DarkCloseButtonStyle", typeof(System.Windows.Style), typeof(WindowButtonCommands), new PropertyMetadata(null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			WindowButtonCommands.ThemeProperty = DependencyProperty.Register("Theme", typeof(MahApps.Metro.Controls.Theme), typeof(WindowButtonCommands), new PropertyMetadata((object)MahApps.Metro.Controls.Theme.Light, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButtonCommands), new FrameworkPropertyMetadata(typeof(WindowButtonCommands)));
		}

		public WindowButtonCommands()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Loaded += new RoutedEventHandler(this.WindowButtonCommands_Loaded);
		}

		public void ApplyTheme()
		{
			if (this.close != null)
			{
				if (this.ParentWindow == null || this.ParentWindow.WindowCloseButtonStyle == null)
				{
					this.close.Style = (this.Theme == MahApps.Metro.Controls.Theme.Light ? this.LightCloseButtonStyle : this.DarkCloseButtonStyle);
				}
				else
				{
					this.close.Style = this.ParentWindow.WindowCloseButtonStyle;
				}
			}
			if (this.max != null)
			{
				if (this.ParentWindow == null || this.ParentWindow.WindowMaxButtonStyle == null)
				{
					this.max.Style = (this.Theme == MahApps.Metro.Controls.Theme.Light ? this.LightMaxButtonStyle : this.DarkMaxButtonStyle);
				}
				else
				{
					this.max.Style = this.ParentWindow.WindowMaxButtonStyle;
				}
			}
			if (this.min != null)
			{
				if (this.ParentWindow != null && this.ParentWindow.WindowMinButtonStyle != null)
				{
					this.min.Style = this.ParentWindow.WindowMinButtonStyle;
					return;
				}
				this.min.Style = (this.Theme == MahApps.Metro.Controls.Theme.Light ? this.LightMinButtonStyle : this.DarkMinButtonStyle);
			}
		}

		private void CloseClick(object sender, RoutedEventArgs e)
		{
			ClosingWindowEventHandlerArgs closingWindowEventHandlerArg = new ClosingWindowEventHandlerArgs();
			this.OnClosingWindow(closingWindowEventHandlerArg);
			if (closingWindowEventHandlerArg.Cancelled)
			{
				return;
			}
			if (this.ParentWindow == null)
			{
				return;
			}
			this.ParentWindow.Close();
		}

		private string GetCaption(int id)
		{
			if (this.user32 == null)
			{
				this.user32 = UnsafeNativeMethods.LoadLibraryW(string.Concat(Environment.SystemDirectory, "\\User32.dll"));
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			UnsafeNativeMethods.LoadStringW(this.user32, (uint)id, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString().Replace("&", "");
		}

		private void MaximizeClick(object sender, RoutedEventArgs e)
		{
			if (this.ParentWindow == null)
			{
				return;
			}
			if (this.ParentWindow.WindowState == WindowState.Maximized)
			{
				Microsoft.Windows.Shell.SystemCommands.RestoreWindow(this.ParentWindow);
				return;
			}
			Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(this.ParentWindow);
		}

		private void MinimizeClick(object sender, RoutedEventArgs e)
		{
			if (this.ParentWindow == null)
			{
				return;
			}
			Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this.ParentWindow);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.close = base.Template.FindName("PART_Close", this) as Button;
			if (this.close != null)
			{
				this.close.Click += new RoutedEventHandler(this.CloseClick);
			}
			this.max = base.Template.FindName("PART_Max", this) as Button;
			if (this.max != null)
			{
				this.max.Click += new RoutedEventHandler(this.MaximizeClick);
			}
			this.min = base.Template.FindName("PART_Min", this) as Button;
			if (this.min != null)
			{
				this.min.Click += new RoutedEventHandler(this.MinimizeClick);
			}
			this.ApplyTheme();
		}

		protected void OnClosingWindow(ClosingWindowEventHandlerArgs args)
		{
			WindowButtonCommands.ClosingWindowEventHandler closingWindowEventHandler = this.ClosingWindow;
			if (closingWindowEventHandler != null)
			{
				closingWindowEventHandler(this, args);
			}
		}

		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
			{
				return;
			}
			((WindowButtonCommands)d).ApplyTheme();
		}

		protected virtual void RaisePropertyChanged(string propertyName = null)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void WindowButtonCommands_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.WindowButtonCommands_Loaded);
			if (this.ParentWindow == null)
			{
				this.ParentWindow = this.TryFindParent<MetroWindow>();
			}
		}

		public event WindowButtonCommands.ClosingWindowEventHandler ClosingWindow;

		public event PropertyChangedEventHandler PropertyChanged;

		public delegate void ClosingWindowEventHandler(object sender, ClosingWindowEventHandlerArgs args);
	}
}