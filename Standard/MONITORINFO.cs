using System;
using System.Runtime.InteropServices;

namespace Standard
{
	internal class MONITORINFO
	{
		public int cbSize;

		public RECT rcMonitor;

		public RECT rcWork;

		public int dwFlags;

		public MONITORINFO()
		{
			Class6.yDnXvgqzyB5jw();
			this.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
			base();
		}
	}
}