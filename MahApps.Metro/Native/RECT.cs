using System;
using System.Windows;

namespace MahApps.Metro.Native
{
	[Serializable]
	public struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public readonly static RECT Empty;

		public int Height
		{
			get
			{
				return this.bottom - this.top;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (this.left >= this.right)
				{
					return true;
				}
				return this.top >= this.bottom;
			}
		}

		public int Width
		{
			get
			{
				return Math.Abs(this.right - this.left);
			}
		}

		static RECT()
		{
			Class6.yDnXvgqzyB5jw();
		}

		public RECT(int left, int top, int right, int bottom)
		{
			Class6.yDnXvgqzyB5jw();
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public RECT(RECT rcSrc)
		{
			Class6.yDnXvgqzyB5jw();
			this.left = rcSrc.left;
			this.top = rcSrc.top;
			this.right = rcSrc.right;
			this.bottom = rcSrc.bottom;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Rect))
			{
				return false;
			}
			return this == (RECT)obj;
		}

		public override int GetHashCode()
		{
			return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
		}

		public static bool operator ==(RECT rect1, RECT rect2)
		{
			if (rect1.left != rect2.left || rect1.top != rect2.top || rect1.right != rect2.right)
			{
				return false;
			}
			return rect1.bottom == rect2.bottom;
		}

		public static bool operator !=(RECT rect1, RECT rect2)
		{
			return !(rect1 == rect2);
		}

		public override string ToString()
		{
			if (this == RECT.Empty)
			{
				return "RECT {Empty}";
			}
			return string.Concat(new object[] { "RECT { left : ", this.left, " / top : ", this.top, " / right : ", this.right, " / bottom : ", this.bottom, " }" });
		}
	}
}