using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class MetroThumb : Thumb
	{
		private TouchDevice _currentDevice;

		public MetroThumb()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private void CaptureCurrentDevice(TouchEventArgs e)
		{
			if (base.CaptureTouch(e.TouchDevice))
			{
				this._currentDevice = e.TouchDevice;
			}
		}

		protected override void OnLostTouchCapture(TouchEventArgs e)
		{
			if (this._currentDevice != null)
			{
				this.CaptureCurrentDevice(e);
			}
		}

		protected override void OnPreviewTouchDown(TouchEventArgs e)
		{
			this.ReleaseCurrentDevice();
			this.CaptureCurrentDevice(e);
		}

		protected override void OnPreviewTouchUp(TouchEventArgs e)
		{
			this.ReleaseCurrentDevice();
		}

		private void ReleaseCurrentDevice()
		{
			if (this._currentDevice != null)
			{
				TouchDevice touchDevice = this._currentDevice;
				this._currentDevice = null;
				base.ReleaseTouchCapture(touchDevice);
			}
		}
	}
}