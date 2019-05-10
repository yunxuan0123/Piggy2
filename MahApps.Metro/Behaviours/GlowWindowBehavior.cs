using MahApps.Metro.Controls;
using MahApps.Metro.Models.Win32;
using MahApps.Metro.Native;
using Standard;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Threading;

namespace MahApps.Metro.Behaviours
{
	public class GlowWindowBehavior : Behavior<Window>
	{
		private readonly static TimeSpan GlowTimerDelay;

		private GlowWindow left;

		private GlowWindow right;

		private GlowWindow top;

		private GlowWindow bottom;

		private DispatcherTimer makeGlowVisibleTimer;

		private IntPtr handle;

		private WINDOWPOS _previousWP;

		private bool IsGlowDisabled
		{
			get
			{
				MetroWindow associatedObject = base.AssociatedObject as MetroWindow;
				if (associatedObject == null)
				{
					return false;
				}
				if (associatedObject.UseNoneWindowStyle)
				{
					return true;
				}
				return associatedObject.GlowBrush == null;
			}
		}

		private bool IsWindowTransitionsEnabled
		{
			get
			{
				MetroWindow associatedObject = base.AssociatedObject as MetroWindow;
				if (associatedObject == null)
				{
					return false;
				}
				return associatedObject.WindowTransitionsEnabled;
			}
		}

		static GlowWindowBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			GlowWindowBehavior.GlowTimerDelay = TimeSpan.FromMilliseconds(200);
		}

		public GlowWindowBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private void AssociatedObjectIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (base.AssociatedObject.IsVisible)
			{
				this.StartOpacityStoryboard();
				return;
			}
			this.SetOpacityTo(0);
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs e)
		{
			if (this.IsGlowDisabled)
			{
				return;
			}
			base.AssociatedObject.StateChanged -= new EventHandler(this.AssociatedObjectStateChanged);
			base.AssociatedObject.StateChanged += new EventHandler(this.AssociatedObjectStateChanged);
			if (this.makeGlowVisibleTimer == null)
			{
				this.makeGlowVisibleTimer = new DispatcherTimer()
				{
					Interval = GlowWindowBehavior.GlowTimerDelay
				};
				this.makeGlowVisibleTimer.Tick += new EventHandler(this.makeGlowVisibleTimer_Tick);
			}
			this.left = new GlowWindow(base.AssociatedObject, GlowDirection.Left);
			this.right = new GlowWindow(base.AssociatedObject, GlowDirection.Right);
			this.top = new GlowWindow(base.AssociatedObject, GlowDirection.Top);
			this.bottom = new GlowWindow(base.AssociatedObject, GlowDirection.Bottom);
			this.Show();
			this.Update();
			if (!this.IsWindowTransitionsEnabled)
			{
				base.AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => this.SetOpacityTo(1)));
				return;
			}
			this.StartOpacityStoryboard();
			base.AssociatedObject.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.AssociatedObjectIsVisibleChanged);
			base.AssociatedObject.Closing += new CancelEventHandler((object argument0, CancelEventArgs argument1) => {
				if (!argument1.Cancel)
				{
					base.AssociatedObject.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.AssociatedObjectIsVisibleChanged);
				}
			});
		}

		private void AssociatedObjectStateChanged(object sender, EventArgs e)
		{
			if (this.makeGlowVisibleTimer != null)
			{
				this.makeGlowVisibleTimer.Stop();
			}
			if (base.AssociatedObject.WindowState != WindowState.Normal)
			{
				this.HideGlow();
				return;
			}
			MetroWindow associatedObject = base.AssociatedObject as MetroWindow;
			bool flag = (associatedObject == null ? false : associatedObject.IgnoreTaskbarOnMaximize);
			if (this.makeGlowVisibleTimer == null || !SystemParameters.MinimizeAnimation || flag)
			{
				this.RestoreGlow();
				return;
			}
			this.makeGlowVisibleTimer.Start();
		}

		private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
		{
			if (this.makeGlowVisibleTimer != null)
			{
				this.makeGlowVisibleTimer.Stop();
				this.makeGlowVisibleTimer.Tick -= new EventHandler(this.makeGlowVisibleTimer_Tick);
				this.makeGlowVisibleTimer = null;
			}
		}

		private IntPtr AssociatedObjectWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			WINDOWPOS structure;
			MahApps.Metro.Models.Win32.WM wM = (MahApps.Metro.Models.Win32.WM)msg;
			if (wM > MahApps.Metro.Models.Win32.WM.WINDOWPOSCHANGING)
			{
				if (wM == MahApps.Metro.Models.Win32.WM.WINDOWPOSCHANGED)
				{
					structure = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
					if (!structure.Equals(this._previousWP))
					{
						this.UpdateCore();
					}
					this._previousWP = structure;
					return IntPtr.Zero;
				}
				if (wM == MahApps.Metro.Models.Win32.WM.SIZING)
				{
					this.UpdateCore();
					return IntPtr.Zero;
				}
				return IntPtr.Zero;
			}
			else
			{
				if (wM == MahApps.Metro.Models.Win32.WM.SIZE)
				{
					this.UpdateCore();
					return IntPtr.Zero;
				}
				if (wM == MahApps.Metro.Models.Win32.WM.WINDOWPOSCHANGING)
				{
					structure = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
					if (!structure.Equals(this._previousWP))
					{
						this.UpdateCore();
					}
					this._previousWP = structure;
					return IntPtr.Zero;
				}
				return IntPtr.Zero;
			}
			structure = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
			if (!structure.Equals(this._previousWP))
			{
				this.UpdateCore();
			}
			this._previousWP = structure;
			return IntPtr.Zero;
		}

		private void HideGlow()
		{
			if (this.left != null)
			{
				this.left.IsGlowing = false;
			}
			if (this.top != null)
			{
				this.top.IsGlowing = false;
			}
			if (this.right != null)
			{
				this.right.IsGlowing = false;
			}
			if (this.bottom != null)
			{
				this.bottom.IsGlowing = false;
			}
			this.Update();
		}

		private void makeGlowVisibleTimer_Tick(object sender, EventArgs e)
		{
			if (this.makeGlowVisibleTimer != null)
			{
				this.makeGlowVisibleTimer.Stop();
			}
			this.RestoreGlow();
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.SourceInitialized += new EventHandler((object sender, EventArgs e) => {
				if (this.IsGlowDisabled)
				{
					return;
				}
				this.handle = (new WindowInteropHelper(base.AssociatedObject)).Handle;
				HwndSource hwndSource = HwndSource.FromHwnd(this.handle);
				if (hwndSource != null)
				{
					hwndSource.AddHook(new HwndSourceHook(this.AssociatedObjectWindowProc));
				}
			});
			base.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObjectOnLoaded);
			base.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObjectUnloaded);
		}

		private void RestoreGlow()
		{
			if (this.left != null)
			{
				this.left.IsGlowing = true;
			}
			if (this.top != null)
			{
				this.top.IsGlowing = true;
			}
			if (this.right != null)
			{
				this.right.IsGlowing = true;
			}
			if (this.bottom != null)
			{
				this.bottom.IsGlowing = true;
			}
			this.Update();
		}

		private void SetOpacityTo(double newOpacity)
		{
			if (this.left != null)
			{
				this.left.Opacity = newOpacity;
			}
			if (this.right != null)
			{
				this.right.Opacity = newOpacity;
			}
			if (this.top != null)
			{
				this.top.Opacity = newOpacity;
			}
			if (this.bottom != null)
			{
				this.bottom.Opacity = newOpacity;
			}
		}

		private void Show()
		{
			if (this.left != null)
			{
				this.left.Show();
			}
			if (this.right != null)
			{
				this.right.Show();
			}
			if (this.top != null)
			{
				this.top.Show();
			}
			if (this.bottom != null)
			{
				this.bottom.Show();
			}
		}

		private void StartOpacityStoryboard()
		{
			if (this.left != null && this.left.OpacityStoryboard != null)
			{
				this.left.BeginStoryboard(this.left.OpacityStoryboard);
			}
			if (this.right != null && this.right.OpacityStoryboard != null)
			{
				this.right.BeginStoryboard(this.right.OpacityStoryboard);
			}
			if (this.top != null && this.top.OpacityStoryboard != null)
			{
				this.top.BeginStoryboard(this.top.OpacityStoryboard);
			}
			if (this.bottom != null && this.bottom.OpacityStoryboard != null)
			{
				this.bottom.BeginStoryboard(this.bottom.OpacityStoryboard);
			}
		}

		private void Update()
		{
			if (this.left != null)
			{
				this.left.Update();
			}
			if (this.right != null)
			{
				this.right.Update();
			}
			if (this.top != null)
			{
				this.top.Update();
			}
			if (this.bottom != null)
			{
				this.bottom.Update();
			}
		}

		private void UpdateCore()
		{
			MahApps.Metro.Native.RECT rECT;
			if (this.handle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(this.handle, out rECT))
			{
				if (this.left != null)
				{
					this.left.UpdateCore(rECT);
				}
				if (this.right != null)
				{
					this.right.UpdateCore(rECT);
				}
				if (this.top != null)
				{
					this.top.UpdateCore(rECT);
				}
				if (this.bottom != null)
				{
					this.bottom.UpdateCore(rECT);
				}
			}
		}
	}
}