using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class ExpandoObjectConverter : JsonConverter
	{
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public ExpandoObjectConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return this.ReadValue(reader);
		}

		private object ReadList(JsonReader reader)
		{
			IList<object> objs = new List<object>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType == JsonToken.Comment)
				{
					continue;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return objs;
				}
				objs.Add(this.ReadValue(reader));
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> expandoObjects = new ExpandoObject();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType == JsonToken.PropertyName)
				{
					string str = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					expandoObjects[str] = this.ReadValue(reader);
				}
				else
				{
					if (tokenType == JsonToken.Comment)
					{
						continue;
					}
					if (tokenType == JsonToken.EndObject)
					{
						return expandoObjects;
					}
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		private object ReadValue(JsonReader reader)
		{
			if (!reader.MoveToContent())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
			}
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadObject(reader);
			}
			if (tokenType == JsonToken.StartArray)
			{
				return this.ReadList(reader);
			}
			if (!JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
			{
				throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			return reader.Value;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
}