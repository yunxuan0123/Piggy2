using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	internal class BsonString : BsonValue
	{
		public int ByteCount
		{
			get;
			set;
		}

		public bool IncludeLength
		{
			get;
		}

		public BsonString(object value, bool includeLength)
		{
			Class6.yDnXvgqzyB5jw();
			base(value, BsonType.String);
			this.IncludeLength = includeLength;
		}
	}
}