using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IServiceProvider
	{
		object QueryService(ref Guid guidService, ref Guid riid);
	}
}