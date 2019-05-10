using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Standard
{
	internal sealed class SafeHBITMAP : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeHBITMAP()
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return Standard.NativeMethods.DeleteObject(this.handle);
		}
	}
}