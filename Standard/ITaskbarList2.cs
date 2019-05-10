using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ITaskbarList2 : ITaskbarList
	{
		void ActivateTab(IntPtr hwnd);

		void AddTab(IntPtr hwnd);

		void DeleteTab(IntPtr hwnd);

		void HrInit();

		void MarkFullscreenWindow(IntPtr hwnd, bool fFullscreen);

		void SetActiveAlt(IntPtr hwnd);
	}
}