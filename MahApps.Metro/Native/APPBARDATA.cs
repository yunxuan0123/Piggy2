using System;

namespace MahApps.Metro.Native
{
	[Obsolete("Use Standard.APPBARDATA instead.")]
	internal struct APPBARDATA
	{
		public int cbSize;

		public IntPtr hWnd;

		public int uCallbackMessage;

		public int uEdge;

		public RECT rc;

		public bool lParam;
	}
}