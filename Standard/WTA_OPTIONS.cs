using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct WTA_OPTIONS
	{
		[FieldOffset(-1)]
		public const uint Size = 8;

		[FieldOffset(0)]
		public WTNCA dwFlags;

		[FieldOffset(4)]
		public WTNCA dwMask;
	}
}