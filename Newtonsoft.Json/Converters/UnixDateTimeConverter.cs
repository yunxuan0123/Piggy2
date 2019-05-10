using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class UnixDateTimeConverter : DateTimeConverterBase
	{
		internal readonly static DateTime UnixEpoch;

		static UnixDateTimeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			UnixDateTimeConverter.UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}

		public UnixDateTimeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			long value;
			bool flag = ReflectionUtils.IsNullable(objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer or String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				if (!long.TryParse((string)reader.Value, out value))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert invalid value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
			}
			else
			{
				value = (long)reader.Value;
			}
			if (value < 0L)
			{
				throw JsonSerializationException.Create(reader, "Cannot convert value that is before Unix epoch of 00:00:00 UTC on 1 January 1970 to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			DateTime dateTime = UnixDateTimeConverter.UnixEpoch.AddSeconds((double)value);
			if ((flag ? Nullable.GetUnderlyingType(objectType) : objectType) != typeof(DateTimeOffset))
			{
				return dateTime;
			}
			return new DateTimeOffset(dateTime, TimeSpan.Zero);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			long totalSeconds;
			TimeSpan universalTime;
			object obj = value;
			object obj1 = obj;
			if (!(obj is DateTime))
			{
				object obj2 = value;
				obj1 = obj2;
				if (!(obj2 is DateTimeOffset))
				{
					throw new JsonSerializationException("Expected date object value.");
				}
				universalTime = ((DateTimeOffset)obj1).ToUniversalTime() - UnixDateTimeConverter.UnixEpoch;
				totalSeconds = (long)universalTime.TotalSeconds;
			}
			else
			{
				universalTime = ((DateTime)obj1).ToUniversalTime() - UnixDateTimeConverter.UnixEpoch;
				totalSeconds = (long)universalTime.TotalSeconds;
			}
			if (totalSeconds < 0L)
			{
				throw new JsonSerializationException("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.");
			}
			writer.WriteValue(totalSeconds);
		}
	}
}