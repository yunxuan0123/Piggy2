using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[StructLayout(LayoutKind.Explicit)]
	internal class PROPVARIANT : IDisposable
	{
		[FieldOffset(0)]
		private ushort vt;

		[FieldOffset(8)]
		private IntPtr pointerVal;

		[FieldOffset(8)]
		private byte byteVal;

		[FieldOffset(8)]
		private long longVal;

		[FieldOffset(8)]
		private short boolVal;

		public VarEnum VarType
		{
			get
			{
				return (VarEnum)this.vt;
			}
		}

		public PROPVARIANT()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public void Clear()
		{
			PROPVARIANT.NativeMethods.PropVariantClear(this);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			this.Clear();
		}

		~PROPVARIANT()
		{
			this.Dispose(false);
		}

		public string GetValue()
		{
			if (this.vt != 31)
			{
				return null;
			}
			return Marshal.PtrToStringUni(this.pointerVal);
		}

		public void SetValue(bool f)
		{
			object obj;
			this.Clear();
			this.vt = 11;
			if (f)
			{
				obj = -1;
			}
			else
			{
				obj = null;
			}
			this.boolVal = (short)obj;
		}

		public void SetValue(string val)
		{
			this.Clear();
			this.vt = 31;
			this.pointerVal = Marshal.StringToCoTaskMemUni(val);
		}

		private static class NativeMethods
		{
			[DllImport("ole32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
		}
	}
}