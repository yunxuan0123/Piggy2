using System;
using System.Runtime.InteropServices;

namespace Standard
{
	internal class WINDOWPLACEMENT
	{
		public int length;

		public int flags;

		public SW showCmd;

		public POINT ptMinPosition;

		public POINT ptMaxPosition;

		public RECT rcNormalPosition;

		public WINDOWPLACEMENT()
		{
			Class6.yDnXvgqzyB5jw();
			this.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
			base();
		}
	}
}