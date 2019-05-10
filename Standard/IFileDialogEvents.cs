using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileDialogEvents
	{
		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnFileOk(IFileDialog pfd);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnFolderChange(IFileDialog pfd);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnSelectionChange(IFileDialog pfd);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT OnTypeChange(IFileDialog pfd);
	}
}