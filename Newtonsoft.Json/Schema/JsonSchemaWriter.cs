using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaWriter
	{
		private readonly JsonWriter _writer;

		private readonly JsonSchemaResolver _resolver;

		public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
			this._resolver = resolver;
		}

		private void ReferenceOrWriteSchema(JsonSchema schema)
		{
			if (schema.Id == null || this._resolver.GetSchema(schema.Id) == null)
			{
				this.WriteSchema(schema);
				return;
			}
			this._writer.WriteStartObject();
			this._writer.WritePropertyName("$ref");
			this._writer.WriteValue(schema.Id);
			this._writer.WriteEndObject();
		}

		private void WriteItems(JsonSchema schema)
		{
			if (schema.Items == null && !schema.PositionalItemsValidation)
			{
				return;
			}
			this._writer.WritePropertyName("items");
			if (!schema.PositionalItemsValidation)
			{
				if (schema.Items != null && schema.Items.Count > 0)
				{
					this.ReferenceOrWriteSchema(schema.Items[0]);
					return;
				}
				this._writer.WriteStartObject();
				this._writer.WriteEndObject();
				return;
			}
			this._writer.WriteStartArray();
			if (schema.Items != null)
			{
				foreach (JsonSchema item in schema.Items)
				{
					this.ReferenceOrWriteSchema(item);
				}
			}
			this._writer.WriteEndArray();
		}

		private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}

		public void WriteSchema(JsonSchema schema)
		{
			JsonSchemaType? type;
			ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!this._resolver.LoadedSchemas.Contains(schema))
			{
				this._resolver.LoadedSchemas.Add(schema);
			}
			this._writer.WriteStartObject();
			this.WritePropertyIfNotNull(this._writer, "id", schema.Id);
			this.WritePropertyIfNotNull(this._writer, "title", schema.Title);
			this.WritePropertyIfNotNull(this._writer, "description", schema.Description);
			this.WritePropertyIfNotNull(this._writer, "required", schema.Required);
			this.WritePropertyIfNotNull(this._writer, "readonly", schema.ReadOnly);
			this.WritePropertyIfNotNull(this._writer, "hidden", schema.Hidden);
			this.WritePropertyIfNotNull(this._writer, "transient", schema.Transient);
			if (schema.Type.HasValue)
			{
				JsonWriter jsonWriter = this._writer;
				type = schema.Type;
				this.WriteType("type", jsonWriter, type.GetValueOrDefault());
			}
			if (!schema.AllowAdditionalProperties)
			{
				this._writer.WritePropertyName("additionalProperties");
				this._writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				this._writer.WritePropertyName("additionalProperties");
				this.ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			if (!schema.AllowAdditionalItems)
			{
				this._writer.WritePropertyName("additionalItems");
				this._writer.WriteValue(schema.AllowAdditionalItems);
			}
			else if (schema.AdditionalItems != null)
			{
				this._writer.WritePropertyName("additionalItems");
				this.ReferenceOrWriteSchema(schema.AdditionalItems);
			}
			this.WriteSchemaDictionaryIfNotNull(this._writer, "properties", schema.Properties);
			this.WriteSchemaDictionaryIfNotNull(this._writer, "patternProperties", schema.PatternProperties);
			this.WriteItems(schema);
			this.WritePropertyIfNotNull(this._writer, "minimum", schema.Minimum);
			this.WritePropertyIfNotNull(this._writer, "maximum", schema.Maximum);
			this.WritePropertyIfNotNull(this._writer, "exclusiveMinimum", schema.ExclusiveMinimum);
			this.WritePropertyIfNotNull(this._writer, "exclusiveMaximum", schema.ExclusiveMaximum);
			this.WritePropertyIfNotNull(this._writer, "minLength", schema.MinimumLength);
			this.WritePropertyIfNotNull(this._writer, "maxLength", schema.MaximumLength);
			this.WritePropertyIfNotNull(this._writer, "minItems", schema.MinimumItems);
			this.WritePropertyIfNotNull(this._writer, "maxItems", schema.MaximumItems);
			this.WritePropertyIfNotNull(this._writer, "divisibleBy", schema.DivisibleBy);
			this.WritePropertyIfNotNull(this._writer, "format", schema.Format);
			this.WritePropertyIfNotNull(this._writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				this._writer.WritePropertyName("enum");
				this._writer.WriteStartArray();
				foreach (JToken @enum in schema.Enum)
				{
					@enum.WriteTo(this._writer, new JsonConverter[0]);
				}
				this._writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				this._writer.WritePropertyName("default");
				schema.Default.WriteTo(this._writer, new JsonConverter[0]);
			}
			if (schema.Disallow.HasValue)
			{
				JsonWriter jsonWriter1 = this._writer;
				type = schema.Disallow;
				this.WriteType("disallow", jsonWriter1, type.GetValueOrDefault());
			}
			if (schema.Extends != null && schema.Extends.Count > 0)
			{
				this._writer.WritePropertyName("extends");
				if (schema.Extends.Count != 1)
				{
					this._writer.WriteStartArray();
					foreach (JsonSchema extend in schema.Extends)
					{
						this.ReferenceOrWriteSchema(extend);
					}
					this._writer.WriteEndArray();
				}
				else
				{
					this.ReferenceOrWriteSchema(schema.Extends[0]);
				}
			}
			this._writer.WriteEndObject();
		}

		private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
		{
			if (properties != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteStartObject();
				foreach (KeyValuePair<string, JsonSchema> property in properties)
				{
					writer.WritePropertyName(property.Key);
					this.ReferenceOrWriteSchema(property.Value);
				}
				writer.WriteEndObject();
			}
		}

		private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
		{
			if (Enum.IsDefined(typeof(JsonSchemaType), type))
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(JsonSchemaBuilder.MapType(type));
				return;
			}
			IEnumerator<JsonSchemaType> enumerator = (
				from v in EnumUtils.GetFlagsValues<JsonSchemaType>(type)
				where v != JsonSchemaType.None
				select v).GetEnumerator();
			if (enumerator.MoveNext())
			{
				writer.WritePropertyName(propertyName);
				JsonSchemaType current = enumerator.Current;
				if (enumerator.MoveNext())
				{
					writer.WriteStartArray();
					writer.WriteValue(JsonSchemaBuilder.MapType(current));
					do
					{
						writer.WriteValue(JsonSchemaBuilder.MapType(enumerator.Current));
					}
					while (enumerator.MoveNext());
					writer.WriteEndArray();
					return;
				}
				writer.WriteValue(JsonSchemaBuilder.MapType(current));
			}
		}
	}
}