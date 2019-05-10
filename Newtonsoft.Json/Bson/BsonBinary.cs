using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	internal class BsonBinary : BsonValue
	{
		public BsonBinaryType BinaryType
		{
			get;
			set;
		}

		public BsonBinary(byte[] value, BsonBinaryType binaryType)
		{
			Class6.yDnXvgqzyB5jw();
			base(value, BsonType.Binary);
			this.BinaryType = binaryType;
		}
	}
}