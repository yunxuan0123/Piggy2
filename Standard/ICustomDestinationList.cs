using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ICustomDestinationList
	{
		void AbortList();

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT AddUserTasks(IObjectArray poa);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT AppendCategory(string pszCategory, IObjectArray poa);

		void AppendKnownCategory(KDC category);

		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		void CommitList();

		void DeleteList(string pszAppID);

		object GetRemovedDestinations([In] ref Guid riid);

		void SetAppID([In] string pszAppID);
	}
}