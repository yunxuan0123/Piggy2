using System;

namespace Newtonsoft.Json
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		private bool _allowNullItems;

		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		public JsonArrayAttribute()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonArrayAttribute(bool allowNullItems)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._allowNullItems = allowNullItems;
		}

		public JsonArrayAttribute(string id)
		{
			Class6.yDnXvgqzyB5jw();
			base(id);
		}
	}
}