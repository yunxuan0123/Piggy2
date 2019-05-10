using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IObjectCollection : IObjectArray
	{
		void AddFromArray(IObjectArray poaSource);

		void AddObject(object punk);

		void Clear();

		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		uint GetCount();

		void RemoveObjectAt(uint uiIndex);
	}
}