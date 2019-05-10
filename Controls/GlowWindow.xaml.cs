using MahApps.Metro.Models.Win32;
using MahApps.Metro.Native;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public partial class GlowWindow : Window
	{
		private readonly Func<Point, System.Windows.Input.Cursor> getCursor;

		private readonly Func<Point, HitTestValues> getHitTestValue;

		private readonly Func<RECT, double> getLeft;

		private readonly Func<RECT, double> getTop;

		private readonly Func<RECT, double> getWidth;

		private readonly Func<RECT, double> getHeight;

		private IntPtr handle;

		private IntPtr ownerHandle;

		private bool closing;

		private HwndSource hwndSource;

		private PropertyChangeNotifier resizeModeChangeNotifier;

		public bool IsGlowing
		{
			get;
			set;
		}

		public Storyboard OpacityStoryboard
		{
			get;
			set;
		}

		public GlowWindow(Window owner, GlowDirection direction)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			GlowWindow glowWindow = this;
			this.InitializeComponent();
			this.IsGlowing = true;
			base.AllowsTransparency = true;
			base.Closing += new CancelEventHandler((object sender, CancelEventArgs e) => e.Cancel = !this.closing);
			base.Owner = owner;
			this.glow.Visibility = System.Windows.Visibility.Collapsed;
			Binding binding = new Binding("GlowBrush")
			{
				Source = owner
			};
			this.glow.SetBinding(Glow.GlowBrushProperty, binding);
			binding = new Binding("NonActiveGlowBrush")
			{
				Source = owner
			};
			this.glow.SetBinding(Glow.NonActiveGlowBrushProperty, binding);
			binding = new Binding("BorderThickness")
			{
				Source = owner
			};
			this.glow.SetBinding(Control.BorderThicknessProperty, binding);
			this.glow.Direction = direction;
			switch (direction)
			{
				case GlowDirection.Left:
				{
					this.glow.Orientation = Orientation.Vertical;
					this.glow.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
					this.getLeft = (RECT rect) => (double)rect.left - 6 + 1;
					this.getTop = (RECT rect) => (double)(rect.top - 2);
					this.getWidth = (RECT rect) => 6;
					this.getHeight = (RECT rect) => (double)(rect.Height + 4);
					this.getHitTestValue = (Point p) => {
						if ((new Rect(0, 0, base.ActualWidth, 20)).Contains(p))
						{
							return HitTestValues.HTTOPLEFT;
						}
						if (!(new Rect(0, base.ActualHeight - 20, base.ActualWidth, 20)).Contains(p))
						{
							return HitTestValues.HTLEFT;
						}
						return HitTestValues.HTBOTTOMLEFT;
					};
					this.getCursor = (Point p) => {
						if (owner.ResizeMode == System.Windows.ResizeMode.NoResize || owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
						{
							return owner.Cursor;
						}
						if ((new Rect(0, 0, glowWindow.ActualWidth, 20)).Contains(p))
						{
							return Cursors.SizeNWSE;
						}
						if (!(new Rect(0, glowWindow.ActualHeight - 20, glowWindow.ActualWidth, 20)).Contains(p))
						{
							return Cursors.SizeWE;
						}
						return Cursors.SizeNESW;
					};
					break;
				}
				case GlowDirection.Right:
				{
					this.glow.Orientation = Orientation.Vertical;
					this.glow.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
					this.getLeft = (RECT rect) => (double)(rect.right - 1);
					this.getTop = (RECT rect) => (double)(rect.top - 2);
					this.getWidth = (RECT rect) => 6;
					this.getHeight = (RECT rect) => (double)(rect.Height + 4);
					this.getHitTestValue = (Point p) => {
						if ((new Rect(0, 0, base.ActualWidth, 20)).Contains(p))
						{
							return HitTestValues.HTTOPRIGHT;
						}
						if (!(new Rect(0, base.ActualHeight - 20, base.ActualWidth, 20)).Contains(p))
						{
							return HitTestValues.HTRIGHT;
						}
						return HitTestValues.HTBOTTOMRIGHT;
					};
					this.getCursor = (Point p) => {
						if (owner.ResizeMode == System.Windows.ResizeMode.NoResize || owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
						{
							return owner.Cursor;
						}
						if ((new Rect(0, 0, glowWindow.ActualWidth, 20)).Contains(p))
						{
							return Cursors.SizeNESW;
						}
						if (!(new Rect(0, glowWindow.ActualHeight - 20, glowWindow.ActualWidth, 20)).Contains(p))
						{
							return Cursors.SizeWE;
						}
						return Cursors.SizeNWSE;
					};
					break;
				}
				case GlowDirection.Top:
				{
					this.glow.Orientation = Orientation.Horizontal;
					this.glow.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
					this.getLeft = (RECT rect) => (double)(rect.left - 2);
					this.getTop = (RECT rect) => (double)rect.top - 6 + 1;
					this.getWidth = (RECT rect) => (double)(rect.Width + 4);
					this.getHeight = (RECT rect) => 6;
					this.getHitTestValue = (Point p) => {
						if ((new Rect(0, 0, 14, base.ActualHeight)).Contains(p))
						{
							return HitTestValues.HTTOPLEFT;
						}
						if (!(new Rect(base.Width - 20 + 6, 0, 14, base.ActualHeight)).Contains(p))
						{
							return HitTestValues.HTTOP;
						}
						return HitTestValues.HTTOPRIGHT;
					};
					this.getCursor = (Point p) => {
						if (owner.ResizeMode == System.Windows.ResizeMode.NoResize || owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
						{
							return owner.Cursor;
						}
						if ((new Rect(0, 0, 14, glowWindow.ActualHeight)).Contains(p))
						{
							return Cursors.SizeNWSE;
						}
						if (!(new Rect(glowWindow.Width - 20 + 6, 0, 14, glowWindow.ActualHeight)).Contains(p))
						{
							return Cursors.SizeNS;
						}
						return Cursors.SizeNESW;
					};
					break;
				}
				case GlowDirection.Bottom:
				{
					this.glow.Orientation = Orientation.Horizontal;
					this.glow.VerticalAlignment = System.Windows.VerticalAlignment.Top;
					this.getLeft = (RECT rect) => (double)(rect.left - 2);
					this.getTop = (RECT rect) => (double)(rect.bottom - 1);
					this.getWidth = (RECT rect) => (double)(rect.Width + 4);
					this.getHeight = (RECT rect) => 6;
					this.getHitTestValue = (Point p) => {
						if ((new Rect(0, 0, 14, base.ActualHeight)).Contains(p))
						{
							return HitTestValues.HTBOTTOMLEFT;
						}
						if (!(new Rect(base.Width - 20 + 6, 0, 14, base.ActualHeight)).Contains(p))
						{
							return HitTestValues.HTBOTTOM;
						}
						return HitTestValues.HTBOTTOMRIGHT;
					};
					this.getCursor = (Point p) => {
						if (owner.ResizeMode == System.Windows.ResizeMode.NoResize || owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
						{
							return owner.Cursor;
						}
						if ((new Rect(0, 0, 14, glowWindow.ActualHeight)).Contains(p))
						{
							return Cursors.SizeNESW;
						}
						if (!(new Rect(glowWindow.Width - 20 + 6, 0, 14, glowWindow.ActualHeight)).Contains(p))
						{
							return Cursors.SizeNS;
						}
						return Cursors.SizeNWSE;
					};
					break;
				}
			}
			owner.ContentRendered += new EventHandler((object sender, EventArgs e) => this.glow.Visibility = System.Windows.Visibility.Visible);
			owner.Activated += new EventHandler((object sender, EventArgs e) => {
				this.Update();
				this.glow.IsGlow = true;
			});
			owner.Deactivated += new EventHandler((object sender, EventArgs e) => this.glow.IsGlow = false);
			owner.StateChanged += new EventHandler((object sender, EventArgs e) => this.Update());
			owner.IsVisibleChanged += new DependencyPropertyChangedEventHandler((object sender, DependencyPropertyChangedEventArgs e) => this.Update());
			owner.Closed += new EventHandler((object sender, EventArgs e) => {
				this.closing = true;
				base.Close();
			});
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.OpacityStoryboard = base.TryFindResource("OpacityStoryboard") as Storyboard;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			this.hwndSource = (HwndSource)PresentationSource.FromVisual(this);
			if (this.hwndSource == null)
			{
				return;
			}
			WS windowLong = this.hwndSource.Handle.GetWindowLong();
			WSEX windowLongEx = this.hwndSource.Handle.GetWindowLongEx();
			windowLongEx ^= WSEX.APPWINDOW;
			windowLongEx |= WSEX.NOACTIVATE;
			if (base.Owner.ResizeMode == System.Windows.ResizeMode.NoResize || base.Owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
			{
				windowLongEx |= WSEX.TRANSPARENT;
			}
			this.hwndSource.Handle.SetWindowLong(windowLong);
			this.hwndSource.Handle.SetWindowLongEx(windowLongEx);
			this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
			this.handle = this.hwndSource.Handle;
			this.ownerHandle = (new WindowInteropHelper(base.Owner)).Handle;
			this.resizeModeChangeNotifier = new PropertyChangeNotifier(base.Owner, Window.ResizeModeProperty);
			this.resizeModeChangeNotifier.ValueChanged += new EventHandler(this.ResizeModeChanged);
		}

		private void ResizeModeChanged(object sender, EventArgs e)
		{
			WSEX wSEX;
			WSEX windowLongEx = this.hwndSource.Handle.GetWindowLongEx();
			if (base.Owner.ResizeMode != System.Windows.ResizeMode.NoResize)
			{
				if (base.Owner.ResizeMode == System.Windows.ResizeMode.CanMinimize)
				{
					windowLongEx |= WSEX.TRANSPARENT;
					wSEX = this.hwndSource.Handle.SetWindowLongEx(windowLongEx);
					return;
				}
				windowLongEx ^= WSEX.TRANSPARENT;
				wSEX = this.hwndSource.Handle.SetWindowLongEx(windowLongEx);
				return;
			}
			windowLongEx |= WSEX.TRANSPARENT;
			wSEX = this.hwndSource.Handle.SetWindowLongEx(windowLongEx);
		}

		public void Update()
		{
			RECT rECT;
			if (base.Owner.Visibility == System.Windows.Visibility.Hidden)
			{
				base.Visibility = System.Windows.Visibility.Hidden;
				if (this.ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out rECT))
				{
					this.UpdateCore(rECT);
					return;
				}
			}
			else if (base.Owner.WindowState != System.Windows.WindowState.Normal)
			{
				base.Visibility = System.Windows.Visibility.Collapsed;
			}
			else
			{
				if (this.closing)
				{
					return;
				}
				base.Visibility = (this.IsGlowing ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
				this.glow.Visibility = (this.IsGlowing ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
				if (this.ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out rECT))
				{
					this.UpdateCore(rECT);
					return;
				}
			}
		}

		internal void UpdateCore(RECT rect)
		{
			NativeMethods.SetWindowPos(this.handle, this.ownerHandle, (int)this.getLeft(rect), (int)this.getTop(rect), (int)this.getWidth(rect), (int)this.getHeight(rect), SWP.NOZORDER | SWP.NOACTIVATE);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 24 && (int)lParam == 3 && base.Visibility != System.Windows.Visibility.Visible)
			{
				handled = true;
			}
			if (msg == 33)
			{
				handled = true;
				return new IntPtr(3);
			}
			if (msg == 513)
			{
				Point point = new Point((double)((int)lParam & 65535), (double)((int)lParam >> 16 & 65535));
				NativeMethods.PostMessage(this.ownerHandle, 161, (IntPtr)this.getHitTestValue(point), IntPtr.Zero);
			}
			if (msg == 132)
			{
				Point point1 = new Point((double)((int)lParam & 65535), (double)((int)lParam >> 16 & 65535));
				Point point2 = base.PointFromScreen(point1);
				System.Windows.Input.Cursor cursor = this.getCursor(point2);
				if (cursor != base.Cursor)
				{
					base.Cursor = cursor;
				}
			}
			return IntPtr.Zero;
		}
	}
}