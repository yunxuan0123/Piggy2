using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Standard
{
	internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
	{
		private IntPtr? _hwnd;

		private bool _created;

		public IntPtr Hwnd
		{
			set
			{
				this._hwnd = new IntPtr?(value);
			}
		}

		private SafeDC()
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
		}

		public static SafeDC CreateCompatibleDC(SafeDC hdc)
		{
			SafeDC safeDC = null;
			try
			{
				IntPtr zero = IntPtr.Zero;
				if (hdc != null)
				{
					zero = hdc.handle;
				}
				safeDC = SafeDC.NativeMethods.CreateCompatibleDC(zero);
				if (safeDC == null)
				{
					HRESULT.ThrowLastError();
				}
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		public static SafeDC CreateDC(string deviceName)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = SafeDC.NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		public static SafeDC GetDC(IntPtr hwnd)
		{
			SafeDC dC = null;
			try
			{
				dC = SafeDC.NativeMethods.GetDC(hwnd);
			}
			finally
			{
				if (dC != null)
				{
					dC.Hwnd = hwnd;
				}
			}
			if (dC.IsInvalid)
			{
				HRESULT.E_FAIL.ThrowIfFailed();
			}
			return dC;
		}

		public static SafeDC GetDesktop()
		{
			return SafeDC.GetDC(IntPtr.Zero);
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (this._created)
			{
				return SafeDC.NativeMethods.DeleteDC(this.handle);
			}
			if (!this._hwnd.HasValue || this._hwnd.Value == IntPtr.Zero)
			{
				return true;
			}
			return SafeDC.NativeMethods.ReleaseDC(this._hwnd.Value, this.handle) == 1;
		}

		public static SafeDC WrapDC(IntPtr hdc)
		{
			return new SafeDC()
			{
				handle = hdc,
				_created = false,
				_hwnd = new IntPtr?(IntPtr.Zero)
			};
		}

		private static class NativeMethods
		{
			[DllImport("gdi32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
			public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

			[DllImport("gdi32.dll", CharSet=CharSet.Unicode, ExactSpelling=false)]
			public static extern SafeDC CreateDC(string lpszDriver, string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

			[DllImport("gdi32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			public static extern bool DeleteDC(IntPtr hdc);

			[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			public static extern SafeDC GetDC(IntPtr hwnd);

			[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		}
	}
}