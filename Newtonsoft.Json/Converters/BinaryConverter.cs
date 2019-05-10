using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class BinaryConverter : JsonConverter
	{
		private static ReflectionObject _reflectionObject;

		public BinaryConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType.FullName == "System.Data.Linq.Binary")
			{
				return true;
			}
			if (!(objectType == typeof(SqlBinary)) && !(objectType == typeof(SqlBinary?)))
			{
				return false;
			}
			return true;
		}

		private static void EnsureReflectionObject(Type t)
		{
			if (BinaryConverter._reflectionObject == null)
			{
				BinaryConverter._reflectionObject = ReflectionObject.Create(t, t.GetConstructor(new Type[] { typeof(byte[]) }), new string[] { "ToArray" });
			}
		}

		private byte[] GetByteArray(object value)
		{
			if (value.GetType().FullName == "System.Data.Linq.Binary")
			{
				BinaryConverter.EnsureReflectionObject(value.GetType());
				return (byte[])BinaryConverter._reflectionObject.GetValue(value, "ToArray");
			}
			object obj = value;
			object obj1 = obj;
			if (!(obj is SqlBinary))
			{
				throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
			}
			return ((SqlBinary)obj1).Value;
		}

		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> nums = new List<byte>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType == JsonToken.Comment)
				{
					continue;
				}
				if (tokenType != JsonToken.Integer)
				{
					if (tokenType != JsonToken.EndArray)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
					}
					return nums.ToArray();
				}
				nums.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			byte[] numArray;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				numArray = Convert.FromBase64String(reader.Value.ToString());
			}
			else
			{
				numArray = this.ReadByteArray(reader);
			}
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (type.FullName == "System.Data.Linq.Binary")
			{
				BinaryConverter.EnsureReflectionObject(type);
				return BinaryConverter._reflectionObject.Creator(new object[] { numArray });
			}
			if (type != typeof(SqlBinary))
			{
				throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			return new SqlBinary(numArray);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue(this.GetByteArray(value));
		}
	}
}