using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Standard
{
	internal static class NativeMethods
	{
		public static RECT AdjustWindowRectEx(RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle)
		{
			if (!Standard.NativeMethods.AdjustWindowRectEx_1(ref lpRect, dwStyle, bMenu, dwExStyle))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="AdjustWindowRectEx", ExactSpelling=false, SetLastError=true)]
		private static extern bool AdjustWindowRectEx_1(ref RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle);

		public static void AllowSetForegroundWindow()
		{
			Standard.NativeMethods.AllowSetForegroundWindow(-1);
		}

		public static void AllowSetForegroundWindow(int dwProcessId)
		{
			if (!Standard.NativeMethods.AllowSetForegroundWindow_1(dwProcessId))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="AllowSetForegroundWindow", ExactSpelling=false, SetLastError=true)]
		private static extern bool AllowSetForegroundWindow_1(int dwProcessId);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool ChangeWindowMessageFilter(WM message, MSGFLT dwFlag);

		public static HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, out MSGFLTINFO filterInfo)
		{
			filterInfo = MSGFLTINFO.NONE;
			if (!Utility.IsOSVistaOrNewer)
			{
				return HRESULT.S_FALSE;
			}
			if (!Utility.IsOSWindows7OrNewer)
			{
				if (Standard.NativeMethods.ChangeWindowMessageFilter(message, action))
				{
					return HRESULT.S_OK;
				}
				return (HRESULT)Win32Error.GetLastError();
			}
			CHANGEFILTERSTRUCT cHANGEFILTERSTRUCT = new CHANGEFILTERSTRUCT()
			{
				cbSize = (uint)Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT))
			};
			CHANGEFILTERSTRUCT cHANGEFILTERSTRUCT1 = cHANGEFILTERSTRUCT;
			if (!Standard.NativeMethods.ChangeWindowMessageFilterEx_1(hwnd, message, action, ref cHANGEFILTERSTRUCT1))
			{
				return (HRESULT)Win32Error.GetLastError();
			}
			filterInfo = cHANGEFILTERSTRUCT1.ExtStatus;
			return HRESULT.S_OK;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="ChangeWindowMessageFilterEx", ExactSpelling=false, SetLastError=true)]
		private static extern bool ChangeWindowMessageFilterEx_1(IntPtr hwnd, WM message, MSGFLT action, [In][Out] ref CHANGEFILTERSTRUCT pChangeFilterStruct = default(CHANGEFILTERSTRUCT));

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, RGN fnCombineMode);

		public static string[] CommandLineToArgvW(string cmdLine)
		{
			string[] strArrays;
			IntPtr zero = IntPtr.Zero;
			try
			{
				int num = 0;
				zero = Standard.NativeMethods.CommandLineToArgvW_1(cmdLine, out num);
				if (zero == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string[] stringUni = new string[num];
				for (int i = 0; i < num; i++)
				{
					IntPtr intPtr = Marshal.ReadIntPtr(zero, i * Marshal.SizeOf(typeof(IntPtr)));
					stringUni[i] = Marshal.PtrToStringUni(intPtr);
				}
				strArrays = stringUni;
			}
			finally
			{
				Standard.NativeMethods.LocalFree(zero);
			}
			return strArrays;
		}

		[DllImport("shell32.dll", CharSet=CharSet.Unicode, EntryPoint="CommandLineToArgvW", ExactSpelling=false)]
		private static extern IntPtr CommandLineToArgvW_1(string cmdLine, out int numArgs);

		[DllImport("urlmon.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern HRESULT CopyStgMedium(ref STGMEDIUM pcstgmedSrc, ref STGMEDIUM pstgmedDest);

		public static SafeHBITMAP CreateDIBSection(SafeDC hdc, ref BITMAPINFO bitmapInfo, out IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			SafeHBITMAP safeHBITMAP = null;
			safeHBITMAP = (hdc != null ? Standard.NativeMethods.CreateDIBSection_1(hdc, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset) : Standard.NativeMethods.CreateDIBSection_2(IntPtr.Zero, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset));
			if (safeHBITMAP.IsInvalid)
			{
				HRESULT.ThrowLastError();
			}
			return safeHBITMAP;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="CreateDIBSection", ExactSpelling=false, SetLastError=true)]
		private static extern SafeHBITMAP CreateDIBSection_1(SafeDC hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="CreateDIBSection", ExactSpelling=false, SetLastError=true)]
		private static extern SafeHBITMAP CreateDIBSection_2(IntPtr hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			IntPtr intPtr = Standard.NativeMethods.CreateRectRgn_1(nLeftRect, nTopRect, nRightRect, nBottomRect);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="CreateRectRgn", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr CreateRectRgn_1(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		public static IntPtr CreateRectRgnIndirect(RECT lprc)
		{
			IntPtr intPtr = Standard.NativeMethods.CreateRectRgnIndirect_1(ref lprc);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="CreateRectRgnIndirect", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr CreateRectRgnIndirect_1([In] ref RECT lprc);

		public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
		{
			IntPtr intPtr = Standard.NativeMethods.CreateRoundRectRgn_1(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="CreateRoundRectRgn", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr CreateRoundRectRgn_1(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr CreateSolidBrush(int crColor);

		[DllImport("ole32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern HRESULT CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, out IStream ppstm);

		public static IntPtr CreateWindowEx(WS_EX dwExStyle, string lpClassName, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
		{
			IntPtr intPtr = Standard.NativeMethods.CreateWindowExW(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
			if (IntPtr.Zero == intPtr)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("User32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr CreateWindowExW(WS_EX dwExStyle, string lpClassName, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		[DllImport("User32.dll", CharSet=CharSet.Unicode, ExactSpelling=false)]
		public static extern IntPtr DefWindowProcW(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool DestroyIcon(IntPtr handle);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		public static void DrawMenuBar(IntPtr hWnd)
		{
			if (!Standard.NativeMethods.DrawMenuBar_1(hWnd))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="DrawMenuBar", ExactSpelling=false, SetLastError=true)]
		private static extern bool DrawMenuBar_1(IntPtr hWnd);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool DwmDefWindowProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

		public static bool DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend)
		{
			if (Utility.IsOSVistaOrNewer && Standard.NativeMethods.IsThemeActive() && Standard.NativeMethods.DwmGetColorizationColor_1(out pcrColorization, out pfOpaqueBlend).Succeeded)
			{
				return true;
			}
			pcrColorization = -16777216;
			pfOpaqueBlend = true;
			return false;
		}

		[DllImport("dwmapi.dll", CharSet=CharSet.None, EntryPoint="DwmGetColorizationColor", ExactSpelling=false)]
		private static extern HRESULT DwmGetColorizationColor_1(out uint pcrColorization, out bool pfOpaqueBlend);

		public static DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
		{
			DWM_TIMING_INFO? nullable;
			if (!Utility.IsOSVistaOrNewer)
			{
				nullable = null;
				return nullable;
			}
			DWM_TIMING_INFO dWMTIMINGINFO = new DWM_TIMING_INFO()
			{
				cbSize = Marshal.SizeOf(typeof(DWM_TIMING_INFO))
			};
			DWM_TIMING_INFO dWMTIMINGINFO1 = dWMTIMINGINFO;
			HRESULT hRESULT = Standard.NativeMethods.DwmGetCompositionTimingInfo_1(hwnd, ref dWMTIMINGINFO1);
			if (hRESULT == HRESULT.E_PENDING)
			{
				nullable = null;
				return nullable;
			}
			hRESULT.ThrowIfFailed();
			return new DWM_TIMING_INFO?(dWMTIMINGINFO1);
		}

		[DllImport("dwmapi.dll", CharSet=CharSet.None, EntryPoint="DwmGetCompositionTimingInfo", ExactSpelling=false)]
		private static extern HRESULT DwmGetCompositionTimingInfo_1(IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmInvalidateIconicBitmaps(IntPtr hwnd);

		public static bool DwmIsCompositionEnabled()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return false;
			}
			return Standard.NativeMethods.DwmIsCompositionEnabled_1();
		}

		[DllImport("dwmapi.dll", CharSet=CharSet.None, EntryPoint="DwmIsCompositionEnabled", ExactSpelling=false, PreserveSig=false)]
		private static extern bool DwmIsCompositionEnabled_1();

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, RefPOINT pptClient, DWM_SIT dwm_SIT_0);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, DWM_SIT dwm_SIT_0);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);

		public static void DwmSetWindowAttributeDisallowPeek(IntPtr hwnd, bool disallowPeek)
		{
			int num = (disallowPeek ? 1 : 0);
			Standard.NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA.DISALLOW_PEEK, ref num, 4);
		}

		public static void DwmSetWindowAttributeFlip3DPolicy(IntPtr hwnd, DWMFLIP3D flip3dPolicy)
		{
			int num = (int)flip3dPolicy;
			Standard.NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA.FLIP3D_POLICY, ref num, 4);
		}

		public static MF EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable)
		{
			return (MF)Standard.NativeMethods.EnableMenuItem_1(hMenu, uIDEnableItem, uEnable);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="EnableMenuItem", ExactSpelling=false)]
		private static extern int EnableMenuItem_1(IntPtr hMenu, SC uIDEnableItem, MF uEnable);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static extern bool FindClose(IntPtr handle);

		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		public static extern Standard.SafeFindHandle FindFirstFileW(string lpFileName, [In][Out] WIN32_FIND_DATAW lpFindFileData);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool FindNextFileW(Standard.SafeFindHandle hndFindFile, [In][Out] WIN32_FIND_DATAW lpFindFileData);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdipCreateHBITMAPFromBitmap(IntPtr bitmap, out IntPtr hbmReturn, int background);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdipCreateHICONFromBitmap(IntPtr bitmap, out IntPtr hbmReturn);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdipDisposeImage(IntPtr image);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdipImageForceValidation(IntPtr image);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdiplusShutdown(IntPtr token);

		[DllImport("gdiplus.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Status GdiplusStartup(out IntPtr token, StartupInput input, out StartupOutput output);

		public static RECT GetClientRect(IntPtr hwnd)
		{
			RECT rECT;
			if (!Standard.NativeMethods.GetClientRect_1(hwnd, out rECT))
			{
				HRESULT.ThrowLastError();
			}
			return rECT;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetClientRect", ExactSpelling=false, SetLastError=true)]
		private static extern bool GetClientRect_1(IntPtr hwnd, out RECT lpRect);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern HRESULT GetCurrentProcessExplicitAppUserModelID(out string AppID);

		public static void GetCurrentThemeName(out string themeFileName, out string color, out string size)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder1 = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			HRESULT currentThemeName1 = Standard.NativeMethods.GetCurrentThemeName_1(stringBuilder, stringBuilder.Capacity, stringBuilder1, stringBuilder1.Capacity, stringBuilder2, stringBuilder2.Capacity);
			currentThemeName1.ThrowIfFailed();
			themeFileName = stringBuilder.ToString();
			color = stringBuilder1.ToString();
			size = stringBuilder2.ToString();
		}

		[DllImport("uxtheme.dll", CharSet=CharSet.Unicode, EntryPoint="GetCurrentThemeName", ExactSpelling=false)]
		private static extern HRESULT GetCurrentThemeName_1(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		public static POINT GetCursorPos()
		{
			POINT pOINT;
			if (!Standard.NativeMethods.GetCursorPos_1(out pOINT))
			{
				HRESULT.ThrowLastError();
			}
			return pOINT;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetCursorPos", ExactSpelling=false, SetLastError=true)]
		private static extern bool GetCursorPos_1(out POINT lpPoint);

		[Obsolete("Use SafeDC.GetDC instead.", true)]
		public static void GetDC()
		{
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int GetDeviceCaps(SafeDC hdc, DeviceCap nIndex);

		public static string GetModuleFileName(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			while (true)
			{
				int moduleFileName1 = Standard.NativeMethods.GetModuleFileName_1(hModule, stringBuilder, stringBuilder.Capacity);
				if (moduleFileName1 == 0)
				{
					HRESULT.ThrowLastError();
				}
				if (moduleFileName1 != stringBuilder.Capacity)
				{
					break;
				}
				stringBuilder.EnsureCapacity(stringBuilder.Capacity * 2);
			}
			return stringBuilder.ToString();
		}

		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, EntryPoint="GetModuleFileName", ExactSpelling=false, SetLastError=true)]
		private static extern int GetModuleFileName_1(IntPtr hModule, StringBuilder lpFilename, int nSize);

		public static IntPtr GetModuleHandle(string lpModuleName)
		{
			IntPtr moduleHandleW = Standard.NativeMethods.GetModuleHandleW(lpModuleName);
			if (moduleHandleW == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return moduleHandleW;
		}

		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr GetModuleHandleW(string lpModuleName);

		public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
		{
			MONITORINFO mONITORINFO = new MONITORINFO();
			if (!Standard.NativeMethods.GetMonitorInfo_1(hMonitor, mONITORINFO))
			{
				throw new Win32Exception();
			}
			return mONITORINFO;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetMonitorInfo", ExactSpelling=false, SetLastError=true)]
		private static extern bool GetMonitorInfo_1(IntPtr hMonitor, [In][Out] MONITORINFO lpmi);

		public static MONITORINFO GetMonitorInfoW(IntPtr hMonitor)
		{
			MONITORINFO mONITORINFO = new MONITORINFO();
			if (!Standard.NativeMethods.GetMonitorInfoW_1(hMonitor, mONITORINFO))
			{
				throw new Win32Exception();
			}
			return mONITORINFO;
		}

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="GetMonitorInfoW", ExactSpelling=true, SetLastError=true)]
		private static extern bool GetMonitorInfoW_1([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

		public static IntPtr GetStockObject(StockObject fnObject)
		{
			return Standard.NativeMethods.GetStockObject_1(fnObject);
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="GetStockObject", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr GetStockObject_1(StockObject fnObject);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int GetSystemMetrics(SM nIndex);

		public static WINDOWINFO GetWindowInfo(IntPtr hWnd)
		{
			WINDOWINFO wINDOWINFO = new WINDOWINFO()
			{
				cbSize = Marshal.SizeOf(typeof(WINDOWINFO))
			};
			WINDOWINFO wINDOWINFO1 = wINDOWINFO;
			if (!Standard.NativeMethods.GetWindowInfo_1(hWnd, ref wINDOWINFO1))
			{
				HRESULT.ThrowLastError();
			}
			return wINDOWINFO1;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetWindowInfo", ExactSpelling=false, SetLastError=true)]
		private static extern bool GetWindowInfo_1(IntPtr hWnd, ref WINDOWINFO pwi);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

		public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
		{
			IntPtr zero = IntPtr.Zero;
			zero = (8 != IntPtr.Size ? new IntPtr(Standard.NativeMethods.GetWindowLong(hwnd, nIndex)) : Standard.NativeMethods.GetWindowLongPtr_1(hwnd, nIndex));
			if (IntPtr.Zero == zero)
			{
				throw new Win32Exception();
			}
			return zero;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetWindowLongPtr", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr GetWindowLongPtr_1(IntPtr hWnd, GWL nIndex);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

		public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT wINDOWPLACEMENT = new WINDOWPLACEMENT();
			if (!Standard.NativeMethods.GetWindowPlacement(hwnd, wINDOWPLACEMENT))
			{
				throw new Win32Exception();
			}
			return wINDOWPLACEMENT;
		}

		public static RECT GetWindowRect(IntPtr hwnd)
		{
			RECT rECT;
			if (!Standard.NativeMethods.GetWindowRect_1(hwnd, out rECT))
			{
				HRESULT.ThrowLastError();
			}
			return rECT;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="GetWindowRect", ExactSpelling=false, SetLastError=true)]
		private static extern bool GetWindowRect_1(IntPtr hWnd, out RECT lpRect);

		[DllImport("uxtheme.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool IsThemeActive();

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool IsWindow(IntPtr hwnd);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool IsZoomed(IntPtr hwnd);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr LocalFree(IntPtr hMem);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		public static void PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!Standard.NativeMethods.PostMessage_1(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="PostMessage", ExactSpelling=false, SetLastError=true)]
		private static extern bool PostMessage_1(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		public static short RegisterClassEx(ref WNDCLASSEX lpwcx)
		{
			short num = Standard.NativeMethods.RegisterClassExW(ref lpwcx);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern short RegisterClassExW([In] ref WNDCLASSEX lpwcx);

		public static uint RegisterClipboardFormat(string formatName)
		{
			uint num = Standard.NativeMethods.RegisterClipboardFormatW(formatName);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		[DllImport("User32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		private static extern uint RegisterClipboardFormatW(string lpszFormatName);

		public static WM RegisterWindowMessage(string lpString)
		{
			uint num = Standard.NativeMethods.RegisterWindowMessage_1(lpString);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return (WM)num;
		}

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="RegisterWindowMessage", ExactSpelling=false, SetLastError=true)]
		private static extern uint RegisterWindowMessage_1(string lpString);

		[DllImport("ole32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

		public static void RemoveMenu(IntPtr hMenu, SC uPosition, MF uFlags)
		{
			if (!Standard.NativeMethods.RemoveMenu_1(hMenu, (uint)uPosition, (uint)uFlags))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="RemoveMenu", ExactSpelling=false, SetLastError=true)]
		private static extern bool RemoveMenu_1(IntPtr hMenu, uint uPosition, uint uFlags);

		public static IntPtr SelectObject(SafeDC hdc, IntPtr hgdiobj)
		{
			IntPtr intPtr = Standard.NativeMethods.SelectObject_1(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		public static IntPtr SelectObject(SafeDC hdc, SafeHBITMAP hgdiobj)
		{
			IntPtr intPtr = Standard.NativeMethods.SelectObject_2(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="SelectObject", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SelectObject_1(SafeDC hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll", CharSet=CharSet.None, EntryPoint="SelectObject", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SelectObject_2(SafeDC hdc, SafeHBITMAP hgdiobj);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		public static IntPtr SetActiveWindow(IntPtr hwnd)
		{
			Verify.IsNotDefault<IntPtr>(hwnd, "hwnd");
			IntPtr intPtr = Standard.NativeMethods.SetActiveWindow_1(hwnd);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="SetActiveWindow", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SetActiveWindow_1(IntPtr hWnd);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern int SetClassLong(IntPtr hWnd, GCLP nIndex, int dwNewLong);

		public static IntPtr SetClassLongPtr(IntPtr hwnd, GCLP nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return Standard.NativeMethods.SetClassLongPtr_1(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(Standard.NativeMethods.SetClassLong(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="SetClassLongPtr", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SetClassLongPtr_1(IntPtr hWnd, GCLP nIndex, IntPtr dwNewLong);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void SetCurrentProcessExplicitAppUserModelID(string AppID);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern ErrorModes SetErrorMode(ErrorModes newMode);

		public static void SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize)
		{
			if (!Standard.NativeMethods.SetProcessWorkingSetSize_1(hProcess, new IntPtr(dwMinimumWorkingSetSize), new IntPtr(dwMaximumWorkingSetSize)))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, EntryPoint="SetProcessWorkingSetSize", ExactSpelling=false, SetLastError=true)]
		private static extern bool SetProcessWorkingSetSize_1(IntPtr hProcess, IntPtr dwMinimiumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

		public static void SetProp(IntPtr hwnd, string lpString, IntPtr hData)
		{
			if (!Standard.NativeMethods.SetProp_1(hwnd, lpString, hData))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="SetProp", ExactSpelling=false, SetLastError=true)]
		private static extern bool SetProp_1(IntPtr hWnd, string lpString, IntPtr hData);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

		public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return Standard.NativeMethods.SetWindowLongPtr_1(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(Standard.NativeMethods.SetWindowLong(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="SetWindowLongPtr", ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SetWindowLongPtr_1(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

		public static void SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
		{
			if (!Standard.NativeMethods.SetWindowPos_1(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="SetWindowPos", ExactSpelling=false, SetLastError=true)]
		private static extern bool SetWindowPos_1(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

		public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
		{
			if (Standard.NativeMethods.SetWindowRgn_1(hWnd, hRgn, bRedraw) == 0)
			{
				throw new Win32Exception();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="SetWindowRgn", ExactSpelling=false, SetLastError=true)]
		private static extern int SetWindowRgn_1(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

		[DllImport("uxtheme.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

		public static void SHAddToRecentDocs(string path)
		{
			Standard.NativeMethods.SHAddToRecentDocs_2(SHARD.PATHW, path);
		}

		public static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			Standard.NativeMethods.SHAddToRecentDocs_3(SHARD.LINK, shellLink);
		}

		public static void SHAddToRecentDocs(SHARDAPPIDINFO info)
		{
			Standard.NativeMethods.SHAddToRecentDocs_1(SHARD.APPIDINFO, info);
		}

		public static void SHAddToRecentDocs(SHARDAPPIDINFOIDLIST infodIdList)
		{
			Standard.NativeMethods.SHAddToRecentDocs_1(SHARD.APPIDINFOIDLIST, infodIdList);
		}

		[DllImport("shell32.dll", CharSet=CharSet.None, EntryPoint="SHAddToRecentDocs", ExactSpelling=false, PreserveSig=false)]
		private static extern void SHAddToRecentDocs_1(SHARD uFlags, object pv);

		[DllImport("shell32.dll", CharSet=CharSet.None, EntryPoint="SHAddToRecentDocs", ExactSpelling=false)]
		private static extern void SHAddToRecentDocs_2(SHARD uFlags, string pv);

		[DllImport("shell32.dll", CharSet=CharSet.None, EntryPoint="SHAddToRecentDocs", ExactSpelling=false)]
		private static extern void SHAddToRecentDocs_3(SHARD uFlags, IShellLinkW pv);

		[DllImport("shell32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.None, ExactSpelling=false)]
		public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern HRESULT SHCreateItemFromParsingName(string pszPath, IBindCtx pbc, [In] ref Guid riid, out object ppv);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool Shell_NotifyIcon(NIM dwMessage, [In] NOTIFYICONDATA lpdata);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern Win32Error SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

		[DllImport("shell32.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid, out object ppv);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

		public static HIGHCONTRAST SystemParameterInfo_GetHIGHCONTRAST()
		{
			HIGHCONTRAST hIGHCONTRAST = new HIGHCONTRAST()
			{
				cbSize = Marshal.SizeOf(typeof(HIGHCONTRAST))
			};
			HIGHCONTRAST hIGHCONTRAST1 = hIGHCONTRAST;
			if (!Standard.NativeMethods.SystemParametersInfoW_2(SPI.GETHIGHCONTRAST, hIGHCONTRAST1.cbSize, ref hIGHCONTRAST1, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return hIGHCONTRAST1;
		}

		public static NONCLIENTMETRICS SystemParameterInfo_GetNONCLIENTMETRICS()
		{
			NONCLIENTMETRICS nONCLIENTMETRIC = (Utility.IsOSVistaOrNewer ? NONCLIENTMETRICS.VistaMetricsStruct : NONCLIENTMETRICS.XPMetricsStruct);
			if (!Standard.NativeMethods.SystemParametersInfoW_1(SPI.GETNONCLIENTMETRICS, nONCLIENTMETRIC.cbSize, ref nONCLIENTMETRIC, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return nONCLIENTMETRIC;
		}

		public static void SystemParametersInfo(SPI uiAction, int uiParam, string pvParam, SPIF fWinIni)
		{
			if (!Standard.NativeMethods.SystemParametersInfoW(uiAction, uiParam, pvParam, fWinIni))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool SystemParametersInfoW(SPI uiAction, int uiParam, string pvParam, SPIF fWinIni);

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="SystemParametersInfoW", ExactSpelling=false, SetLastError=true)]
		private static extern bool SystemParametersInfoW_1(SPI uiAction, int uiParam, [In][Out] ref NONCLIENTMETRICS pvParam, SPIF fWinIni);

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="SystemParametersInfoW", ExactSpelling=false, SetLastError=true)]
		private static extern bool SystemParametersInfoW_2(SPI uiAction, int uiParam, [In][Out] ref HIGHCONTRAST pvParam, SPIF fWinIni);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		public static void UnregisterClass(short atom, IntPtr hinstance)
		{
			if (!Standard.NativeMethods.UnregisterClass_1(new IntPtr(atom), hinstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		public static void UnregisterClass(string lpClassName, IntPtr hInstance)
		{
			if (!Standard.NativeMethods.UnregisterClass_2(lpClassName, hInstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="UnregisterClass", ExactSpelling=false, SetLastError=true)]
		private static extern bool UnregisterClass_1(IntPtr lpClassName, IntPtr hInstance);

		[DllImport("User32.dll", CharSet=CharSet.Unicode, EntryPoint="UnregisterClass", ExactSpelling=false, SetLastError=true)]
		private static extern bool UnregisterClass_2(string lpClassName, IntPtr hInstance);

		public static void UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, ref POINT pptDst, ref SIZE psize, SafeDC hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!Standard.NativeMethods.UpdateLayeredWindow_1(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		public static void UpdateLayeredWindow(IntPtr hwnd, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!Standard.NativeMethods.UpdateLayeredWindow_2(hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="UpdateLayeredWindow", ExactSpelling=false, SetLastError=true)]
		private static extern bool UpdateLayeredWindow_1(IntPtr hwnd, SafeDC hdcDst, [In] ref POINT pptDst, [In] ref SIZE psize, SafeDC hdcSrc, [In] ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		[DllImport("User32.dll", CharSet=CharSet.None, EntryPoint="UpdateLayeredWindow", ExactSpelling=false, SetLastError=true)]
		private static extern bool UpdateLayeredWindow_2(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);
	}
}