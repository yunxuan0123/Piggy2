using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileOpenDialog : IModalWindow, IFileDialog
	{
		void AddPlace(IShellItem psi, FDAP fdcp);

		uint Advise(IFileDialogEvents pfde);

		void ClearClientData();

		void Close(int hr);

		IShellItem GetCurrentSelection();

		string GetFileName();

		uint GetFileTypeIndex();

		IShellItem GetFolder();

		FOS GetOptions();

		IShellItem GetResult();

		IShellItemArray GetResults();

		IShellItemArray GetSelectedItems();

		void SetClientGuid([In] ref Guid guid);

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

		void SetTitle(string pszTitle);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT Show(IntPtr parent);

		void Unadvise(uint dwCookie);
	}
}