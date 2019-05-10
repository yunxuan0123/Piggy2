using System;

namespace Newtonsoft.Json
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple=false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		private Newtonsoft.Json.MemberSerialization _memberSerialization;

		internal Required? _itemRequired;

		internal NullValueHandling? _itemNullValueHandling;

		public NullValueHandling ItemNullValueHandling
		{
			get
			{
				NullValueHandling? nullable = this._itemNullValueHandling;
				if (!nullable.HasValue)
				{
					return NullValueHandling.Include;
				}
				return nullable.GetValueOrDefault();
			}
			set
			{
				this._itemNullValueHandling = new NullValueHandling?(value);
			}
		}

		public Required ItemRequired
		{
			get
			{
				Required? nullable = this._itemRequired;
				if (!nullable.HasValue)
				{
					return Required.Default;
				}
				return nullable.GetValueOrDefault();
			}
			set
			{
				this._itemRequired = new Required?(value);
			}
		}

		public Newtonsoft.Json.MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		public JsonObjectAttribute()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public JsonObjectAttribute(Newtonsoft.Json.MemberSerialization memberSerialization)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.MemberSerialization = memberSerialization;
		}

		public JsonObjectAttribute(string id)
		{
			Class6.yDnXvgqzyB5jw();
			base(id);
		}
	}
}