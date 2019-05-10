using MahApps.Metro.Controls;
using MahApps.Metro.Native;
using Microsoft.Windows.Shell;
using Standard;
using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MahApps.Metro.Behaviours
{
	public class BorderlessWindowBehavior : Behavior<Window>
	{
		private IntPtr handle;

		private HwndSource hwndSource;

		private WindowChrome windowChrome;

		private PropertyChangeNotifier borderThicknessChangeNotifier;

		private Thickness? savedBorderThickness;

		private PropertyChangeNotifier topMostChangeNotifier;

		private bool savedTopMost;

		private bool isCleanedUp;

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" properties in your Window to get a drop shadow around it.")]
		public readonly static DependencyProperty EnableDWMDropShadowProperty;

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" properties in your Window to get a drop shadow around it.")]
		public bool EnableDWMDropShadow
		{
			get
			{
				return (bool)base.GetValue(BorderlessWindowBehavior.EnableDWMDropShadowProperty);
			}
			set
			{
				base.SetValue(BorderlessWindowBehavior.EnableDWMDropShadowProperty, value);
			}
		}

		static BorderlessWindowBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			BorderlessWindowBehavior.EnableDWMDropShadowProperty = DependencyProperty.Register("EnableDWMDropShadow", typeof(bool), typeof(BorderlessWindowBehavior), new PropertyMetadata(false));
		}

		public BorderlessWindowBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		[SecurityCritical]
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			if (this.handle == IntPtr.Zero)
			{
				return false;
			}
			IntPtr windowLongPtr = NativeMethods.GetWindowLongPtr(this.handle, GWL.STYLE);
			WS w = (WS)((uint)((Environment.Is64BitProcess ? windowLongPtr.ToInt64() : (long)windowLongPtr.ToInt32())));
			WS w1 = w & ~removeStyle | addStyle;
			if (w == w1)
			{
				return false;
			}
			NativeMethods.SetWindowLongPtr(this.handle, GWL.STYLE, new IntPtr((int)w1));
			return true;
		}

		private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
		{
			MetroWindow metroWindow = sender as MetroWindow;
			if (metroWindow == null)
			{
				return;
			}
			metroWindow.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_Icon");
			metroWindow.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_TitleBar");
			metroWindow.SetIsHitTestVisibleInChromeProperty<Thumb>("PART_WindowTitleThumb");
			metroWindow.SetIsHitTestVisibleInChromeProperty<ContentPresenter>("PART_LeftWindowCommands");
			metroWindow.SetIsHitTestVisibleInChromeProperty<ContentPresenter>("PART_RightWindowCommands");
			metroWindow.SetIsHitTestVisibleInChromeProperty<ContentControl>("PART_WindowButtonCommands");
			metroWindow.SetWindowChromeResizeGripDirection("WindowResizeGrip", ResizeGripDirection.BottomRight);
		}

		private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
		{
			this.handle = (new WindowInteropHelper(base.AssociatedObject)).Handle;
			this.hwndSource = HwndSource.FromHwnd(this.handle);
			if (this.hwndSource != null)
			{
				this.hwndSource.AddHook(new HwndSourceHook(this.WindowProc));
			}
			if (base.AssociatedObject.ResizeMode != ResizeMode.NoResize)
			{
				SizeToContent sizeToContent = base.AssociatedObject.SizeToContent;
				bool snapsToDevicePixels = base.AssociatedObject.SnapsToDevicePixels;
				base.AssociatedObject.SnapsToDevicePixels = true;
				base.AssociatedObject.SizeToContent = (sizeToContent == SizeToContent.WidthAndHeight ? SizeToContent.Height : SizeToContent.Manual);
				base.AssociatedObject.SizeToContent = sizeToContent;
				base.AssociatedObject.SnapsToDevicePixels = snapsToDevicePixels;
			}
		}

		private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
		{
			this.Cleanup();
		}

		private void BorderThicknessChangeNotifierOnValueChanged(object sender, EventArgs e)
		{
			this.savedBorderThickness = new Thickness?(base.AssociatedObject.BorderThickness);
		}

		private void Cleanup()
		{
			if (!this.isCleanedUp)
			{
				this.isCleanedUp = true;
				if (base.AssociatedObject is MetroWindow)
				{
					DependencyPropertyDescriptor.FromProperty(MetroWindow.IgnoreTaskbarOnMaximizeProperty, typeof(MetroWindow)).RemoveValueChanged(base.AssociatedObject, new EventHandler(this.IgnoreTaskbarOnMaximizePropertyChangedCallback));
					DependencyPropertyDescriptor.FromProperty(MetroWindow.UseNoneWindowStyleProperty, typeof(MetroWindow)).RemoveValueChanged(base.AssociatedObject, new EventHandler(this.UseNoneWindowStylePropertyChangedCallback));
				}
				base.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObject_Loaded);
				base.AssociatedObject.Unloaded -= new RoutedEventHandler(this.AssociatedObject_Unloaded);
				base.AssociatedObject.SourceInitialized -= new EventHandler(this.AssociatedObject_SourceInitialized);
				base.AssociatedObject.StateChanged -= new EventHandler(this.OnAssociatedObjectHandleMaximize);
				if (this.hwndSource != null)
				{
					this.hwndSource.RemoveHook(new HwndSourceHook(this.WindowProc));
				}
				this.windowChrome = null;
			}
		}

		private void ForceRedrawWindowFromPropertyChanged()
		{
			this.HandleMaximize();
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.RedrawWindow(this.handle, IntPtr.Zero, IntPtr.Zero, Constants.RedrawWindowFlags.Invalidate | Constants.RedrawWindowFlags.Frame);
			}
		}

		private void HandleMaximize()
		{
			bool flag;
			bool flag1;
			this.borderThicknessChangeNotifier.ValueChanged -= new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
			this.topMostChangeNotifier.ValueChanged -= new EventHandler(this.TopMostChangeNotifierOnValueChanged);
			MetroWindow associatedObject = base.AssociatedObject as MetroWindow;
			bool enableDWMDropShadow = this.EnableDWMDropShadow;
			if (associatedObject != null)
			{
				if (associatedObject.GlowBrush != null)
				{
					flag1 = false;
				}
				else
				{
					flag1 = (associatedObject.EnableDWMDropShadow ? true : this.EnableDWMDropShadow);
				}
				enableDWMDropShadow = flag1;
			}
			if (base.AssociatedObject.WindowState != WindowState.Maximized)
			{
				this.windowChrome.ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
				if (!enableDWMDropShadow)
				{
					base.AssociatedObject.BorderThickness = this.savedBorderThickness.GetValueOrDefault(new Thickness(0));
				}
			}
			else
			{
				this.windowChrome.ResizeBorderThickness = new Thickness(0);
				base.AssociatedObject.BorderThickness = new Thickness(0);
				flag = (associatedObject == null ? false : associatedObject.IgnoreTaskbarOnMaximize);
				bool flag2 = flag;
				if (flag && this.handle != IntPtr.Zero)
				{
					IntPtr intPtr = UnsafeNativeMethods.MonitorFromWindow(this.handle, 2);
					if (intPtr != IntPtr.Zero)
					{
						MahApps.Metro.Native.MONITORINFO mONITORINFO = new MahApps.Metro.Native.MONITORINFO();
						UnsafeNativeMethods.GetMonitorInfoW(intPtr, mONITORINFO);
						int num = (flag2 ? mONITORINFO.rcMonitor.left : mONITORINFO.rcWork.left);
						int num1 = (flag2 ? mONITORINFO.rcMonitor.top : mONITORINFO.rcWork.top);
						int num2 = (flag2 ? Math.Abs(mONITORINFO.rcMonitor.right - num) : Math.Abs(mONITORINFO.rcWork.right - num));
						int num3 = (flag2 ? Math.Abs(mONITORINFO.rcMonitor.bottom - num1) : Math.Abs(mONITORINFO.rcWork.bottom - num1));
						UnsafeNativeMethods.SetWindowPos(this.handle, new IntPtr(-2), num, num1, num2, num3, 64);
					}
				}
			}
			base.AssociatedObject.Topmost = false;
			base.AssociatedObject.Topmost = (base.AssociatedObject.WindowState == WindowState.Minimized ? true : this.savedTopMost);
			this.borderThicknessChangeNotifier.ValueChanged += new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
			this.topMostChangeNotifier.ValueChanged += new EventHandler(this.TopMostChangeNotifierOnValueChanged);
		}

		private void IgnoreTaskbarOnMaximizePropertyChangedCallback(object sender, EventArgs e)
		{
			MetroWindow metroWindow = sender as MetroWindow;
			if (metroWindow != null && this.windowChrome != null && !object.Equals(this.windowChrome.IgnoreTaskbarOnMaximize, metroWindow.IgnoreTaskbarOnMaximize))
			{
				bool flag = this._ModifyStyle(WS.THICKFRAME | WS.GROUP | WS.TABSTOP | WS.MINIMIZEBOX | WS.MAXIMIZEBOX | WS.SIZEBOX, WS.OVERLAPPED);
				this.windowChrome.IgnoreTaskbarOnMaximize = metroWindow.IgnoreTaskbarOnMaximize;
				this.ForceRedrawWindowFromPropertyChanged();
				if (flag)
				{
					this._ModifyStyle(WS.OVERLAPPED, WS.THICKFRAME | WS.GROUP | WS.TABSTOP | WS.MINIMIZEBOX | WS.MAXIMIZEBOX | WS.SIZEBOX);
				}
			}
		}

		private void OnAssociatedObjectHandleMaximize(object sender, EventArgs e)
		{
			this.HandleMaximize();
		}

		protected override void OnAttached()
		{
			this.windowChrome = new WindowChrome()
			{
				ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness,
				CaptionHeight = 0,
				CornerRadius = new CornerRadius(0),
				GlassFrameThickness = new Thickness(0),
				UseAeroCaptionButtons = false
			};
			MetroWindow associatedObject = base.AssociatedObject as MetroWindow;
			if (associatedObject != null)
			{
				this.windowChrome.IgnoreTaskbarOnMaximize = associatedObject.IgnoreTaskbarOnMaximize;
				this.windowChrome.UseNoneWindowStyle = associatedObject.UseNoneWindowStyle;
				DependencyPropertyDescriptor.FromProperty(MetroWindow.IgnoreTaskbarOnMaximizeProperty, typeof(MetroWindow)).AddValueChanged(base.AssociatedObject, new EventHandler(this.IgnoreTaskbarOnMaximizePropertyChangedCallback));
				DependencyPropertyDescriptor.FromProperty(MetroWindow.UseNoneWindowStyleProperty, typeof(MetroWindow)).AddValueChanged(base.AssociatedObject, new EventHandler(this.UseNoneWindowStylePropertyChangedCallback));
			}
			base.AssociatedObject.SetValue(WindowChrome.WindowChromeProperty, this.windowChrome);
			IntPtr handle = (new WindowInteropHelper(base.AssociatedObject)).Handle;
			if (!base.AssociatedObject.IsLoaded && handle == IntPtr.Zero)
			{
				try
				{
					base.AssociatedObject.AllowsTransparency = false;
				}
				catch (Exception exception)
				{
				}
			}
			base.AssociatedObject.WindowStyle = WindowStyle.None;
			this.savedBorderThickness = new Thickness?(base.AssociatedObject.BorderThickness);
			this.borderThicknessChangeNotifier = new PropertyChangeNotifier(base.AssociatedObject, Control.BorderThicknessProperty);
			this.borderThicknessChangeNotifier.ValueChanged += new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
			this.savedTopMost = base.AssociatedObject.Topmost;
			this.topMostChangeNotifier = new PropertyChangeNotifier(base.AssociatedObject, Window.TopmostProperty);
			this.topMostChangeNotifier.ValueChanged += new EventHandler(this.TopMostChangeNotifierOnValueChanged);
			base.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObject_Loaded);
			base.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObject_Unloaded);
			base.AssociatedObject.SourceInitialized += new EventHandler(this.AssociatedObject_SourceInitialized);
			base.AssociatedObject.StateChanged += new EventHandler(this.OnAssociatedObjectHandleMaximize);
			this.HandleMaximize();
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			this.Cleanup();
			base.OnDetaching();
		}

		private void TopMostChangeNotifierOnValueChanged(object sender, EventArgs e)
		{
			this.savedTopMost = base.AssociatedObject.Topmost;
		}

		private void UseNoneWindowStylePropertyChangedCallback(object sender, EventArgs e)
		{
			MetroWindow metroWindow = sender as MetroWindow;
			if (metroWindow != null && this.windowChrome != null && !object.Equals(this.windowChrome.UseNoneWindowStyle, metroWindow.UseNoneWindowStyle))
			{
				this.windowChrome.UseNoneWindowStyle = metroWindow.UseNoneWindowStyle;
				this.ForceRedrawWindowFromPropertyChanged();
			}
		}

		private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg == 133)
			{
				handled = true;
			}
			else if (msg == 134)
			{
				zero = UnsafeNativeMethods.DefWindowProc(hwnd, msg, wParam, new IntPtr(-1));
				handled = true;
			}
			return zero;
		}
	}
}