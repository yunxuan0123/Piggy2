using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;

namespace MahApps.Metro.Native
{
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		internal const int GWL_STYLE = -16;

		internal const int WS_SYSMENU = 524288;

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr CreateSolidBrush(int crColor);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr DefWindowProc([In] IntPtr hwnd, [In] int msg, [In] IntPtr wParam, [In] IntPtr lParam);

		[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool DeleteObject(IntPtr hObject);

		[DllImport("dwmapi", CharSet=CharSet.None, ExactSpelling=true)]
		internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, [In] ref MARGINS pMarInset);

		[DllImport("dwmapi", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		internal static extern bool DwmIsCompositionEnabled();

		[DllImport("dwmapi", CharSet=CharSet.None, ExactSpelling=true)]
		internal static extern int DwmSetWindowAttribute([In] IntPtr hwnd, [In] int attr, [In] ref int attrValue, [In] int attrSize);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern uint EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		[Obsolete("Use NativeMethods.FindWindow instead.")]
		internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("kernel32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool FreeLibrary([In] IntPtr hModule);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern uint GetClassLong(IntPtr hWnd, int nIndex);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool GetCursorPos(out POINT pt);

		[DllImport("user32", CharSet=CharSet.Auto, ExactSpelling=true)]
		internal static extern int GetDoubleClickTime();

		[DllImport("user32", CharSet=CharSet.Unicode, ExactSpelling=true)]
		internal static extern bool GetMonitorInfoW([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

		internal static Point GetPoint(IntPtr ptr)
		{
			uint num = (Environment.Is64BitProcess ? (uint)ptr.ToInt64() : (uint)ptr.ToInt32());
			int num1 = (short)num;
			int num2 = (short)(num >> 16);
			return new Point((double)num1, (double)num2);
		}

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32", CharSet=CharSet.Auto, ExactSpelling=true)]
		internal static extern bool IsWindow([In] IntPtr hWnd = default(IntPtr));

		[DllImport("kernel32", CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
		internal static extern SafeLibraryHandle LoadLibraryW([In] string lpFileName);

		[DllImport("user32", CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
		internal static extern int LoadStringW([In] SafeLibraryHandle hInstance = default(SafeLibraryHandle), [In] uint uID, [Out] StringBuilder lpBuffer, [In] int nBufferMax);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern IntPtr MonitorFromPoint(POINT pt, MONITORINFO.MonitorOptions dwFlags);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] int flags);

		internal static void PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!UnsafeNativeMethods.PostMessage_1(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("user32", CharSet=CharSet.None, EntryPoint="PostMessage", ExactSpelling=false, SetLastError=true)]
		private static extern bool PostMessage_1([In] IntPtr hWnd = default(IntPtr), [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, Constants.RedrawWindowFlags flags);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, Constants.RedrawWindowFlags flags);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool ReleaseCapture();

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern uint SetClassLong(IntPtr hWnd, int nIndex, uint dwNewLong);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr SetClassLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("shell32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.None, ExactSpelling=false)]
		[Obsolete("Use NativeMethods.SHAppBarMessage instead.")]
		public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern uint TrackPopupMenuEx([In] IntPtr hmenu, [In] uint fuFlags, [In] int x, [In] int y, [In] IntPtr hwnd, [In] IntPtr lptpm = default(IntPtr));

		internal struct Win32Point
		{
			public readonly int X;

			public readonly int Y;
		}
	}
}