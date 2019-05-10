using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItemArray
	{
		object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

		object EnumItems();

		uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

		uint GetCount();

		IShellItem GetItemAt(uint dwIndex);

		object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

		object GetPropertyStore(int flags, [In] ref Guid riid);
	}
}