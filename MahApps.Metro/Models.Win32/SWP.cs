using System;

namespace MahApps.Metro.Models.Win32
{
	[Flags]
	internal enum SWP
	{
		NOSIZE = 1,
		NOMOVE = 2,
		NOZORDER = 4,
		NOREDRAW = 8,
		NOACTIVATE = 16,
		FRAMECHANGED = 32,
		SHOWWINDOW = 64,
		NOOWNERZORDER = 512,
		NOSENDCHANGING = 1024,
		ASYNCWINDOWPOS = 16384
	}
}