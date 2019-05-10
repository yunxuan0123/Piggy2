using System;

namespace Standard
{
	internal struct RECT
	{
		private int _left;

		private int _top;

		private int _right;

		private int _bottom;

		public int Bottom
		{
			get
			{
				return this._bottom;
			}
			set
			{
				this._bottom = value;
			}
		}

		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		public int Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
			}
		}

		public POINT Position
		{
			get
			{
				POINT pOINT = new POINT()
				{
					x = this._left,
					y = this._top
				};
				return pOINT;
			}
		}

		public int Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		public SIZE Size
		{
			get
			{
				SIZE sIZE = new SIZE()
				{
					cx = this.Width,
					cy = this.Height
				};
				return sIZE;
			}
		}

		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
			}
		}

		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		public override bool Equals(object obj)
		{
			bool flag;
			try
			{
				RECT rECT = (RECT)obj;
				flag = (rECT._bottom != this._bottom || rECT._left != this._left || rECT._right != this._right ? false : rECT._top == this._top);
			}
			catch (InvalidCastException invalidCastException)
			{
				flag = false;
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
		}

		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		public static RECT Union(RECT rect1, RECT rect2)
		{
			RECT rECT = new RECT()
			{
				Left = Math.Min(rect1.Left, rect2.Left),
				Top = Math.Min(rect1.Top, rect2.Top),
				Right = Math.Max(rect1.Right, rect2.Right),
				Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
			};
			return rECT;
		}
	}
}