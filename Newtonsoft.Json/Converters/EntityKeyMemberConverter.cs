using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class EntityKeyMemberConverter : JsonConverter
	{
		private static ReflectionObject _reflectionObject;

		public EntityKeyMemberConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember", false);
		}

		private static void EnsureReflectionObject(Type objectType)
		{
			if (EntityKeyMemberConverter._reflectionObject == null)
			{
				EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, new string[] { "Key", "Value" });
			}
		}

		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			reader.ReadAndAssert();
			if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
			{
				throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			EntityKeyMemberConverter.EnsureReflectionObject(objectType);
			object creator = EntityKeyMemberConverter._reflectionObject.Creator(new object[0]);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
			reader.ReadAndAssert();
			EntityKeyMemberConverter._reflectionObject.SetValue(creator, "Key", reader.Value.ToString());
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
			reader.ReadAndAssert();
			Type type = Type.GetType(reader.Value.ToString());
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
			reader.ReadAndAssert();
			EntityKeyMemberConverter._reflectionObject.SetValue(creator, "Value", serializer.Deserialize(reader, type));
			reader.ReadAndAssert();
			return creator;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string str;
			Type type;
			string fullName;
			EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
			DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
			string str1 = (string)EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
			object obj = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
			if (obj != null)
			{
				type = obj.GetType();
			}
			else
			{
				type = null;
			}
			Type type1 = type;
			writer.WriteStartObject();
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key"));
			writer.WriteValue(str1);
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Type") : "Type"));
			JsonWriter jsonWriter = writer;
			if (type1 != null)
			{
				fullName = type1.FullName;
			}
			else
			{
				fullName = null;
			}
			jsonWriter.WriteValue(fullName);
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value"));
			if (type1 == null)
			{
				writer.WriteNull();
			}
			else if (!JsonSerializerInternalWriter.TryConvertToString(obj, type1, out str))
			{
				writer.WriteValue(obj);
			}
			else
			{
				writer.WriteValue(str);
			}
			writer.WriteEndObject();
		}
	}
}