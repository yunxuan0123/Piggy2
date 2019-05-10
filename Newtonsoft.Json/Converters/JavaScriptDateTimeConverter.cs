using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class JavaScriptDateTimeConverter : DateTimeConverterBase
	{
		public JavaScriptDateTimeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DateTime dateTime;
			string str;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
			{
				throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
			}
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out str))
			{
				throw JsonSerializationException.Create(reader, str);
			}
			if ((ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType) != typeof(DateTimeOffset))
			{
				return dateTime;
			}
			return new DateTimeOffset(dateTime);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			long javaScriptTicks;
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
				DateTimeOffset universalTime = ((DateTimeOffset)obj1).ToUniversalTime();
				javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(universalTime.UtcDateTime);
			}
			else
			{
				javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(((DateTime)obj1).ToUniversalTime());
			}
			writer.WriteStartConstructor("Date");
			writer.WriteValue(javaScriptTicks);
			writer.WriteEndConstructor();
		}
	}
}