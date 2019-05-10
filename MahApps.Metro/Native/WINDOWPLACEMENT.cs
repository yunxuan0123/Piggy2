using System;

namespace MahApps.Metro.Native
{
	[Serializable]
	public struct WINDOWPLACEMENT
	{
		public int length;

		public int flags;

		public int showCmd;

		public POINT minPosition;

		public POINT maxPosition;

		public RECT normalPosition;
	}
}