using System;

namespace MahApps.Metro.Native
{
	[Serializable]
	public struct POINT
	{
		private int _x;

		private int _y;

		public int X
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
			}
		}

		public int Y
		{
			get
			{
				return this._y;
			}
			set
			{
				this._y = value;
			}
		}

		public POINT(int x, int y)
		{
			Class6.yDnXvgqzyB5jw();
			this._x = x;
			this._y = y;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is POINT))
			{
				return this.Equals(obj);
			}
			POINT pOINT = (POINT)obj;
			if (pOINT._x != this._x)
			{
				return false;
			}
			return pOINT._y == this._y;
		}

		public override int GetHashCode()
		{
			return this._x.GetHashCode() ^ this._y.GetHashCode();
		}

		public static bool operator ==(POINT a, POINT b)
		{
			if (a._x != b._x)
			{
				return false;
			}
			return a._y == b._y;
		}

		public static bool operator !=(POINT a, POINT b)
		{
			return !(a == b);
		}
	}
}