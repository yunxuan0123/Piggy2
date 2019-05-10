using System;

namespace Standard
{
	internal class RefRECT
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

		public RefRECT(int left, int top, int right, int bottom)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._left = left;
			this._top = top;
			this._right = right;
			this._bottom = bottom;
		}

		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}
	}
}