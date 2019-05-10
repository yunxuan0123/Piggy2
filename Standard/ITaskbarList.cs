using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ITaskbarList
	{
		void ActivateTab(IntPtr hwnd);

		void AddTab(IntPtr hwnd);

		void DeleteTab(IntPtr hwnd);

		void HrInit();

		void SetActiveAlt(IntPtr hwnd);
	}
}