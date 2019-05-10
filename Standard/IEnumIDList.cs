using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumIDList
	{
		void Clone(out IEnumIDList ppenum);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		void Reset();

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT Skip(uint celt);
	}
}