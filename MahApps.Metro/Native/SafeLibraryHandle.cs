using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace MahApps.Metro.Native
{
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
	internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeLibraryHandle()
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
		}

		protected override bool ReleaseHandle()
		{
			return UnsafeNativeMethods.FreeLibrary(this.handle);
		}
	}
}