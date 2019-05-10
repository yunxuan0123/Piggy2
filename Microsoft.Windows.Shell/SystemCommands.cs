using Standard;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Microsoft.Windows.Shell
{
	internal static class SystemCommands
	{
		public static RoutedCommand CloseWindowCommand
		{
			get;
			private set;
		}

		public static RoutedCommand MaximizeWindowCommand
		{
			get;
			private set;
		}

		public static RoutedCommand MinimizeWindowCommand
		{
			get;
			private set;
		}

		public static RoutedCommand RestoreWindowCommand
		{
			get;
			private set;
		}

		public static RoutedCommand ShowSystemMenuCommand
		{
			get;
			private set;
		}

		static SystemCommands()
		{
			Class6.yDnXvgqzyB5jw();
			Microsoft.Windows.Shell.SystemCommands.CloseWindowCommand = new RoutedCommand("CloseWindow", typeof(Microsoft.Windows.Shell.SystemCommands));
			Microsoft.Windows.Shell.SystemCommands.MaximizeWindowCommand = new RoutedCommand("MaximizeWindow", typeof(Microsoft.Windows.Shell.SystemCommands));
			Microsoft.Windows.Shell.SystemCommands.MinimizeWindowCommand = new RoutedCommand("MinimizeWindow", typeof(Microsoft.Windows.Shell.SystemCommands));
			Microsoft.Windows.Shell.SystemCommands.RestoreWindowCommand = new RoutedCommand("RestoreWindow", typeof(Microsoft.Windows.Shell.SystemCommands));
			Microsoft.Windows.Shell.SystemCommands.ShowSystemMenuCommand = new RoutedCommand("ShowSystemMenu", typeof(Microsoft.Windows.Shell.SystemCommands));
		}

		private static void _PostSystemCommand(Window window, SC command)
		{
			IntPtr handle = (new WindowInteropHelper(window)).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int)command), IntPtr.Zero);
		}

		public static void CloseWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			Microsoft.Windows.Shell.SystemCommands._PostSystemCommand(window, SC.CLOSE);
		}

		public static void MaximizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			Microsoft.Windows.Shell.SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
		}

		public static void MinimizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			Microsoft.Windows.Shell.SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
		}

		public static void RestoreWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			Microsoft.Windows.Shell.SystemCommands._PostSystemCommand(window, SC.RESTORE);
		}

		public static void ShowSystemMenu(Window window, Point screenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			Microsoft.Windows.Shell.SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation));
		}

		internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			IntPtr handle = (new WindowInteropHelper(window)).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			IntPtr systemMenu = NativeMethods.GetSystemMenu(handle, false);
			uint num = NativeMethods.TrackPopupMenuEx(systemMenu, 256, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
			if (num != 0)
			{
				NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long)num), IntPtr.Zero);
			}
		}
	}
}