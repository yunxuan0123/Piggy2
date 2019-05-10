using MahApps.Metro;
using MahApps.Metro.Native;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MahApps.Metro.Controls
{
	public class WindowSettings
	{
		public readonly static DependencyProperty WindowPlacementSettingsProperty;

		private Window _window;

		private IWindowPlacementSettings _settings;

		static WindowSettings()
		{
			Class6.yDnXvgqzyB5jw();
			WindowSettings.WindowPlacementSettingsProperty = DependencyProperty.RegisterAttached("WindowPlacementSettings", typeof(IWindowPlacementSettings), typeof(WindowSettings), new FrameworkPropertyMetadata(new PropertyChangedCallback(WindowSettings.OnWindowPlacementSettingsInvalidated)));
		}

		public WindowSettings(Window window, IWindowPlacementSettings windowPlacementSettings)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._window = window;
			this._settings = windowPlacementSettings;
		}

		private void Attach()
		{
			if (this._window != null)
			{
				this._window.SourceInitialized += new EventHandler(this.WindowSourceInitialized);
				this._window.Closed += new EventHandler(this.WindowClosed);
			}
		}

		protected virtual void LoadWindowState()
		{
			if (this._settings == null)
			{
				return;
			}
			this._settings.Reload();
			if (!this._settings.Placement.HasValue || this._settings.Placement.Value.normalPosition.IsEmpty)
			{
				return;
			}
			try
			{
				WINDOWPLACEMENT value = this._settings.Placement.Value;
				value.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
				value.flags = 0;
				value.showCmd = (value.showCmd == 2 ? 1 : value.showCmd);
				IntPtr handle = (new WindowInteropHelper(this._window)).Handle;
				UnsafeNativeMethods.SetWindowPlacement(handle, ref value);
			}
			catch (Exception exception)
			{
				throw new MahAppsException("Failed to set the window state from the settings file", exception);
			}
		}

		private static void OnWindowPlacementSettingsInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Window window = dependencyObject as Window;
			if (window == null || !(e.NewValue is IWindowPlacementSettings))
			{
				return;
			}
			(new WindowSettings(window, (IWindowPlacementSettings)e.NewValue)).Attach();
		}

		protected virtual void SaveWindowState()
		{
			RECT rECT;
			if (this._settings == null)
			{
				return;
			}
			IntPtr handle = (new WindowInteropHelper(this._window)).Handle;
			WINDOWPLACEMENT wINDOWPLACEMENT = new WINDOWPLACEMENT()
			{
				length = Marshal.SizeOf(wINDOWPLACEMENT)
			};
			UnsafeNativeMethods.GetWindowPlacement(handle, ref wINDOWPLACEMENT);
			if (wINDOWPLACEMENT.showCmd != 0 && wINDOWPLACEMENT.length > 0)
			{
				if (wINDOWPLACEMENT.showCmd == 1 && UnsafeNativeMethods.GetWindowRect(handle, out rECT))
				{
					wINDOWPLACEMENT.normalPosition = rECT;
				}
				if (!wINDOWPLACEMENT.normalPosition.IsEmpty)
				{
					this._settings.Placement = new WINDOWPLACEMENT?(wINDOWPLACEMENT);
				}
			}
			this._settings.Save();
		}

		public static void SetSave(DependencyObject dependencyObject, IWindowPlacementSettings windowPlacementSettings)
		{
			dependencyObject.SetValue(WindowSettings.WindowPlacementSettingsProperty, windowPlacementSettings);
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			this.SaveWindowState();
			this._window.StateChanged -= new EventHandler(this.WindowStateChanged);
			this._window.Closing -= new CancelEventHandler(this.WindowClosing);
			this._window.Closed -= new EventHandler(this.WindowClosed);
			this._window.SourceInitialized -= new EventHandler(this.WindowSourceInitialized);
			this._window = null;
			this._settings = null;
		}

		private void WindowClosing(object sender, CancelEventArgs e)
		{
			this.SaveWindowState();
		}

		private void WindowSourceInitialized(object sender, EventArgs e)
		{
			this.LoadWindowState();
			this._window.StateChanged += new EventHandler(this.WindowStateChanged);
			this._window.Closing += new CancelEventHandler(this.WindowClosing);
		}

		private void WindowStateChanged(object sender, EventArgs e)
		{
			if (this._window.WindowState == WindowState.Minimized)
			{
				this.SaveWindowState();
			}
		}
	}
}