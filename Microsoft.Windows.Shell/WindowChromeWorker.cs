using Standard;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace Microsoft.Windows.Shell
{
	internal class WindowChromeWorker : DependencyObject
	{
		private readonly static Version _presentationFrameworkVersion;

		private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;

		private Window _window;

		[SecurityCritical]
		private IntPtr _hwnd;

		[SecurityCritical]
		private HwndSource _hwndSource;

		private bool _isHooked;

		private bool _isFixedUp;

		private bool _isUserResizing;

		private bool _hasUserMovedWindow;

		private Point _windowPosAtStartOfUserMove;

		private WindowChrome _chromeInfo;

		private WindowState _lastRoundingState;

		private WindowState _lastMenuState;

		private bool _isGlassEnabled;

		private WINDOWPOS _previousWP;

		public readonly static DependencyProperty WindowChromeWorkerProperty;

		private readonly static HT[,] _HitTestBorders;

		private bool _IsWindowDocked
		{
			[SecurityCritical]
			get
			{
				if (this._window.WindowState != WindowState.Normal)
				{
					return false;
				}
				RECT rECT = new RECT()
				{
					Bottom = 100,
					Right = 100
				};
				RECT rECT1 = this._GetAdjustedWindowRect(rECT);
				Point point = new Point(this._window.Left, this._window.Top);
				point -= (Vector)DpiHelper.DevicePixelsToLogical(new Point((double)rECT1.Left, (double)rECT1.Top));
				return this._window.RestoreBounds.Location != point;
			}
		}

		private bool _MinimizeAnimation
		{
			get
			{
				if (!SystemParameters.MinimizeAnimation)
				{
					return false;
				}
				return !this._chromeInfo.IgnoreTaskbarOnMaximize;
			}
		}

		private static bool IsPresentationFrameworkVersionLessThan4
		{
			get
			{
				return WindowChromeWorker._presentationFrameworkVersion < new Version(4, 0);
			}
		}

		static WindowChromeWorker()
		{
			Class6.yDnXvgqzyB5jw();
			WindowChromeWorker._presentationFrameworkVersion = Assembly.GetAssembly(typeof(Window)).GetName().Version;
			WindowChromeWorker.WindowChromeWorkerProperty = DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker), typeof(WindowChromeWorker), new PropertyMetadata(null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));
			WindowChromeWorker._HitTestBorders = new HT[,] { typeof(<PrivateImplementationDetails>{731C3D62-B731-48E9-B118-C3FF2B4304E4}).GetField("struct2_0").FieldHandle };
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		public WindowChromeWorker()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._messageTable = new List<KeyValuePair<WM, MessageHandler>>()
			{
				new KeyValuePair<WM, MessageHandler>(WM.NCUAHDRAWCAPTION, new MessageHandler(this._HandleNCUAHDrawCaption)),
				new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.SYSCOMMAND, new MessageHandler(this._HandleRestoreWindow)),
				new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
				new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
				new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
				new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
				new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGING, new MessageHandler(this._HandleWindowPosChanging)),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
				new KeyValuePair<WM, MessageHandler>(WM.GETMINMAXINFO, new MessageHandler(this._HandleGetMinMaxInfo)),
				new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged)),
				new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMoveForAnimation)),
				new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMoveForRealSize)),
				new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMoveForAnimation))
			};
			if (WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
			{
				this._messageTable.AddRange((IEnumerable<!0>)(new KeyValuePair<WM, MessageHandler>[] { new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)), new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)), new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)), new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove)) }));
			}
		}

		[SecurityCritical]
		private void _ApplyNewCustomChrome()
		{
			if (this._hwnd == IntPtr.Zero || this._hwndSource.IsDisposed)
			{
				return;
			}
			if (this._chromeInfo == null)
			{
				this._RestoreStandardChromeState(false);
				return;
			}
			if (!this._isHooked)
			{
				this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
				this._isHooked = true;
			}
			this._ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
			this._FixupTemplateIssues();
			this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
			this._UpdateFrameState(true);
			Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.FRAMECHANGED | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOREPOSITION | SWP.NOSIZE | SWP.NOZORDER);
		}

		[SecurityCritical]
		private void _ClearRoundingRegion()
		{
			Standard.NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, Standard.NativeMethods.IsWindowVisible(this._hwnd));
		}

		[SecurityCritical]
		private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
		{
			IntPtr zero = IntPtr.Zero;
			try
			{
				zero = WindowChromeWorker._CreateRoundRectRgn(region, radius);
				if (Standard.NativeMethods.CombineRgn(hrgnSource, hrgnSource, zero, RGN.OR) == CombineRgnResult.ERROR)
				{
					throw new InvalidOperationException("Unable to combine two HRGNs.");
				}
			}
			catch
			{
				Utility.SafeDeleteObject(ref zero);
				throw;
			}
		}

		[SecurityCritical]
		private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
		{
			if (DoubleUtilities.AreClose(0, radius))
			{
				return Standard.NativeMethods.CreateRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right), (int)Math.Ceiling(region.Bottom));
			}
			return Standard.NativeMethods.CreateRoundRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right) + 1, (int)Math.Ceiling(region.Bottom) + 1, (int)Math.Ceiling(radius), (int)Math.Ceiling(radius));
		}

		[SecurityCritical]
		private void _ExtendGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return;
			}
			if (IntPtr.Zero == this._hwnd)
			{
				return;
			}
			if (!Standard.NativeMethods.DwmIsCompositionEnabled())
			{
				if (this._window.AllowsTransparency)
				{
					this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
					return;
				}
				this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
				return;
			}
			if (this._window.AllowsTransparency)
			{
				this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
			}
			Thickness device = DpiHelper.LogicalThicknessToDevice(this._chromeInfo.GlassFrameThickness);
			if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
			{
				Thickness thickness = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness);
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 2))
				{
					device.Top = device.Top - thickness.Top;
					device.Top = Math.Max(0, device.Top);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 1))
				{
					device.Left = device.Left - thickness.Left;
					device.Left = Math.Max(0, device.Left);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 8))
				{
					device.Bottom = device.Bottom - thickness.Bottom;
					device.Bottom = Math.Max(0, device.Bottom);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 4))
				{
					device.Right = device.Right - thickness.Right;
					device.Right = Math.Max(0, device.Right);
				}
			}
			MARGINS mARGIN = new MARGINS()
			{
				cxLeftWidth = (int)Math.Ceiling(device.Left),
				cxRightWidth = (int)Math.Ceiling(device.Right),
				cyTopHeight = (int)Math.Ceiling(device.Top),
				cyBottomHeight = (int)Math.Ceiling(device.Bottom)
			};
			MARGINS mARGIN1 = mARGIN;
			Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref mARGIN1);
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private void _FixupRestoreBounds(object sender, EventArgs e)
		{
			if ((this._window.WindowState == WindowState.Maximized || this._window.WindowState == WindowState.Minimized) && this._hasUserMovedWindow)
			{
				this._hasUserMovedWindow = false;
				WINDOWPLACEMENT windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd);
				RECT rECT = new RECT()
				{
					Bottom = 100,
					Right = 100
				};
				RECT rECT1 = this._GetAdjustedWindowRect(rECT);
				Point logical = DpiHelper.DevicePixelsToLogical(new Point((double)(windowPlacement.rcNormalPosition.Left - rECT1.Left), (double)(windowPlacement.rcNormalPosition.Top - rECT1.Top)));
				this._window.Top = logical.Y;
				this._window.Left = logical.X;
			}
		}

		[SecurityCritical]
		private void _FixupTemplateIssues()
		{
			Thickness windowResizeBorderThickness;
			if (this._window.Template == null)
			{
				return;
			}
			if (VisualTreeHelper.GetChildrenCount(this._window) == 0)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupTemplateIssues));
				return;
			}
			Thickness top = new Thickness();
			Transform matrixTransform = null;
			FrameworkElement child = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
			{
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 2))
				{
					double num = top.Top;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Top = num - windowResizeBorderThickness.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 1))
				{
					double left = top.Left;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Left = left - windowResizeBorderThickness.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 8))
				{
					double bottom = top.Bottom;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Bottom = bottom - windowResizeBorderThickness.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 4))
				{
					double right = top.Right;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Right = right - windowResizeBorderThickness.Right;
				}
			}
			if (WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
			{
				RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
				RECT rECT = this._GetAdjustedWindowRect(windowRect);
				Rect logical = DpiHelper.DeviceRectToLogical(new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height));
				Rect rect = DpiHelper.DeviceRectToLogical(new Rect((double)rECT.Left, (double)rECT.Top, (double)rECT.Width, (double)rECT.Height));
				if (!Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 1))
				{
					double right1 = top.Right;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Right = right1 - windowResizeBorderThickness.Left;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 4))
				{
					double num1 = top.Right;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Right = num1 - windowResizeBorderThickness.Right;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 2))
				{
					double bottom1 = top.Bottom;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Bottom = bottom1 - windowResizeBorderThickness.Top;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 8))
				{
					double bottom2 = top.Bottom;
					windowResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
					top.Bottom = bottom2 - windowResizeBorderThickness.Bottom;
				}
				top.Bottom = top.Bottom - SystemParameters.WindowCaptionHeight;
				if (this._window.FlowDirection != FlowDirection.RightToLeft)
				{
					matrixTransform = null;
				}
				else
				{
					Thickness thickness = new Thickness(logical.Left - rect.Left, logical.Top - rect.Top, rect.Right - logical.Right, rect.Bottom - logical.Bottom);
					matrixTransform = new MatrixTransform(1, 0, 0, 1, -(thickness.Left + thickness.Right), 0);
				}
				child.RenderTransform = matrixTransform;
			}
			child.Margin = top;
			if (WindowChromeWorker.IsPresentationFrameworkVersionLessThan4 && !this._isFixedUp)
			{
				this._hasUserMovedWindow = false;
				this._window.StateChanged += new EventHandler(this._FixupRestoreBounds);
				this._isFixedUp = true;
			}
		}

		[SecurityCritical]
		private RECT _GetAdjustedWindowRect(RECT rcWindow)
		{
			WS windowLongPtr = (WS)((int)Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE));
			WS_EX wSEX = (WS_EX)((int)Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE));
			return Standard.NativeMethods.AdjustWindowRectEx(rcWindow, windowLongPtr, false, wSEX);
		}

		[SecurityCritical]
		private RECT _GetClientRectRelativeToWindowRect(IntPtr hWnd)
		{
			RECT windowRect = Standard.NativeMethods.GetWindowRect(hWnd);
			RECT clientRect = Standard.NativeMethods.GetClientRect(hWnd);
			POINT pOINT = new POINT()
			{
				x = 0,
				y = 0
			};
			POINT pOINT1 = pOINT;
			Standard.NativeMethods.ClientToScreen(hWnd, ref pOINT1);
			if (this._window.FlowDirection != FlowDirection.RightToLeft)
			{
				clientRect.Offset(pOINT1.x - windowRect.Left, pOINT1.y - windowRect.Top);
			}
			else
			{
				clientRect.Offset(windowRect.Right - pOINT1.x, pOINT1.y - windowRect.Top);
			}
			return clientRect;
		}

		private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
		{
			bool flowDirection = this._window.FlowDirection == FlowDirection.RightToLeft;
			switch (direction)
			{
				case ResizeGripDirection.TopLeft:
				{
					if (!flowDirection)
					{
						return HT.TOPLEFT;
					}
					return HT.TOPRIGHT;
				}
				case ResizeGripDirection.Top:
				{
					return HT.TOP;
				}
				case ResizeGripDirection.TopRight:
				{
					if (!flowDirection)
					{
						return HT.TOPRIGHT;
					}
					return HT.TOPLEFT;
				}
				case ResizeGripDirection.Right:
				{
					if (!flowDirection)
					{
						return HT.RIGHT;
					}
					return HT.LEFT;
				}
				case ResizeGripDirection.BottomRight:
				{
					if (!flowDirection)
					{
						return HT.BOTTOMRIGHT;
					}
					return HT.BOTTOMLEFT;
				}
				case ResizeGripDirection.Bottom:
				{
					return HT.BOTTOM;
				}
				case ResizeGripDirection.BottomLeft:
				{
					if (!flowDirection)
					{
						return HT.BOTTOMLEFT;
					}
					return HT.BOTTOMRIGHT;
				}
				case ResizeGripDirection.Left:
				{
					if (!flowDirection)
					{
						return HT.LEFT;
					}
					return HT.RIGHT;
				}
				case ResizeGripDirection.Caption:
				{
					return HT.CAPTION;
				}
				default:
				{
					return HT.NOWHERE;
				}
			}
		}

		[SecurityCritical]
		private WindowState _GetHwndState()
		{
			SW windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd;
			if (windowPlacement == SW.SHOWMINIMIZED)
			{
				return WindowState.Minimized;
			}
			if (windowPlacement != SW.SHOWMAXIMIZED)
			{
				return WindowState.Normal;
			}
			return WindowState.Maximized;
		}

		[SecurityCritical]
		private Rect _GetWindowRect()
		{
			RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
			return new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height);
		}

		[SecurityCritical]
		private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateFrameState(false);
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = true;
			if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
			{
				this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleEnterSizeMoveForAnimation(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._MinimizeAnimation && this._GetHwndState() == WindowState.Maximized)
			{
				this._ModifyStyle(WS.DLGFRAME, WS.OVERLAPPED);
			}
			handled = false;
			return IntPtr.Zero;
		}

		private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = false;
			if (this._window.WindowState == WindowState.Maximized)
			{
				this._window.Top = this._windowPosAtStartOfUserMove.Y;
				this._window.Left = this._windowPosAtStartOfUserMove.X;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleExitSizeMoveForAnimation(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._MinimizeAnimation && this._ModifyStyle(WS.OVERLAPPED, WS.DLGFRAME))
			{
				this._UpdateFrameState(true);
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleGetMinMaxInfo(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._chromeInfo.IgnoreTaskbarOnMaximize && Standard.NativeMethods.IsZoomed(this._hwnd))
			{
				MINMAXINFO structure = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
				IntPtr intPtr = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2);
				if (intPtr != IntPtr.Zero)
				{
					MONITORINFO monitorInfoW = Standard.NativeMethods.GetMonitorInfoW(intPtr);
					RECT rECT = monitorInfoW.rcWork;
					RECT rECT1 = monitorInfoW.rcMonitor;
					structure.ptMaxPosition.x = Math.Abs(rECT.Left - rECT1.Left);
					structure.ptMaxPosition.y = Math.Abs(rECT.Top - rECT1.Top);
					structure.ptMaxSize.x = Math.Abs(monitorInfoW.rcMonitor.Width);
					structure.ptMaxSize.y = Math.Abs(monitorInfoW.rcMonitor.Height);
					structure.ptMaxTrackSize.x = structure.ptMaxSize.x;
					structure.ptMaxTrackSize.y = structure.ptMaxSize.y;
				}
				Marshal.StructureToPtr(structure, lParam, true);
			}
			handled = false;
			return IntPtr.Zero;
		}

		private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._isUserResizing)
			{
				this._hasUserMovedWindow = true;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleMoveForRealSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._GetHwndState() == WindowState.Maximized)
			{
				IntPtr intPtr = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2);
				if (intPtr != IntPtr.Zero)
				{
					bool ignoreTaskbarOnMaximize = this._chromeInfo.IgnoreTaskbarOnMaximize;
					MONITORINFO monitorInfoW = Standard.NativeMethods.GetMonitorInfoW(intPtr);
					RECT rECT = (ignoreTaskbarOnMaximize ? monitorInfoW.rcMonitor : monitorInfoW.rcWork);
					Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, rECT.Left, rECT.Top, rECT.Width, rECT.Height, SWP.ASYNCWINDOWPOS | SWP.DRAWFRAME | SWP.FRAMECHANGED | SWP.NOCOPYBITS);
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr intPtr = Standard.NativeMethods.DefWindowProcW(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
			handled = true;
			return intPtr;
		}

		[SecurityCritical]
		private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd == SW.SHOWMAXIMIZED && this._MinimizeAnimation)
			{
				IntPtr intPtr = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2);
				MONITORINFO monitorInfo = Standard.NativeMethods.GetMonitorInfo(intPtr);
				RECT structure = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				Standard.NativeMethods.DefWindowProcW(this._hwnd, WM.NCCALCSIZE, wParam, lParam);
				RECT top = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				top.Top = (int)((long)structure.Top + (ulong)Standard.NativeMethods.GetWindowInfo(this._hwnd).cyWindowBorders);
				if (monitorInfo.rcMonitor.Height == monitorInfo.rcWork.Height && monitorInfo.rcMonitor.Width == monitorInfo.rcWork.Width)
				{
					top = WindowChromeWorker.AdjustWorkingAreaForAutoHide(intPtr, top);
				}
				Marshal.StructureToPtr(top, lParam, true);
			}
			if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
			{
				Thickness device = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness);
				RECT left = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 2))
				{
					left.Top = left.Top + (int)device.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 1))
				{
					left.Left = left.Left + (int)device.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 8))
				{
					left.Bottom = left.Bottom - (int)device.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.SacrificialEdge, 4))
				{
					left.Right = left.Right - (int)device.Right;
				}
				Marshal.StructureToPtr(left, lParam, false);
			}
			handled = true;
			return new IntPtr(1792);
		}

		[SecurityCritical]
		private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr intPtr;
			Point point = new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam));
			Rect rect = this._GetWindowRect();
			Point logical = point;
			logical.Offset(-rect.X, -rect.Y);
			logical = DpiHelper.DevicePixelsToLogical(logical);
			IInputElement inputElement = this._window.InputHitTest(logical);
			if (inputElement != null)
			{
				if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
				{
					handled = true;
					return new IntPtr(1);
				}
				ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
				if (resizeGripDirection != ResizeGripDirection.None)
				{
					handled = true;
					return new IntPtr((int)this._GetHTFromResizeGripDirection(resizeGripDirection));
				}
			}
			if (this._chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer)
			{
				if (this._chromeInfo.GlassFrameThickness != new Thickness() && this._isGlassEnabled)
				{
					handled = Standard.NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out intPtr);
					if (IntPtr.Zero != intPtr)
					{
						return intPtr;
					}
				}
			}
			HT hT = this._HitTestNca(DpiHelper.DeviceRectToLogical(rect), DpiHelper.DevicePixelsToLogical(point));
			handled = true;
			return new IntPtr((int)hT);
		}

		[SecurityCritical]
		private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (2 == (int)((Environment.Is64BitProcess ? wParam.ToInt64() : (long)wParam.ToInt32())))
			{
				Microsoft.Windows.Shell.SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam)));
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleNCUAHDrawCaption(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._window.ShowInTaskbar || this._GetHwndState() != WindowState.Minimized)
			{
				handled = false;
				return IntPtr.Zero;
			}
			bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr intPtr = Standard.NativeMethods.DefWindowProcW(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return intPtr;
		}

		[SecurityCritical]
		private IntPtr _HandleRestoreWindow(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WINDOWPLACEMENT windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd);
			if (61728 != (int)((Environment.Is64BitProcess ? wParam.ToInt64() : (long)wParam.ToInt32())) || windowPlacement.showCmd != SW.SHOWMAXIMIZED || !this._MinimizeAnimation)
			{
				handled = false;
				return IntPtr.Zero;
			}
			bool flag = this._ModifyStyle(WS.SYSMENU, WS.OVERLAPPED);
			IntPtr intPtr = Standard.NativeMethods.DefWindowProcW(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				flag = this._ModifyStyle(WS.OVERLAPPED, WS.SYSMENU);
			}
			handled = true;
			return intPtr;
		}

		private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr intPtr = Standard.NativeMethods.DefWindowProcW(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return intPtr;
		}

		[SecurityCritical]
		private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._FixupTemplateIssues();
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WindowState? nullable = null;
			if ((Environment.Is64BitProcess ? wParam.ToInt64() : (long)wParam.ToInt32()) == 2L)
			{
				nullable = new WindowState?(WindowState.Maximized);
			}
			this._UpdateSystemMenu(nullable);
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateSystemMenu(null);
			if (!this._isGlassEnabled)
			{
				WINDOWPOS structure = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if (!structure.Equals(this._previousWP))
				{
					this._previousWP = structure;
					this._SetRoundingRegion(new WINDOWPOS?(structure));
				}
				this._previousWP = structure;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleWindowPosChanging(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (!this._isGlassEnabled)
			{
				WINDOWPOS structure = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if (this._chromeInfo.IgnoreTaskbarOnMaximize && this._GetHwndState() == WindowState.Maximized && structure.flags == 32)
				{
					structure.flags = 0;
					Marshal.StructureToPtr(structure, lParam, true);
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		private HT _HitTestNca(Rect windowPosition, Point mousePosition)
		{
			int num = 1;
			int num1 = 1;
			bool top = false;
			if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
			{
				double y = mousePosition.Y;
				double top1 = windowPosition.Top;
				Thickness resizeBorderThickness = this._chromeInfo.ResizeBorderThickness;
				top = y < top1 + resizeBorderThickness.Top;
				num = 0;
			}
			else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double)((int)this._chromeInfo.ResizeBorderThickness.Bottom))
			{
				num = 2;
			}
			if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double)((int)this._chromeInfo.ResizeBorderThickness.Left))
			{
				num1 = 0;
			}
			else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
			{
				num1 = 2;
			}
			if (num == 0 && num1 != 1 && !top)
			{
				num = 1;
			}
			HT hT = WindowChromeWorker._HitTestBorders[num, num1];
			if (hT == HT.TOP && !top)
			{
				hT = HT.CAPTION;
			}
			return hT;
		}

		private static bool _IsUniform(CornerRadius cornerRadius)
		{
			if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight))
			{
				return false;
			}
			if (!DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight))
			{
				return false;
			}
			if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight))
			{
				return false;
			}
			return true;
		}

		[SecurityCritical]
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			IntPtr windowLongPtr = Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
			WS w = (WS)((uint)((Environment.Is64BitProcess ? windowLongPtr.ToInt64() : (long)windowLongPtr.ToInt32())));
			WS w1 = w & ~removeStyle | addStyle;
			if (w == w1)
			{
				return false;
			}
			Standard.NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int)w1));
			return true;
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
		{
			this._UpdateFrameState(true);
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			((WindowChromeWorker)e.NewValue)._SetWindow(window);
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
		{
			if (this._chromeInfo != null && this._hwnd != IntPtr.Zero)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupTemplateIssues));
			}
		}

		[SecurityCritical]
		private void _RestoreFrameworkIssueFixups()
		{
			FrameworkElement child = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			child.Margin = new Thickness();
			if (WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
			{
				this._window.StateChanged -= new EventHandler(this._FixupRestoreBounds);
				this._isFixedUp = false;
			}
		}

		[SecurityCritical]
		private void _RestoreGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
			{
				return;
			}
			this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
			if (Standard.NativeMethods.DwmIsCompositionEnabled())
			{
				MARGINS mARGIN = new MARGINS();
				Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref mARGIN);
			}
		}

		[SecurityCritical]
		private void _RestoreHrgn()
		{
			this._ClearRoundingRegion();
			Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.FRAMECHANGED | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOREPOSITION | SWP.NOSIZE | SWP.NOZORDER);
		}

		[SecurityCritical]
		private void _RestoreStandardChromeState(bool isClosing)
		{
			base.VerifyAccess();
			this._UnhookCustomChrome();
			if (!isClosing && !this._hwndSource.IsDisposed)
			{
				this._RestoreFrameworkIssueFixups();
				this._RestoreGlassFrame();
				this._RestoreHrgn();
				this._window.InvalidateMeasure();
			}
		}

		[SecurityCritical]
		private void _SetRoundingRegion(WINDOWPOS? wp)
		{
			RECT windowRect;
			int left;
			int top;
			Size size;
			if (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd != SW.SHOWMAXIMIZED)
			{
				if (!wp.HasValue || Utility.IsFlagSet(wp.Value.flags, 1))
				{
					if (wp.HasValue && this._lastRoundingState == this._window.WindowState)
					{
						return;
					}
					size = this._GetWindowRect().Size;
				}
				else
				{
					size = new Size((double)wp.Value.cx, (double)wp.Value.cy);
				}
				this._lastRoundingState = this._window.WindowState;
				IntPtr zero = IntPtr.Zero;
				try
				{
					double num = Math.Min(size.Width, size.Height);
					CornerRadius cornerRadius = this._chromeInfo.CornerRadius;
					Point device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.TopLeft, 0));
					double x = device.X;
					x = Math.Min(x, num / 2);
					if (!WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
					{
						zero = WindowChromeWorker._CreateRoundRectRgn(new Rect(0, 0, size.Width / 2 + x, size.Height / 2 + x), x);
						cornerRadius = this._chromeInfo.CornerRadius;
						device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.TopRight, 0));
						double x1 = device.X;
						x1 = Math.Min(x1, num / 2);
						Rect rect = new Rect(0, 0, size.Width / 2 + x1, size.Height / 2 + x1);
						rect.Offset(size.Width / 2 - x1, 0);
						WindowChromeWorker._CreateAndCombineRoundRectRgn(zero, rect, x1);
						cornerRadius = this._chromeInfo.CornerRadius;
						device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.BottomLeft, 0));
						double num1 = device.X;
						num1 = Math.Min(num1, num / 2);
						Rect rect1 = new Rect(0, 0, size.Width / 2 + num1, size.Height / 2 + num1);
						rect1.Offset(0, size.Height / 2 - num1);
						WindowChromeWorker._CreateAndCombineRoundRectRgn(zero, rect1, num1);
						cornerRadius = this._chromeInfo.CornerRadius;
						device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.BottomRight, 0));
						double x2 = device.X;
						x2 = Math.Min(x2, num / 2);
						Rect rect2 = new Rect(0, 0, size.Width / 2 + x2, size.Height / 2 + x2);
						rect2.Offset(size.Width / 2 - x2, size.Height / 2 - x2);
						WindowChromeWorker._CreateAndCombineRoundRectRgn(zero, rect2, x2);
					}
					else
					{
						zero = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), x);
					}
					Standard.NativeMethods.SetWindowRgn(this._hwnd, zero, Standard.NativeMethods.IsWindowVisible(this._hwnd));
					zero = IntPtr.Zero;
				}
				finally
				{
					Utility.SafeDeleteObject(ref zero);
				}
			}
			else
			{
				if (!this._MinimizeAnimation)
				{
					if (!wp.HasValue)
					{
						Rect rect3 = this._GetWindowRect();
						left = (int)rect3.Left;
						top = (int)rect3.Top;
					}
					else
					{
						left = wp.Value.x;
						top = wp.Value.y;
					}
					IntPtr intPtr = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2);
					MONITORINFO monitorInfo = Standard.NativeMethods.GetMonitorInfo(intPtr);
					windowRect = (this._chromeInfo.IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork);
					windowRect.Offset(-left, -top);
				}
				else
				{
					windowRect = this._GetClientRectRelativeToWindowRect(this._hwnd);
				}
				IntPtr zero1 = IntPtr.Zero;
				try
				{
					zero1 = Standard.NativeMethods.CreateRectRgnIndirect(windowRect);
					Standard.NativeMethods.SetWindowRgn(this._hwnd, zero1, Standard.NativeMethods.IsWindowVisible(this._hwnd));
					zero1 = IntPtr.Zero;
				}
				finally
				{
					Utility.SafeDeleteObject(ref zero1);
				}
			}
		}

		[SecurityCritical]
		private void _SetWindow(Window window)
		{
			this.UnsubscribeWindowEvents();
			this._window = window;
			this._hwnd = (new WindowInteropHelper(this._window)).Handle;
			Utility.AddDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			Utility.AddDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			this._window.Closed += new EventHandler(this._UnsetWindow);
			if (IntPtr.Zero == this._hwnd)
			{
				this._window.SourceInitialized += new EventHandler(this._WindowSourceInitialized);
			}
			else
			{
				this._hwndSource = HwndSource.FromHwnd(this._hwnd);
				this._window.ApplyTemplate();
				if (this._chromeInfo != null)
				{
					this._ApplyNewCustomChrome();
					return;
				}
			}
		}

		[SecurityCritical]
		private void _UnhookCustomChrome()
		{
			if (this._isHooked)
			{
				this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
				this._isHooked = false;
			}
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private void _UnsetWindow(object sender, EventArgs e)
		{
			this.UnsubscribeWindowEvents();
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
			}
			this._RestoreStandardChromeState(true);
		}

		[SecurityCritical]
		private void _UpdateFrameState(bool force)
		{
			if (IntPtr.Zero == this._hwnd || this._hwndSource.IsDisposed)
			{
				return;
			}
			bool flag = Standard.NativeMethods.DwmIsCompositionEnabled();
			if (force || flag != this._isGlassEnabled)
			{
				this._isGlassEnabled = (!flag ? false : this._chromeInfo.GlassFrameThickness != new Thickness());
				if (this._isGlassEnabled)
				{
					this._ClearRoundingRegion();
					this._ExtendGlassFrame();
				}
				else
				{
					this._SetRoundingRegion(null);
				}
				if (!this._MinimizeAnimation)
				{
					this._ModifyStyle(WS.CAPTION, WS.OVERLAPPED);
				}
				else
				{
					this._ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
				}
				Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.FRAMECHANGED | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOREPOSITION | SWP.NOSIZE | SWP.NOZORDER);
			}
		}

		[SecurityCritical]
		private void _UpdateSystemMenu(WindowState? assumeState)
		{
			WindowState? nullable = assumeState;
			WindowState windowState = (nullable.HasValue ? nullable.GetValueOrDefault() : this._GetHwndState());
			if (assumeState.HasValue || this._lastMenuState != windowState)
			{
				this._lastMenuState = windowState;
				IntPtr systemMenu = Standard.NativeMethods.GetSystemMenu(this._hwnd, false);
				if (IntPtr.Zero != systemMenu)
				{
					IntPtr windowLongPtr = Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
					WS w = (WS)((uint)((Environment.Is64BitProcess ? windowLongPtr.ToInt64() : (long)windowLongPtr.ToInt32())));
					bool flag = Utility.IsFlagSet((int)w, 131072);
					bool flag1 = Utility.IsFlagSet((int)w, 65536);
					bool flag2 = Utility.IsFlagSet((int)w, 262144);
					if (windowState == WindowState.Minimized)
					{
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, (flag1 ? MF.ENABLED : MF.GRAYED | MF.DISABLED));
						return;
					}
					if (windowState == WindowState.Maximized)
					{
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, (flag ? MF.ENABLED : MF.GRAYED | MF.DISABLED));
						Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
						return;
					}
					Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
					Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
					Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, (flag2 ? MF.ENABLED : MF.GRAYED | MF.DISABLED));
					Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, (flag ? MF.ENABLED : MF.GRAYED | MF.DISABLED));
					Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, (flag1 ? MF.ENABLED : MF.GRAYED | MF.DISABLED));
				}
			}
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		private void _WindowSourceInitialized(object sender, EventArgs e)
		{
			this._hwnd = (new WindowInteropHelper(this._window)).Handle;
			this._hwndSource = HwndSource.FromHwnd(this._hwnd);
			if (this._chromeInfo != null)
			{
				this._ApplyNewCustomChrome();
			}
		}

		[SecurityCritical]
		private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr value;
			WM wM = (WM)msg;
			List<KeyValuePair<WM, MessageHandler>>.Enumerator enumerator = this._messageTable.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<WM, MessageHandler> current = enumerator.Current;
					if (current.Key != wM)
					{
						continue;
					}
					value = current.Value(wM, wParam, lParam, out handled);
					return value;
				}
				return IntPtr.Zero;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return value;
		}

		[SecurityCritical]
		private static RECT AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, RECT area)
		{
			IntPtr intPtr = Standard.NativeMethods.FindWindow("Shell_TrayWnd", null);
			IntPtr intPtr1 = Standard.NativeMethods.MonitorFromWindow(intPtr, 2);
			APPBARDATA aPPBARDATum = new APPBARDATA()
			{
				cbSize = Marshal.SizeOf(aPPBARDATum),
				hWnd = intPtr
			};
			Standard.NativeMethods.SHAppBarMessage(5, ref aPPBARDATum);
			if (!Convert.ToBoolean(Standard.NativeMethods.SHAppBarMessage(4, ref aPPBARDATum)) || !object.Equals(monitorContainingApplication, intPtr1))
			{
				return area;
			}
			switch (aPPBARDATum.uEdge)
			{
				case 0:
				{
					area.Left = area.Left + 2;
					break;
				}
				case 1:
				{
					area.Top = area.Top + 2;
					break;
				}
				case 2:
				{
					area.Right = area.Right - 2;
					break;
				}
				case 3:
				{
					area.Bottom = area.Bottom - 2;
					break;
				}
				default:
				{
					return area;
				}
			}
			return area;
		}

		public static WindowChromeWorker GetWindowChromeWorker(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChromeWorker)window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
		}

		[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
		[SecuritySafeCritical]
		public void SetWindowChrome(WindowChrome newChrome)
		{
			base.VerifyAccess();
			if (newChrome == this._chromeInfo)
			{
				return;
			}
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
			}
			this._chromeInfo = newChrome;
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint += new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
			}
			this._ApplyNewCustomChrome();
		}

		public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, chrome);
		}

		[SecurityCritical]
		private void UnsubscribeWindowEvents()
		{
			if (this._window != null)
			{
				Utility.RemoveDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				Utility.RemoveDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				this._window.SourceInitialized -= new EventHandler(this._WindowSourceInitialized);
				this._window.StateChanged -= new EventHandler(this._FixupRestoreBounds);
			}
		}

		private delegate void _Action();
	}
}