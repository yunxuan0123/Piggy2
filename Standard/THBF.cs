using System;

namespace Standard
{
	[Flags]
	internal enum THBF : uint
	{
		ENABLED = 0,
		DISABLED = 1,
		DISMISSONCLICK = 2,
		NOBACKGROUND = 4,
		HIDDEN = 8,
		NONINTERACTIVE = 16
	}
}