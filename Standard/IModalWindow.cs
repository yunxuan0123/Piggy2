using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IModalWindow
	{
		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT Show(IntPtr parent);
	}
}