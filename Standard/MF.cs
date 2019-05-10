using System;

namespace Standard
{
	[Flags]
	internal enum MF : uint
	{
		BYCOMMAND = 0,
		ENABLED = 0,
		GRAYED = 1,
		DISABLED = 2,
		DOES_NOT_EXIST = 4294967295
	}
}