using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItem
	{
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		int Compare(IShellItem psi, SICHINT hint);

		SFGAO GetAttributes(SFGAO sfgaoMask);

		string GetDisplayName(SIGDN sigdnName);

		IShellItem GetParent();
	}
}