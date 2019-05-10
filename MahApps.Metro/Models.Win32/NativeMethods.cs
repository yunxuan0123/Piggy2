using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MahApps.Metro.Models.Win32
{
	internal static class NativeMethods
	{
		public static WS GetWindowLong(this IntPtr hWnd)
		{
			return (WS)MahApps.Metro.Models.Win32.NativeMethods.GetWindowLongA(hWnd, -16);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern int GetWindowLongA(IntPtr hWnd, int nIndex);

		public static WSEX GetWindowLongEx(this IntPtr hWnd)
		{
			return (WSEX)MahApps.Metro.Models.Win32.NativeMethods.GetWindowLongA(hWnd, -20);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool PostMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public static WS SetWindowLong(this IntPtr hWnd, WS dwNewLong)
		{
			return (WS)MahApps.Metro.Models.Win32.NativeMethods.SetWindowLong(hWnd, -16, (int)dwNewLong);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		public static WSEX SetWindowLongEx(this IntPtr hWnd, WSEX dwNewLong)
		{
			return (WSEX)MahApps.Metro.Models.Win32.NativeMethods.SetWindowLong(hWnd, -20, (int)dwNewLong);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP flags);
	}
}