using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	internal sealed class ComStream : Stream
	{
		private IStream _source;

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				System.Runtime.InteropServices.ComTypes.STATSTG sTATSTG;
				this._Validate();
				this._source.Stat(out sTATSTG, 1);
				return sTATSTG.cbSize;
			}
		}

		public override long Position
		{
			get
			{
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		public ComStream(ref IStream stream)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			Verify.IsNotNull<IStream>(stream, "stream");
			this._source = stream;
			stream = null;
		}

		private void _Validate()
		{
			if (this._source == null)
			{
				throw new ObjectDisposedException("this");
			}
		}

		public override void Close()
		{
			if (this._source != null)
			{
				Utility.SafeRelease<IStream>(ref this._source);
			}
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num;
			this._Validate();
			IntPtr zero = IntPtr.Zero;
			try
			{
				zero = Marshal.AllocHGlobal(4);
				byte[] numArray = new byte[count];
				this._source.Read(numArray, count, zero);
				Array.Copy(numArray, 0, buffer, offset, Marshal.ReadInt32(zero));
				num = Marshal.ReadInt32(zero);
			}
			finally
			{
				Utility.SafeFreeHGlobal(ref zero);
			}
			return num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long num;
			this._Validate();
			IntPtr zero = IntPtr.Zero;
			try
			{
				zero = Marshal.AllocHGlobal(8);
				this._source.Seek(offset, (int)origin, zero);
				num = Marshal.ReadInt64(zero);
			}
			finally
			{
				Utility.SafeFreeHGlobal(ref zero);
			}
			return num;
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}