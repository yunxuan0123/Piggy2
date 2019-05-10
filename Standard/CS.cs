using System;

namespace Standard
{
	[Flags]
	internal enum CS : uint
	{
		VREDRAW = 1,
		HREDRAW = 2,
		DBLCLKS = 8,
		OWNDC = 32,
		CLASSDC = 64,
		PARENTDC = 128,
		NOCLOSE = 512,
		SAVEBITS = 2048,
		BYTEALIGNCLIENT = 4096,
		BYTEALIGNWINDOW = 8192,
		GLOBALCLASS = 16384,
		IME = 65536,
		DROPSHADOW = 131072
	}
}