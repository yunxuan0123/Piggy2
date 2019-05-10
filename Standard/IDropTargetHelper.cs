using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	[Guid("4657278B-411B-11D2-839A-00C04FD918D0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IDropTargetHelper
	{
		void DragEnter(IntPtr hwndTarget, IDataObject pDataObject, ref POINT ppt, int effect);

		void DragLeave();

		void DragOver(ref POINT ppt, int effect);

		void Drop(IDataObject dataObject, ref POINT ppt, int effect);

		void Show(bool fShow);
	}
}