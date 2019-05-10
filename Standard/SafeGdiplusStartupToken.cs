using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Standard
{
	internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeGdiplusStartupToken(IntPtr ptr)
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
			this.handle = ptr;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return Standard.NativeMethods.GdiplusShutdown(this.handle) == Status.Ok;
		}

		public static SafeGdiplusStartupToken Startup()
		{
			IntPtr intPtr;
			StartupOutput startupOutput;
			if (Standard.NativeMethods.GdiplusStartup(out intPtr, new StartupInput(), out startupOutput) != Status.Ok)
			{
				throw new Exception("Unable to initialize GDI+");
			}
			return new SafeGdiplusStartupToken(intPtr);
		}
	}
}