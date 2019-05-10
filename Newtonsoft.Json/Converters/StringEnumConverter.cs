using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	public class StringEnumConverter : JsonConverter
	{
		public bool AllowIntegerValues
		{
			get;
			set;
		}

		[Obsolete("StringEnumConverter.CamelCaseText is obsolete. Set StringEnumConverter.NamingStrategy with CamelCaseNamingStrategy instead.")]
		public bool CamelCaseText
		{
			get
			{
				if (!(this.NamingStrategy is CamelCaseNamingStrategy))
				{
					return false;
				}
				return true;
			}
			set
			{
				if (value)
				{
					if (this.NamingStrategy is CamelCaseNamingStrategy)
					{
						return;
					}
					this.NamingStrategy = new CamelCaseNamingStrategy();
					return;
				}
				if (!(this.NamingStrategy is CamelCaseNamingStrategy))
				{
					return;
				}
				this.NamingStrategy = null;
			}
		}

		public Newtonsoft.Json.Serialization.NamingStrategy NamingStrategy
		{
			get;
			set;
		}

		public StringEnumConverter()
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
		}

		[Obsolete("StringEnumConverter(bool) is obsolete. Create a converter with StringEnumConverter(NamingStrategy, bool) instead.")]
		public StringEnumConverter(bool camelCaseText)
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
			if (camelCaseText)
			{
				this.NamingStrategy = new CamelCaseNamingStrategy();
			}
		}

		public StringEnumConverter(Newtonsoft.Json.Serialization.NamingStrategy namingStrategy, bool allowIntegerValues = true)
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
			this.NamingStrategy = namingStrategy;
			this.AllowIntegerValues = allowIntegerValues;
		}

		public StringEnumConverter(Type namingStrategyType)
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, null);
		}

		public StringEnumConverter(Type namingStrategyType, object[] namingStrategyParameters)
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
		}

		public StringEnumConverter(Type namingStrategyType, object[] namingStrategyParameters, bool allowIntegerValues)
		{
			Class6.yDnXvgqzyB5jw();
			this.AllowIntegerValues = true;
			base();
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
			this.AllowIntegerValues = allowIntegerValues;
		}

		public override bool CanConvert(Type objectType)
		{
			return ((ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType)).IsEnum();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object obj;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			bool flag = ReflectionUtils.IsNullableType(objectType);
			bool flag1 = flag;
			Type type = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
			try
			{
				if (reader.TokenType == JsonToken.String)
				{
					string str = reader.Value.ToString();
					obj = (!((str == string.Empty) & flag1) ? EnumUtils.ParseEnum(type, this.NamingStrategy, str, !this.AllowIntegerValues) : null);
				}
				else if (reader.TokenType != JsonToken.Integer)
				{
					throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				else
				{
					if (!this.AllowIntegerValues)
					{
						throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, reader.Value));
					}
					obj = ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(reader.Value), objectType), exception);
			}
			return obj;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string str;
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			if (EnumUtils.TryToString(@enum.GetType(), value, this.NamingStrategy, out str))
			{
				writer.WriteValue(str);
				return;
			}
			if (!this.AllowIntegerValues)
			{
				throw JsonSerializationException.Create(null, writer.ContainerPath, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, @enum.ToString("D")), null);
			}
			writer.WriteValue(value);
		}
	}
}