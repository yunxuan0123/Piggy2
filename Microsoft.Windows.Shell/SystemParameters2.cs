using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Microsoft.Windows.Shell
{
	internal class SystemParameters2 : INotifyPropertyChanged
	{
		[ThreadStatic]
		private static SystemParameters2 _threadLocalSingleton;

		private MessageWindow _messageHwnd;

		private bool _isGlassEnabled;

		private Color _glassColor;

		private SolidColorBrush _glassColorBrush;

		private Thickness _windowResizeBorderThickness;

		private Thickness _windowNonClientFrameThickness;

		private double _captionHeight;

		private Size _smallIconSize;

		private string _uxThemeName;

		private string _uxThemeColor;

		private bool _isHighContrast;

		private CornerRadius _windowCornerRadius;

		private Rect _captionButtonLocation;

		private readonly Dictionary<WM, List<SystemParameters2._SystemMetricUpdate>> _UpdateTable;

		public static SystemParameters2 Current
		{
			get
			{
				if (SystemParameters2._threadLocalSingleton == null)
				{
					SystemParameters2._threadLocalSingleton = new SystemParameters2();
				}
				return SystemParameters2._threadLocalSingleton;
			}
		}

		public bool HighContrast
		{
			get
			{
				return this._isHighContrast;
			}
			private set
			{
				if (value != this._isHighContrast)
				{
					this._isHighContrast = value;
					this._NotifyPropertyChanged("HighContrast");
				}
			}
		}

		public bool IsGlassEnabled
		{
			get
			{
				return Standard.NativeMethods.DwmIsCompositionEnabled();
			}
			private set
			{
				if (value != this._isGlassEnabled)
				{
					this._isGlassEnabled = value;
					this._NotifyPropertyChanged("IsGlassEnabled");
				}
			}
		}

		public Size SmallIconSize
		{
			get
			{
				return new Size(this._smallIconSize.Width, this._smallIconSize.Height);
			}
			private set
			{
				if (value != this._smallIconSize)
				{
					this._smallIconSize = value;
					this._NotifyPropertyChanged("SmallIconSize");
				}
			}
		}

		public string UxThemeColor
		{
			get
			{
				return this._uxThemeColor;
			}
			private set
			{
				if (value != this._uxThemeColor)
				{
					this._uxThemeColor = value;
					this._NotifyPropertyChanged("UxThemeColor");
				}
			}
		}

		public string UxThemeName
		{
			get
			{
				return this._uxThemeName;
			}
			private set
			{
				if (value != this._uxThemeName)
				{
					this._uxThemeName = value;
					this._NotifyPropertyChanged("UxThemeName");
				}
			}
		}

		public Rect WindowCaptionButtonsLocation
		{
			get
			{
				return this._captionButtonLocation;
			}
			private set
			{
				if (value != this._captionButtonLocation)
				{
					this._captionButtonLocation = value;
					this._NotifyPropertyChanged("WindowCaptionButtonsLocation");
				}
			}
		}

		public double WindowCaptionHeight
		{
			get
			{
				return this._captionHeight;
			}
			private set
			{
				if (value != this._captionHeight)
				{
					this._captionHeight = value;
					this._NotifyPropertyChanged("WindowCaptionHeight");
				}
			}
		}

		public CornerRadius WindowCornerRadius
		{
			get
			{
				return this._windowCornerRadius;
			}
			private set
			{
				if (value != this._windowCornerRadius)
				{
					this._windowCornerRadius = value;
					this._NotifyPropertyChanged("WindowCornerRadius");
				}
			}
		}

		public SolidColorBrush WindowGlassBrush
		{
			get
			{
				return this._glassColorBrush;
			}
			private set
			{
				if (this._glassColorBrush == null || value.Color != this._glassColorBrush.Color)
				{
					this._glassColorBrush = value;
					this._NotifyPropertyChanged("WindowGlassBrush");
				}
			}
		}

		public Color WindowGlassColor
		{
			get
			{
				return this._glassColor;
			}
			private set
			{
				if (value != this._glassColor)
				{
					this._glassColor = value;
					this._NotifyPropertyChanged("WindowGlassColor");
				}
			}
		}

		public Thickness WindowNonClientFrameThickness
		{
			get
			{
				return this._windowNonClientFrameThickness;
			}
			private set
			{
				if (value != this._windowNonClientFrameThickness)
				{
					this._windowNonClientFrameThickness = value;
					this._NotifyPropertyChanged("WindowNonClientFrameThickness");
				}
			}
		}

		public Thickness WindowResizeBorderThickness
		{
			get
			{
				return this._windowResizeBorderThickness;
			}
			private set
			{
				if (value != this._windowResizeBorderThickness)
				{
					this._windowResizeBorderThickness = value;
					this._NotifyPropertyChanged("WindowResizeBorderThickness");
				}
			}
		}

		private SystemParameters2()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._messageHwnd = new MessageWindow(0, WS.DISABLED | WS.BORDER | WS.DLGFRAME | WS.SYSMENU | WS.THICKFRAME | WS.GROUP | WS.TABSTOP | WS.MINIMIZEBOX | WS.MAXIMIZEBOX | WS.CAPTION | WS.SIZEBOX | WS.TILEDWINDOW | WS.OVERLAPPEDWINDOW, WS_EX.None, new Rect(-16000, -16000, 100, 100), "", new WndProc(this._WndProc));
			this._messageHwnd.Dispatcher.ShutdownStarted += new EventHandler((object sender, EventArgs e) => Utility.SafeDispose<MessageWindow>(ref this._messageHwnd));
			this._InitializeIsGlassEnabled();
			this._InitializeGlassColor();
			this._InitializeCaptionHeight();
			this._InitializeWindowNonClientFrameThickness();
			this._InitializeWindowResizeBorderThickness();
			this._InitializeCaptionButtonLocation();
			this._InitializeSmallIconSize();
			this._InitializeHighContrast();
			this._InitializeThemeInfo();
			this._InitializeWindowCornerRadius();
			this._UpdateTable = new Dictionary<WM, List<SystemParameters2._SystemMetricUpdate>>()
			{
				{ WM.THEMECHANGED, new List<SystemParameters2._SystemMetricUpdate>()
				{
					new SystemParameters2._SystemMetricUpdate(this._UpdateThemeInfo),
					new SystemParameters2._SystemMetricUpdate(this._UpdateHighContrast),
					new SystemParameters2._SystemMetricUpdate(this._UpdateWindowCornerRadius),
					new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionButtonLocation)
				} },
				{ WM.WININICHANGE, new List<SystemParameters2._SystemMetricUpdate>()
				{
					new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionHeight),
					new SystemParameters2._SystemMetricUpdate(this._UpdateWindowResizeBorderThickness),
					new SystemParameters2._SystemMetricUpdate(this._UpdateSmallIconSize),
					new SystemParameters2._SystemMetricUpdate(this._UpdateHighContrast),
					new SystemParameters2._SystemMetricUpdate(this._UpdateWindowNonClientFrameThickness),
					new SystemParameters2._SystemMetricUpdate(this._UpdateCaptionButtonLocation)
				} },
				{ WM.DWMNCRENDERINGCHANGED, new List<SystemParameters2._SystemMetricUpdate>()
				{
					new SystemParameters2._SystemMetricUpdate(this._UpdateIsGlassEnabled)
				} },
				{ WM.DWMCOMPOSITIONCHANGED, new List<SystemParameters2._SystemMetricUpdate>()
				{
					new SystemParameters2._SystemMetricUpdate(this._UpdateIsGlassEnabled)
				} },
				{ WM.DWMCOLORIZATIONCOLORCHANGED, new List<SystemParameters2._SystemMetricUpdate>()
				{
					new SystemParameters2._SystemMetricUpdate(this._UpdateGlassColor)
				} }
			};
		}

		private void _InitializeCaptionButtonLocation()
		{
			if (!Utility.IsOSVistaOrNewer || !Standard.NativeMethods.IsThemeActive())
			{
				this._LegacyInitializeCaptionButtonLocation();
				return;
			}
			TITLEBARINFOEX tITLEBARINFOEX = new TITLEBARINFOEX()
			{
				cbSize = Marshal.SizeOf(typeof(TITLEBARINFOEX))
			};
			TITLEBARINFOEX structure = tITLEBARINFOEX;
			IntPtr intPtr = Marshal.AllocHGlobal(structure.cbSize);
			try
			{
				Marshal.StructureToPtr(structure, intPtr, false);
				Standard.NativeMethods.ShowWindow(this._messageHwnd.Handle, SW.SHOWNA);
				Standard.NativeMethods.SendMessage(this._messageHwnd.Handle, WM.GETTITLEBARINFOEX, IntPtr.Zero, intPtr);
				structure = (TITLEBARINFOEX)Marshal.PtrToStructure(intPtr, typeof(TITLEBARINFOEX));
			}
			finally
			{
				Standard.NativeMethods.ShowWindow(this._messageHwnd.Handle, SW.HIDE);
				Utility.SafeFreeHGlobal(ref intPtr);
			}
			RECT rECT = RECT.Union(structure.rgrect_CloseButton, structure.rgrect_MinimizeButton);
			RECT windowRect = Standard.NativeMethods.GetWindowRect(this._messageHwnd.Handle);
			Rect rect = new Rect((double)(rECT.Left - windowRect.Width - windowRect.Left), (double)(rECT.Top - windowRect.Top), (double)rECT.Width, (double)rECT.Height);
			this.WindowCaptionButtonsLocation = DpiHelper.DeviceRectToLogical(rect);
		}

		private void _InitializeCaptionHeight()
		{
			Point point = new Point(0, (double)Standard.NativeMethods.GetSystemMetrics(SM.CYCAPTION));
			this.WindowCaptionHeight = DpiHelper.DevicePixelsToLogical(point).Y;
		}

		private void _InitializeGlassColor()
		{
			bool flag;
			uint num;
			Standard.NativeMethods.DwmGetColorizationColor(out num, out flag);
			num = num | (flag ? -16777216 : 0);
			this.WindowGlassColor = Utility.ColorFromArgbDword(num);
			SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
			solidColorBrush.Freeze();
			this.WindowGlassBrush = solidColorBrush;
		}

		private void _InitializeHighContrast()
		{
			HIGHCONTRAST hIGHCONTRAST = Standard.NativeMethods.SystemParameterInfo_GetHIGHCONTRAST();
			this.HighContrast = (int)(hIGHCONTRAST.dwFlags & HCF.HIGHCONTRASTON) != 0;
		}

		private void _InitializeIsGlassEnabled()
		{
			this.IsGlassEnabled = Standard.NativeMethods.DwmIsCompositionEnabled();
		}

		private void _InitializeSmallIconSize()
		{
			this.SmallIconSize = new Size((double)Standard.NativeMethods.GetSystemMetrics(SM.CXSMICON), (double)Standard.NativeMethods.GetSystemMetrics(SM.CYSMICON));
		}

		private void _InitializeThemeInfo()
		{
			string str;
			string str1;
			string str2;
			if (!Standard.NativeMethods.IsThemeActive())
			{
				this.UxThemeName = "Classic";
				this.UxThemeColor = "";
				return;
			}
			Standard.NativeMethods.GetCurrentThemeName(out str, out str1, out str2);
			this.UxThemeName = Path.GetFileNameWithoutExtension(str);
			this.UxThemeColor = str1;
		}

		private void _InitializeWindowCornerRadius()
		{
			CornerRadius cornerRadiu = new CornerRadius();
			string upperInvariant = this.UxThemeName.ToUpperInvariant();
			if (upperInvariant == "LUNA")
			{
				cornerRadiu = new CornerRadius(6, 6, 0, 0);
			}
			else if (upperInvariant == "AERO")
			{
				cornerRadiu = (!Standard.NativeMethods.DwmIsCompositionEnabled() ? new CornerRadius(6, 6, 0, 0) : new CornerRadius(8));
			}
			else
			{
				if (!(upperInvariant == "CLASSIC") && !(upperInvariant == "ZUNE"))
				{
					!(upperInvariant == "ROYALE");
				}
				cornerRadiu = new CornerRadius(0);
			}
			this.WindowCornerRadius = cornerRadiu;
		}

		private void _InitializeWindowNonClientFrameThickness()
		{
			Size size = new Size((double)Standard.NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)Standard.NativeMethods.GetSystemMetrics(SM.CYFRAME));
			Size logical = DpiHelper.DeviceSizeToLogical(size);
			int systemMetrics = Standard.NativeMethods.GetSystemMetrics(SM.CYCAPTION);
			Point point = DpiHelper.DevicePixelsToLogical(new Point(0, (double)systemMetrics));
			double y = point.Y;
			this.WindowNonClientFrameThickness = new Thickness(logical.Width, logical.Height + y, logical.Width, logical.Height);
		}

		private void _InitializeWindowResizeBorderThickness()
		{
			Size size = new Size((double)Standard.NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)Standard.NativeMethods.GetSystemMetrics(SM.CYFRAME));
			Size logical = DpiHelper.DeviceSizeToLogical(size);
			this.WindowResizeBorderThickness = new Thickness(logical.Width, logical.Height, logical.Width, logical.Height);
		}

		private void _LegacyInitializeCaptionButtonLocation()
		{
			int systemMetrics = Standard.NativeMethods.GetSystemMetrics(SM.CXSIZE);
			int num = Standard.NativeMethods.GetSystemMetrics(SM.CYSIZE);
			int systemMetrics1 = Standard.NativeMethods.GetSystemMetrics(SM.CXFRAME) + Standard.NativeMethods.GetSystemMetrics(SM.CXEDGE);
			int num1 = Standard.NativeMethods.GetSystemMetrics(SM.CYFRAME) + Standard.NativeMethods.GetSystemMetrics(SM.CYEDGE);
			Rect rect = new Rect(0, 0, (double)(systemMetrics * 3), (double)num);
			rect.Offset((double)(-systemMetrics1) - rect.Width, (double)num1);
			this.WindowCaptionButtonsLocation = rect;
		}

		private void _NotifyPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void _UpdateCaptionButtonLocation(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeCaptionButtonLocation();
		}

		private void _UpdateCaptionHeight(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeCaptionHeight();
		}

		private void _UpdateGlassColor(IntPtr wParam, IntPtr lParam)
		{
			bool flag = lParam != IntPtr.Zero;
			this.WindowGlassColor = Utility.ColorFromArgbDword((int)wParam.ToInt64() | (flag ? -16777216 : 0));
			SolidColorBrush solidColorBrush = new SolidColorBrush(this.WindowGlassColor);
			solidColorBrush.Freeze();
			this.WindowGlassBrush = solidColorBrush;
		}

		private void _UpdateHighContrast(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeHighContrast();
		}

		private void _UpdateIsGlassEnabled(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeIsGlassEnabled();
		}

		private void _UpdateSmallIconSize(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeSmallIconSize();
		}

		private void _UpdateThemeInfo(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeThemeInfo();
		}

		private void _UpdateWindowCornerRadius(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowCornerRadius();
		}

		private void _UpdateWindowNonClientFrameThickness(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowNonClientFrameThickness();
		}

		private void _UpdateWindowResizeBorderThickness(IntPtr wParam, IntPtr lParam)
		{
			this._InitializeWindowResizeBorderThickness();
		}

		private IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			List<SystemParameters2._SystemMetricUpdate> _SystemMetricUpdates;
			if (this._UpdateTable != null && this._UpdateTable.TryGetValue(msg, out _SystemMetricUpdates))
			{
				foreach (SystemParameters2._SystemMetricUpdate __SystemMetricUpdate in _SystemMetricUpdates)
				{
					__SystemMetricUpdate(wParam, lParam);
				}
			}
			return Standard.NativeMethods.DefWindowProcW(hwnd, msg, wParam, lParam);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private delegate void _SystemMetricUpdate(IntPtr wParam, IntPtr lParam);
	}
}