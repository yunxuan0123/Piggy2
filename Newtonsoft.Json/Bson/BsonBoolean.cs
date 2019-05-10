using System;

namespace Newtonsoft.Json.Bson
{
	internal class BsonBoolean : BsonValue
	{
		public readonly static BsonBoolean False;

		public readonly static BsonBoolean True;

		static BsonBoolean()
		{
			Class6.yDnXvgqzyB5jw();
			BsonBoolean.False = new BsonBoolean(false);
			BsonBoolean.True = new BsonBoolean(true);
		}

		private BsonBoolean(bool value)
		{
			Class6.yDnXvgqzyB5jw();
			base(value, BsonType.Boolean);
		}
	}
}