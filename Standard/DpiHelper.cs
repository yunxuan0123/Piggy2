using System;
using System.Windows;
using System.Windows.Media;

namespace Standard
{
	internal static class DpiHelper
	{
		private static Matrix _transformToDevice;

		private static Matrix _transformToDip;

		static DpiHelper()
		{
			Class6.yDnXvgqzyB5jw();
			using (SafeDC desktop = SafeDC.GetDesktop())
			{
				int deviceCaps = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSX);
				int num = NativeMethods.GetDeviceCaps(desktop, DeviceCap.LOGPIXELSY);
				DpiHelper._transformToDip = Matrix.Identity;
				DpiHelper._transformToDip.Scale(96 / (double)deviceCaps, 96 / (double)num);
				DpiHelper._transformToDevice = Matrix.Identity;
				DpiHelper._transformToDevice.Scale((double)deviceCaps / 96, (double)num / 96);
			}
		}

		public static Point DevicePixelsToLogical(Point devicePoint)
		{
			return DpiHelper._transformToDip.Transform(devicePoint);
		}

		public static Rect DeviceRectToLogical(Rect deviceRectangle)
		{
			Point logical = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top));
			Point point = DpiHelper.DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom));
			return new Rect(logical, point);
		}

		public static Size DeviceSizeToLogical(Size deviceSize)
		{
			Point logical = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height));
			return new Size(logical.X, logical.Y);
		}

		public static Point LogicalPixelsToDevice(Point logicalPoint)
		{
			return DpiHelper._transformToDevice.Transform(logicalPoint);
		}

		public static Rect LogicalRectToDevice(Rect logicalRectangle)
		{
			Point device = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom));
			return new Rect(device, point);
		}

		public static Size LogicalSizeToDevice(Size logicalSize)
		{
			Point device = DpiHelper.LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height));
			Size size = new Size()
			{
				Width = device.X,
				Height = device.Y
			};
			return size;
		}

		public static Thickness LogicalThicknessToDevice(Thickness logicalThickness)
		{
			Point device = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top));
			Point point = DpiHelper.LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom));
			return new Thickness(device.X, device.Y, point.X, point.Y);
		}
	}
}