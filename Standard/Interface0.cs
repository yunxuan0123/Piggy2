using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("83E07D0D-0C5F-4163-BF1A-60B274051E40")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface Interface0 : IDragSourceHelper
	{
		void InitializeFromBitmap([In] ref SHDRAGIMAGE pshdi, [In] IDataObject pDataObject);

		void InitializeFromWindow(IntPtr hwnd, [In] ref POINT ppt, [In] IDataObject pDataObject);

		void SetFlags(DSH dwFlags);
	}
}