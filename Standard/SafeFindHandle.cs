using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Standard
{
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		[SecurityCritical]
		private SafeFindHandle()
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
		}

		protected override bool ReleaseHandle()
		{
			return Standard.NativeMethods.FindClose(this.handle);
		}
	}
}