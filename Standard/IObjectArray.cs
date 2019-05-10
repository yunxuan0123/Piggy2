using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IObjectArray
	{
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		uint GetCount();
	}
}