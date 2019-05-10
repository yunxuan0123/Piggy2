using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json
{
	public abstract class JsonConverter<T> : JsonConverter
	{
		protected JsonConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public sealed override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = existingValue == null;
			bool flag1 = flag;
			if (!flag && !(existingValue is T))
			{
				throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			return this.ReadJson(reader, objectType, (flag1 ? default(T) : (T)existingValue), !flag1, serializer);
		}

		public abstract T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer);

		public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if ((value != null ? !(value is T) : !ReflectionUtils.IsNullable(typeof(T))))
			{
				throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			this.WriteJson(writer, (T)value, serializer);
		}

		public abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);
	}
}