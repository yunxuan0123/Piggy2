using Newtonsoft.Json.Utilities;
using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		public byte[] Value
		{
			get;
		}

		public BsonObjectId(byte[] value)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(value, "value");
			if ((int)value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}