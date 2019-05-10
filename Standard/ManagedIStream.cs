using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	internal sealed class ManagedIStream : IDisposable, IStream
	{
		private Stream _source;

		public ManagedIStream(Stream source)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			Verify.IsNotNull<Stream>(source, "source");
			this._source = source;
		}

		private void _Validate()
		{
			if (this._source == null)
			{
				throw new ObjectDisposedException("this");
			}
		}

		[Obsolete("The method is not implemented", true)]
		public void Clone(out IStream ppstm)
		{
			ppstm = null;
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Commit(int grfCommitFlags)
		{
			this._Validate();
			this._source.Flush();
		}

		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			long i;
			int num = 0;
			Verify.IsNotNull<IStream>(pstm, "pstm");
			this._Validate();
			byte[] numArray = new byte[4096];
			for (i = 0L; i < cb; i += (long)num)
			{
				num = this._source.Read(numArray, 0, (int)numArray.Length);
				if (num == 0)
				{
					break;
				}
				pstm.Write(numArray, num, IntPtr.Zero);
			}
			if (IntPtr.Zero != pcbRead)
			{
				Marshal.WriteInt64(pcbRead, i);
			}
			if (IntPtr.Zero != pcbWritten)
			{
				Marshal.WriteInt64(pcbWritten, i);
			}
		}

		public void Dispose()
		{
			this._source = null;
		}

		[Obsolete("The method is not implemented", true)]
		public void LockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Read(byte[] pv, int cb, IntPtr pcbRead)
		{
			this._Validate();
			int num = this._source.Read(pv, 0, cb);
			if (IntPtr.Zero != pcbRead)
			{
				Marshal.WriteInt32(pcbRead, num);
			}
		}

		[Obsolete("The method is not implemented", true)]
		public void Revert()
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
		{
			this._Validate();
			long num = this._source.Seek(dlibMove, (SeekOrigin)dwOrigin);
			if (IntPtr.Zero != plibNewPosition)
			{
				Marshal.WriteInt64(plibNewPosition, num);
			}
		}

		public void SetSize(long libNewSize)
		{
			this._Validate();
			this._source.SetLength(libNewSize);
		}

		public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
			this._Validate();
			pstatstg.type = 2;
			pstatstg.cbSize = this._source.Length;
			pstatstg.grfMode = 2;
			pstatstg.grfLocksSupported = 2;
		}

		[Obsolete("The method is not implemented", true)]
		public void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Write(byte[] pv, int cb, IntPtr pcbWritten)
		{
			this._Validate();
			this._source.Write(pv, 0, cb);
			if (IntPtr.Zero != pcbWritten)
			{
				Marshal.WriteInt32(pcbWritten, cb);
			}
		}
	}
}