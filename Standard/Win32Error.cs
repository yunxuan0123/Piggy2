using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Win32Error
	{
		[FieldOffset(0)]
		private readonly int _value;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_SUCCESS;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_INVALID_FUNCTION;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_FILE_NOT_FOUND;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_PATH_NOT_FOUND;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_TOO_MANY_OPEN_FILES;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_ACCESS_DENIED;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_INVALID_HANDLE;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_OUTOFMEMORY;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_NO_MORE_FILES;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_SHARING_VIOLATION;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_INVALID_PARAMETER;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_INSUFFICIENT_BUFFER;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_NESTING_NOT_ALLOWED;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_KEY_DELETED;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_NOT_FOUND;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_NO_MATCH;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_BAD_DEVICE;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_CANCELLED;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_CANNOT_FIND_WND_CLASS;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_CLASS_ALREADY_EXISTS;

		[FieldOffset(-1)]
		public readonly static Win32Error ERROR_INVALID_DATATYPE;

		static Win32Error()
		{
			Class6.yDnXvgqzyB5jw();
			Win32Error.ERROR_SUCCESS = new Win32Error(0);
			Win32Error.ERROR_INVALID_FUNCTION = new Win32Error(1);
			Win32Error.ERROR_FILE_NOT_FOUND = new Win32Error(2);
			Win32Error.ERROR_PATH_NOT_FOUND = new Win32Error(3);
			Win32Error.ERROR_TOO_MANY_OPEN_FILES = new Win32Error(4);
			Win32Error.ERROR_ACCESS_DENIED = new Win32Error(5);
			Win32Error.ERROR_INVALID_HANDLE = new Win32Error(6);
			Win32Error.ERROR_OUTOFMEMORY = new Win32Error(14);
			Win32Error.ERROR_NO_MORE_FILES = new Win32Error(18);
			Win32Error.ERROR_SHARING_VIOLATION = new Win32Error(32);
			Win32Error.ERROR_INVALID_PARAMETER = new Win32Error(87);
			Win32Error.ERROR_INSUFFICIENT_BUFFER = new Win32Error(122);
			Win32Error.ERROR_NESTING_NOT_ALLOWED = new Win32Error(215);
			Win32Error.ERROR_KEY_DELETED = new Win32Error(1018);
			Win32Error.ERROR_NOT_FOUND = new Win32Error(1168);
			Win32Error.ERROR_NO_MATCH = new Win32Error(1169);
			Win32Error.ERROR_BAD_DEVICE = new Win32Error(1200);
			Win32Error.ERROR_CANCELLED = new Win32Error(1223);
			Win32Error.ERROR_CANNOT_FIND_WND_CLASS = new Win32Error(1407);
			Win32Error.ERROR_CLASS_ALREADY_EXISTS = new Win32Error(1410);
			Win32Error.ERROR_INVALID_DATATYPE = new Win32Error(1804);
		}

		public Win32Error(int i)
		{
			Class6.yDnXvgqzyB5jw();
			this._value = i;
		}

		public override bool Equals(object obj)
		{
			bool flag;
			try
			{
				flag = ((Win32Error)obj)._value == this._value;
			}
			catch (InvalidCastException invalidCastException)
			{
				flag = false;
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		public static Win32Error GetLastError()
		{
			return new Win32Error(Marshal.GetLastWin32Error());
		}

		public HRESULT method_0()
		{
			return (HRESULT)this;
		}

		public static bool operator ==(Win32Error errLeft, Win32Error errRight)
		{
			return errLeft._value == errRight._value;
		}

		public static explicit operator HRESULT(Win32Error error)
		{
			if (error._value <= 0)
			{
				return new HRESULT((uint)error._value);
			}
			return HRESULT.Make(true, Facility.Win32, error._value & 65535);
		}

		public static bool operator !=(Win32Error errLeft, Win32Error errRight)
		{
			return !(errLeft == errRight);
		}
	}
}