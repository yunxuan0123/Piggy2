using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IApplicationDocumentLists
	{
		object GetList([In] APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);

		void SetAppID(string pszAppID);
	}
}