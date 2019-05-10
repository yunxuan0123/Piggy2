using System;

namespace Standard
{
	internal struct SHDRAGIMAGE
	{
		public SIZE sizeDragImage;

		public POINT ptOffset;

		public IntPtr hbmpDragImage;

		public int crColorKey;
	}
}