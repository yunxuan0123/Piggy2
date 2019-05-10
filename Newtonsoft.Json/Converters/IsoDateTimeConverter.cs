using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class IsoDateTimeConverter : DateTimeConverterBase
	{
		private System.Globalization.DateTimeStyles _dateTimeStyles;

		private string _dateTimeFormat;

		private CultureInfo _culture;

		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.CurrentCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		public string DateTimeFormat
		{
			get
			{
				return this._dateTimeFormat ?? string.Empty;
			}
			set
			{
				string str;
				if (string.IsNullOrEmpty(value))
				{
					str = null;
				}
				else
				{
					str = value;
				}
				this._dateTimeFormat = str;
			}
		}

		public System.Globalization.DateTimeStyles DateTimeStyles
		{
			get
			{
				return this._dateTimeStyles;
			}
			set
			{
				this._dateTimeStyles = value;
			}
		}

		public IsoDateTimeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			this._dateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind;
			base();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			Type type = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == JsonToken.Date)
			{
				if (type == typeof(DateTimeOffset))
				{
					if (reader.Value is DateTimeOffset)
					{
						return reader.Value;
					}
					return new DateTimeOffset((DateTime)reader.Value);
				}
				object value = reader.Value;
				object obj = value;
				if (!(value is DateTimeOffset))
				{
					return reader.Value;
				}
				return ((DateTimeOffset)obj).DateTime;
			}
			if (reader.TokenType != JsonToken.String)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			string str = reader.Value.ToString();
			if (string.IsNullOrEmpty(str) & flag)
			{
				return null;
			}
			if (type == typeof(DateTimeOffset))
			{
				if (string.IsNullOrEmpty(this._dateTimeFormat))
				{
					return DateTimeOffset.Parse(str, this.Culture, this._dateTimeStyles);
				}
				return DateTimeOffset.ParseExact(str, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
			}
			if (string.IsNullOrEmpty(this._dateTimeFormat))
			{
				return DateTime.Parse(str, this.Culture, this._dateTimeStyles);
			}
			return DateTime.ParseExact(str, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string str;
			object obj = value;
			object obj1 = obj;
			if (!(obj is DateTime))
			{
				object obj2 = value;
				obj1 = obj2;
				if (!(obj2 is DateTimeOffset))
				{
					throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)));
				}
				DateTimeOffset universalTime = (DateTimeOffset)obj1;
				if ((this._dateTimeStyles & System.Globalization.DateTimeStyles.AdjustToUniversal) == System.Globalization.DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & System.Globalization.DateTimeStyles.AssumeUniversal) == System.Globalization.DateTimeStyles.AssumeUniversal)
				{
					universalTime = universalTime.ToUniversalTime();
				}
				str = universalTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			else
			{
				DateTime dateTime = (DateTime)obj1;
				if ((this._dateTimeStyles & System.Globalization.DateTimeStyles.AdjustToUniversal) == System.Globalization.DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & System.Globalization.DateTimeStyles.AssumeUniversal) == System.Globalization.DateTimeStyles.AssumeUniversal)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				str = dateTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			writer.WriteValue(str);
		}
	}
}