using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("DE5BF786-477A-11D2-839D-00C04FD918D0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IDragSourceHelper
	{
		void InitializeFromBitmap([In] ref SHDRAGIMAGE pshdi, [In] IDataObject pDataObject);

		void InitializeFromWindow(IntPtr hwnd, [In] ref POINT ppt, [In] IDataObject pDataObject);
	}
}