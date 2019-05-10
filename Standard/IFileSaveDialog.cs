using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileSaveDialog : IModalWindow, IFileDialog
	{
		void AddPlace(IShellItem psi, FDAP fdcp);

		uint Advise(IFileDialogEvents pfde);

		void ApplyProperties(IShellItem psi, object pStore, [In] ref IntPtr hwnd, object pSink);

		void ClearClientData();

		void Close(int hr);

		IShellItem GetCurrentSelection();

		string GetFileName();

		uint GetFileTypeIndex();

		IShellItem GetFolder();

		FOS GetOptions();

		object GetProperties();

		IShellItem GetResult();

		void SetClientGuid([In] ref Guid guid);

		void SetCollectedProperties([In] object pList, [In] int fAppendDefault);

		void SetDefaultExtension(string pszDefaultExtension);

		void SetDefaultFolder(IShellItem psi);

		void SetFileName(string pszName);

		void SetFileNameLabel(string pszLabel);

		void SetFileTypeIndex(uint iFileType);

		void SetFileTypes(uint cFileTypes, [In] ref COMDLG_FILTERSPEC rgFilterSpec);

		void SetFilter(object pFilter);

		void SetFolder(IShellItem psi);

		void SetOkButtonLabel(string pszText);

		void SetOptions(FOS fos);

		void SetProperties([In] object pStore);

		void SetSaveAsItem(IShellItem psi);

		void SetTitle(string pszTitle);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT Show(IntPtr parent);

		void Unadvise(uint dwCookie);
	}
}