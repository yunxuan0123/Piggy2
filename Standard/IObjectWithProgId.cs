using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IObjectWithProgId
	{
		string GetProgID();

		void SetProgID(string string_0);
	}
}