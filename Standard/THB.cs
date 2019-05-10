using System;

namespace Standard
{
	[Flags]
	internal enum THB : uint
	{
		BITMAP = 1,
		ICON = 2,
		TOOLTIP = 4,
		FLAGS = 8
	}
}