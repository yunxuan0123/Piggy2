using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
	{
		private IConnectionPoint _cp;

		public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId)
		{
			Class6.yDnXvgqzyB5jw();
			base(true);
			int num;
			Verify.IsNotNull<IConnectionPointContainer>(target, "target");
			Verify.IsNotNull<object>(sink, "sink");
			Verify.IsNotDefault<Guid>(eventId, "eventId");
			this.handle = IntPtr.Zero;
			IConnectionPoint connectionPoint = null;
			try
			{
				target.FindConnectionPoint(ref eventId, out connectionPoint);
				connectionPoint.Advise(sink, out num);
				if (num == 0)
				{
					throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
				}
				this.handle = new IntPtr(num);
				this._cp = connectionPoint;
				connectionPoint = null;
			}
			finally
			{
				Utility.SafeRelease<IConnectionPoint>(ref connectionPoint);
			}
		}

		public void Disconnect()
		{
			this.ReleaseHandle();
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			bool flag;
			try
			{
				if (!this.IsInvalid)
				{
					int num = this.handle.ToInt32();
					this.handle = IntPtr.Zero;
					try
					{
						this._cp.Unadvise(num);
					}
					finally
					{
						Utility.SafeRelease<IConnectionPoint>(ref this._cp);
					}
				}
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}