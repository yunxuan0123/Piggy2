using System;

namespace Standard
{
	internal struct WINDOWINFO
	{
		public int cbSize;

		public RECT rcWindow;

		public RECT rcClient;

		public int dwStyle;

		public int dwExStyle;

		public uint dwWindowStatus;

		public uint cxWindowBorders;

		public uint cyWindowBorders;

		public ushort atomWindowType;

		public ushort wCreatorVersion;
	}
}