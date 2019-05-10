using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Converters
{
	public class RegexConverter : JsonConverter
	{
		public RegexConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType.Name != "Regex")
			{
				return false;
			}
			return this.IsRegex(objectType);
		}

		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		private bool IsRegex(Type objectType)
		{
			return objectType == typeof(Regex);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadRegexObject(reader, serializer);
			}
			if (tokenType == JsonToken.String)
			{
				return this.ReadRegexString(reader);
			}
			if (tokenType != JsonToken.Null)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
			}
			return null;
		}

		private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
		{
			string value = null;
			RegexOptions? nullable = null;
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType == JsonToken.PropertyName)
				{
					string str = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
					}
					if (string.Equals(str, "Pattern", StringComparison.OrdinalIgnoreCase))
					{
						value = (string)reader.Value;
					}
					else if (!string.Equals(str, "Options", StringComparison.OrdinalIgnoreCase))
					{
						reader.Skip();
					}
					else
					{
						nullable = new RegexOptions?(serializer.Deserialize<RegexOptions>(reader));
					}
				}
				else
				{
					if (tokenType == JsonToken.Comment)
					{
						continue;
					}
					if (tokenType == JsonToken.EndObject)
					{
						if (value == null)
						{
							throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
						}
						string str1 = value;
						RegexOptions? nullable1 = nullable;
						return new Regex(str1, (nullable1.HasValue ? nullable1.GetValueOrDefault() : RegexOptions.None));
					}
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
		}

		private object ReadRegexString(JsonReader reader)
		{
			string value = (string)reader.Value;
			if (value.Length > 0 && value[0] == '/')
			{
				int num = value.LastIndexOf('/');
				if (num > 0)
				{
					string str = value.Substring(1, num - 1);
					RegexOptions regexOptions = MiscellaneousUtils.GetRegexOptions(value.Substring(num + 1));
					return new Regex(str, regexOptions);
				}
			}
			throw JsonSerializationException.Create(reader, "Regex pattern must be enclosed by slashes.");
		}

		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string str = null;
			if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
			{
				str = string.Concat(str, "i");
			}
			if (this.HasFlag(regex.Options, RegexOptions.Multiline))
			{
				str = string.Concat(str, "m");
			}
			if (this.HasFlag(regex.Options, RegexOptions.Singleline))
			{
				str = string.Concat(str, "s");
			}
			str = string.Concat(str, "u");
			if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
			{
				str = string.Concat(str, "x");
			}
			writer.WriteRegex(regex.ToString(), str);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Regex regex = (Regex)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			BsonWriter bsonWriter1 = bsonWriter;
			if (bsonWriter != null)
			{
				this.WriteBson(bsonWriter1, regex);
				return;
			}
			this.WriteJson(writer, regex, serializer);
		}

		private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
		{
			DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Pattern") : "Pattern"));
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Options") : "Options"));
			serializer.Serialize(writer, regex.Options);
			writer.WriteEndObject();
		}
	}
}