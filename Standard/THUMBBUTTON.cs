using System;

namespace Standard
{
	internal struct THUMBBUTTON
	{
		public const int THBN_CLICKED = 6144;

		public THB dwMask;

		public uint iId;

		public uint iBitmap;

		public IntPtr hIcon;

		public string szTip;

		public THBF dwFlags;
	}
}