using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumObjects
	{
		IEnumObjects Clone();

		void Next(uint celt, [In] ref Guid riid, [Out] object[] rgelt, out uint pceltFetched);

		void Reset();

		void Skip(uint celt);
	}
}