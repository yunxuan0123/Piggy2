using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyStore
	{
		void Commit();

		PKEY GetAt(uint iProp);

		uint GetCount();

		void GetValue([In] ref PKEY pkey, [In][Out] PROPVARIANT pv);

		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);
	}
}