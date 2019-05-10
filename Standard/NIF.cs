using System;

namespace Standard
{
	[Flags]
	internal enum NIF : uint
	{
		MESSAGE = 1,
		ICON = 2,
		TIP = 4,
		STATE = 8,
		INFO = 16,
		GUID = 32,
		XP_MASK = 59,
		REALTIME = 64,
		SHOWTIP = 128,
		VISTA_MASK = 251
	}
}