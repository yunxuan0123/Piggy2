using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectIdConverter : JsonConverter
	{
		public BsonObjectIdConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BsonObjectId);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Bytes)
			{
				throw new JsonSerializationException("Expected Bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			return new BsonObjectId((byte[])reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BsonObjectId bsonObjectId = (BsonObjectId)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			BsonWriter bsonWriter1 = bsonWriter;
			if (bsonWriter != null)
			{
				bsonWriter1.WriteObjectId(bsonObjectId.Value);
				return;
			}
			writer.WriteValue(bsonObjectId.Value);
		}
	}
}