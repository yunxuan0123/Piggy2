using System;

namespace Standard
{
	[Flags]
	internal enum SPIF
	{
		None,
		UPDATEINIFILE,
		SENDWININICHANGE
	}
}