using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ITaskbarList3 : ITaskbarList, ITaskbarList2
	{
		void ActivateTab(IntPtr hwnd);

		void AddTab(IntPtr hwnd);

		void DeleteTab(IntPtr hwnd);

		void HrInit();

		void MarkFullscreenWindow(IntPtr hwnd, bool fFullscreen);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		void SetActiveAlt(IntPtr hwnd);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, string pszDescription);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, string pszTip);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, THUMBBUTTON[] pButtons);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, object himl);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, THUMBBUTTON[] pButtons);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		HRESULT UnregisterTab(IntPtr hwndTab);
	}
}