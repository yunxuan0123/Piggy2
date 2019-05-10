using System;

namespace Standard
{
	[Flags]
	internal enum WTNCA : uint
	{
		NODRAWCAPTION = 1,
		NODRAWICON = 2,
		NOSYSMENU = 4,
		NOMIRRORHELP = 8,
		VALIDBITS = 15
	}
}