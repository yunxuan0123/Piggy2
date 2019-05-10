using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	internal class BsonEmpty : BsonToken
	{
		public readonly static BsonToken Null;

		public readonly static BsonToken Undefined;

		public override BsonType Type
		{
			get;
		}

		static BsonEmpty()
		{
			Class6.yDnXvgqzyB5jw();
			BsonEmpty.Null = new BsonEmpty(BsonType.Null);
			BsonEmpty.Undefined = new BsonEmpty(BsonType.Undefined);
		}

		private BsonEmpty(BsonType type)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.Type = type;
		}
	}
}