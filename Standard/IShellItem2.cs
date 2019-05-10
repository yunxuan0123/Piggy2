using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItem2 : IShellItem
	{
		object BindToHandler([In] IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		int Compare(IShellItem psi, SICHINT hint);

		SFGAO GetAttributes(SFGAO sfgaoMask);

		void GetBool(IntPtr key);

		Guid GetCLSID(IntPtr key);

		string GetDisplayName(SIGDN sigdnName);

		System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

		int GetInt32(IntPtr key);

		IShellItem GetParent();

		PROPVARIANT GetProperty(IntPtr key);

		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		object GetPropertyStoreWithCreateObject(GPS flags, object punkCreateObject, [In] ref Guid riid);

		string GetString(IntPtr key);

		uint imethod_0(IntPtr key);

		ulong imethod_1(IntPtr key);

		void Update(IBindCtx pbc);
	}
}