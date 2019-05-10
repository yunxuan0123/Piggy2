using System;

namespace Newtonsoft.Json.Linq
{
	public class JsonLoadSettings
	{
		private Newtonsoft.Json.Linq.CommentHandling _commentHandling;

		private Newtonsoft.Json.Linq.LineInfoHandling _lineInfoHandling;

		private Newtonsoft.Json.Linq.DuplicatePropertyNameHandling _duplicatePropertyNameHandling;

		public Newtonsoft.Json.Linq.CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.Linq.CommentHandling.Ignore || value > Newtonsoft.Json.Linq.CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		public Newtonsoft.Json.Linq.DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.Linq.DuplicatePropertyNameHandling.Ignore || value > Newtonsoft.Json.Linq.DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		public Newtonsoft.Json.Linq.LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < Newtonsoft.Json.Linq.LineInfoHandling.Ignore || value > Newtonsoft.Json.Linq.LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		public JsonLoadSettings()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._lineInfoHandling = Newtonsoft.Json.Linq.LineInfoHandling.Load;
			this._commentHandling = Newtonsoft.Json.Linq.CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = Newtonsoft.Json.Linq.DuplicatePropertyNameHandling.Replace;
		}
	}
}