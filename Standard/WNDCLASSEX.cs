using System;

namespace Standard
{
	internal struct WNDCLASSEX
	{
		public int cbSize;

		public CS style;

		public WndProc lpfnWndProc;

		public int cbClsExtra;

		public int cbWndExtra;

		public IntPtr hInstance;

		public IntPtr hIcon;

		public IntPtr hCursor;

		public IntPtr hbrBackground;

		public string lpszMenuName;

		public string lpszClassName;

		public IntPtr hIconSm;
	}
}