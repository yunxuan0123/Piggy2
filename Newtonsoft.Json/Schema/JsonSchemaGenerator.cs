using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaGenerator
	{
		private IContractResolver _contractResolver;

		private JsonSchemaResolver _resolver;

		private readonly IList<JsonSchemaGenerator.TypeSchema> _stack;

		private JsonSchema _currentSchema;

		public IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		public Newtonsoft.Json.Schema.UndefinedSchemaIdHandling UndefinedSchemaIdHandling
		{
			get;
			set;
		}

		public JsonSchemaGenerator()
		{
			Class6.yDnXvgqzyB5jw();
			this._stack = new List<JsonSchemaGenerator.TypeSchema>();
			base();
		}

		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired == Required.Always)
			{
				return type;
			}
			return type | JsonSchemaType.Null;
		}

		public JsonSchema Generate(Type type)
		{
			return this.Generate(type, new JsonSchemaResolver(), false);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return this.Generate(type, resolver, false);
		}

		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			this._resolver = resolver;
			return this.GenerateInternal(type, (!rootSchemaNullable ? Required.Always : Required.Default), false);
		}

		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			JsonSchemaType? nullable;
			Type type1;
			Type type2;
			JsonSchemaType jsonSchemaType;
			JsonSchemaType? nullable1;
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = this.GetTypeId(type, false);
			string str = this.GetTypeId(type, true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = this._resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
					{
						JsonSchema jsonSchema = schema;
						nullable = jsonSchema.Type;
						if (nullable.HasValue)
						{
							nullable1 = new JsonSchemaType?(nullable.GetValueOrDefault() | JsonSchemaType.Null);
						}
						else
						{
							nullable1 = null;
						}
						jsonSchema.Type = nullable1;
					}
					if (required)
					{
						bool? nullable2 = schema.Required;
						if (!nullable2.GetValueOrDefault() | !nullable2.HasValue)
						{
							schema.Required = new bool?(true);
						}
					}
					return schema;
				}
			}
			if (this._stack.Any<JsonSchemaGenerator.TypeSchema>((JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new JsonException("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
			JsonConverter converter = jsonContract.Converter ?? jsonContract.InternalConverter;
			this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
			if (str != null)
			{
				this.CurrentSchema.Id = str;
			}
			if (required)
			{
				this.CurrentSchema.Required = new bool?(true);
			}
			this.CurrentSchema.Title = this.GetTitle(type);
			this.CurrentSchema.Description = this.GetDescription(type);
			if (converter == null)
			{
				switch (jsonContract.ContractType)
				{
					case JsonContractType.Object:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
						this.CurrentSchema.Id = this.GetTypeId(type, false);
						this.GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
						break;
					}
					case JsonContractType.Array:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
						this.CurrentSchema.Id = this.GetTypeId(type, false);
						JsonArrayAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonArrayAttribute>(type);
						bool flag = (cachedAttribute == null ? true : cachedAttribute.AllowNullItems);
						Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
						if (collectionItemType == null)
						{
							break;
						}
						this.CurrentSchema.Items = new List<JsonSchema>()
						{
							this.GenerateInternal(collectionItemType, (!flag ? Required.Always : Required.Default), false)
						};
						break;
					}
					case JsonContractType.Primitive:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
						nullable = this.CurrentSchema.Type;
						if (!(nullable.GetValueOrDefault() == JsonSchemaType.Integer & nullable.HasValue) || !type.IsEnum() || type.IsDefined(typeof(FlagsAttribute), true))
						{
							break;
						}
						this.CurrentSchema.Enum = new List<JToken>();
						EnumInfo enumValuesAndNames = EnumUtils.GetEnumValuesAndNames(type);
						for (int i = 0; i < (int)enumValuesAndNames.Names.Length; i++)
						{
							ulong values = enumValuesAndNames.Values[i];
							JToken jTokens = JToken.FromObject(Enum.ToObject(type, values));
							this.CurrentSchema.Enum.Add(jTokens);
						}
						break;
					}
					case JsonContractType.String:
					{
						jsonSchemaType = (!ReflectionUtils.IsNullable(jsonContract.UnderlyingType) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired));
						this.CurrentSchema.Type = new JsonSchemaType?(jsonSchemaType);
						break;
					}
					case JsonContractType.Dictionary:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
						ReflectionUtils.GetDictionaryKeyValueTypes(type, out type1, out type2);
						if (!(type1 != null) || this.ContractResolver.ResolveContract(type1).ContractType != JsonContractType.Primitive)
						{
							break;
						}
						this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type2, Required.Default, false);
						break;
					}
					case JsonContractType.Dynamic:
					case JsonContractType.Linq:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
						break;
					}
					case JsonContractType.Serializable:
					{
						this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
						this.CurrentSchema.Id = this.GetTypeId(type, false);
						this.GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
						break;
					}
					default:
					{
						throw new JsonException("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContract));
					}
				}
			}
			else
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
			}
			return this.Pop().Schema;
		}

		private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
		{
			this.CurrentSchema.AllowAdditionalProperties = true;
		}

		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			foreach (JsonProperty property in contract.Properties)
			{
				if (property.Ignored)
				{
					continue;
				}
				NullValueHandling? nullValueHandling = property.NullValueHandling;
				bool flag = (nullValueHandling.GetValueOrDefault() == NullValueHandling.Ignore & nullValueHandling.HasValue || this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || property.ShouldSerialize != null ? true : property.GetIsSpecified != null);
				JsonSchema jsonSchema = this.GenerateInternal(property.PropertyType, property.Required, !flag);
				if (property.DefaultValue != null)
				{
					jsonSchema.Default = JToken.FromObject(property.DefaultValue);
				}
				this.CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
			}
			if (type.IsSealed())
			{
				this.CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		private string GetDescription(Type type)
		{
			string description;
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null)
			{
				description = cachedAttribute.Description;
			}
			else
			{
				description = null;
			}
			if (!string.IsNullOrEmpty(description))
			{
				return cachedAttribute.Description;
			}
			DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>(type);
			if (attribute != null)
			{
				return attribute.Description;
			}
			return null;
		}

		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
				case PrimitiveTypeCode.Empty:
				case PrimitiveTypeCode.Object:
				{
					return jsonSchemaType | JsonSchemaType.String;
				}
				case PrimitiveTypeCode.Char:
				{
					return jsonSchemaType | JsonSchemaType.String;
				}
				case PrimitiveTypeCode.CharNullable:
				case PrimitiveTypeCode.BooleanNullable:
				case PrimitiveTypeCode.SByteNullable:
				case PrimitiveTypeCode.Int16Nullable:
				case PrimitiveTypeCode.UInt16Nullable:
				case PrimitiveTypeCode.Int32Nullable:
				case PrimitiveTypeCode.ByteNullable:
				case PrimitiveTypeCode.UInt32Nullable:
				case PrimitiveTypeCode.Int64Nullable:
				case PrimitiveTypeCode.UInt64Nullable:
				case PrimitiveTypeCode.SingleNullable:
				case PrimitiveTypeCode.DoubleNullable:
				case PrimitiveTypeCode.DateTimeNullable:
				case PrimitiveTypeCode.DateTimeOffsetNullable:
				case PrimitiveTypeCode.DecimalNullable:
				case PrimitiveTypeCode.GuidNullable:
				case PrimitiveTypeCode.TimeSpanNullable:
				case PrimitiveTypeCode.BigIntegerNullable:
				{
					throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeCode, type));
				}
				case PrimitiveTypeCode.Boolean:
				{
					return jsonSchemaType | JsonSchemaType.Boolean;
				}
				case PrimitiveTypeCode.SByte:
				case PrimitiveTypeCode.Int16:
				case PrimitiveTypeCode.UInt16:
				case PrimitiveTypeCode.Int32:
				case PrimitiveTypeCode.Byte:
				case PrimitiveTypeCode.UInt32:
				case PrimitiveTypeCode.Int64:
				case PrimitiveTypeCode.UInt64:
				case PrimitiveTypeCode.BigInteger:
				{
					return jsonSchemaType | JsonSchemaType.Integer;
				}
				case PrimitiveTypeCode.Single:
				case PrimitiveTypeCode.Double:
				case PrimitiveTypeCode.Decimal:
				{
					return jsonSchemaType | JsonSchemaType.Float;
				}
				case PrimitiveTypeCode.DateTime:
				case PrimitiveTypeCode.DateTimeOffset:
				{
					return jsonSchemaType | JsonSchemaType.String;
				}
				case PrimitiveTypeCode.Guid:
				case PrimitiveTypeCode.TimeSpan:
				case PrimitiveTypeCode.Uri:
				case PrimitiveTypeCode.String:
				case PrimitiveTypeCode.Bytes:
				{
					return jsonSchemaType | JsonSchemaType.String;
				}
				case PrimitiveTypeCode.DBNull:
				{
					return jsonSchemaType | JsonSchemaType.Null;
				}
				default:
				{
					throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeCode, type));
				}
			}
		}

		private string GetTitle(Type type)
		{
			string title;
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null)
			{
				title = cachedAttribute.Title;
			}
			else
			{
				title = null;
			}
			if (string.IsNullOrEmpty(title))
			{
				return null;
			}
			return cachedAttribute.Title;
		}

		private string GetTypeId(Type type, bool explicitOnly)
		{
			string id;
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null)
			{
				id = cachedAttribute.Id;
			}
			else
			{
				id = null;
			}
			if (!string.IsNullOrEmpty(id))
			{
				return cachedAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			Newtonsoft.Json.Schema.UndefinedSchemaIdHandling undefinedSchemaIdHandling = this.UndefinedSchemaIdHandling;
			if (undefinedSchemaIdHandling == Newtonsoft.Json.Schema.UndefinedSchemaIdHandling.UseTypeName)
			{
				return type.FullName;
			}
			if (undefinedSchemaIdHandling != Newtonsoft.Json.Schema.UndefinedSchemaIdHandling.UseAssemblyQualifiedName)
			{
				return null;
			}
			return type.AssemblyQualifiedName;
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			JsonSchemaType? nullable;
			JsonSchemaType? nullable1;
			JsonSchemaType? nullable2;
			if (!value.HasValue)
			{
				return true;
			}
			JsonSchemaType? nullable3 = value;
			JsonSchemaType jsonSchemaType = flag;
			if (nullable3.HasValue)
			{
				nullable1 = new JsonSchemaType?(nullable3.GetValueOrDefault() & jsonSchemaType);
			}
			else
			{
				nullable = null;
				nullable1 = nullable;
			}
			JsonSchemaType? nullable4 = nullable1;
			if (nullable4.GetValueOrDefault() == flag & nullable4.HasValue)
			{
				return true;
			}
			if (flag == JsonSchemaType.Integer)
			{
				nullable3 = value;
				if (nullable3.HasValue)
				{
					nullable2 = new JsonSchemaType?(nullable3.GetValueOrDefault() & JsonSchemaType.Float);
				}
				else
				{
					nullable = null;
					nullable2 = nullable;
				}
				nullable4 = nullable2;
				if (nullable4.GetValueOrDefault() == JsonSchemaType.Float & nullable4.HasValue)
				{
					return true;
				}
			}
			return false;
		}

		private JsonSchemaGenerator.TypeSchema Pop()
		{
			JsonSchemaGenerator.TypeSchema item = this._stack[this._stack.Count - 1];
			this._stack.RemoveAt(this._stack.Count - 1);
			JsonSchemaGenerator.TypeSchema typeSchema = this._stack.LastOrDefault<JsonSchemaGenerator.TypeSchema>();
			if (typeSchema == null)
			{
				this._currentSchema = null;
				return item;
			}
			this._currentSchema = typeSchema.Schema;
			return item;
		}

		private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
		{
			this._currentSchema = typeSchema.Schema;
			this._stack.Add(typeSchema);
			this._resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		private class TypeSchema
		{
			public JsonSchema Schema
			{
				get;
			}

			public Type Type
			{
				get;
			}

			public TypeSchema(Type type, JsonSchema schema)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				this.Type = type;
				this.Schema = schema;
			}
		}
	}
}