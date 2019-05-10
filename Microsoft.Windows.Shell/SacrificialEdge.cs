using System;

namespace Microsoft.Windows.Shell
{
	[Flags]
	public enum SacrificialEdge
	{
		None = 0,
		Left = 1,
		Top = 2,
		Right = 4,
		Bottom = 8,
		Office = 13
	}
}