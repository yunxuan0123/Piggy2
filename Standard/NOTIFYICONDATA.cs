using System;

namespace Standard
{
	internal class NOTIFYICONDATA
	{
		public int cbSize;

		public IntPtr hWnd;

		public int uID;

		public NIF uFlags;

		public int uCallbackMessage;

		public IntPtr hIcon;

		public char[] szTip;

		public uint dwState;

		public uint dwStateMask;

		public char[] szInfo;

		public uint uVersion;

		public char[] szInfoTitle;

		public uint dwInfoFlags;

		public Guid guidItem;

		private IntPtr hBalloonIcon;

		public NOTIFYICONDATA()
		{
			Class6.yDnXvgqzyB5jw();
			this.szTip = new char[128];
			this.szInfo = new char[256];
			this.szInfoTitle = new char[64];
			base();
		}
	}
}