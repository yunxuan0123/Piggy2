using System;

namespace Standard
{
	internal struct CREATESTRUCT
	{
		public IntPtr lpCreateParams;

		public IntPtr hInstance;

		public IntPtr hMenu;

		public IntPtr hwndParent;

		public int cy;

		public int cx;

		public int y;

		public int x;

		public WS style;

		public string lpszName;

		public string lpszClass;

		public WS_EX dwExStyle;
	}
}