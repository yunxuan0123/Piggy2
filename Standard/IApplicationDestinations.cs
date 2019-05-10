using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IApplicationDestinations
	{
		void RemoveAllDestinations();

		void RemoveDestination(object punk);

		void SetAppID([In] string pszAppID);
	}
}