using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		private readonly string _internalId;

		public JsonSchema AdditionalItems
		{
			get;
			set;
		}

		public JsonSchema AdditionalProperties
		{
			get;
			set;
		}

		public bool AllowAdditionalItems
		{
			get;
			set;
		}

		public bool AllowAdditionalProperties
		{
			get;
			set;
		}

		public JToken Default
		{
			get;
			set;
		}

		internal string DeferredReference
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public JsonSchemaType? Disallow
		{
			get;
			set;
		}

		public double? DivisibleBy
		{
			get;
			set;
		}

		public IList<JToken> Enum
		{
			get;
			set;
		}

		public bool? ExclusiveMaximum
		{
			get;
			set;
		}

		public bool? ExclusiveMinimum
		{
			get;
			set;
		}

		public IList<JsonSchema> Extends
		{
			get;
			set;
		}

		public string Format
		{
			get;
			set;
		}

		public bool? Hidden
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		public IList<JsonSchema> Items
		{
			get;
			set;
		}

		internal string Location
		{
			get;
			set;
		}

		public double? Maximum
		{
			get;
			set;
		}

		public int? MaximumItems
		{
			get;
			set;
		}

		public int? MaximumLength
		{
			get;
			set;
		}

		public double? Minimum
		{
			get;
			set;
		}

		public int? MinimumItems
		{
			get;
			set;
		}

		public int? MinimumLength
		{
			get;
			set;
		}

		public string Pattern
		{
			get;
			set;
		}

		public IDictionary<string, JsonSchema> PatternProperties
		{
			get;
			set;
		}

		public bool PositionalItemsValidation
		{
			get;
			set;
		}

		public IDictionary<string, JsonSchema> Properties
		{
			get;
			set;
		}

		public bool? ReadOnly
		{
			get;
			set;
		}

		internal bool ReferencesResolved
		{
			get;
			set;
		}

		public bool? Required
		{
			get;
			set;
		}

		public string Requires
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public bool? Transient
		{
			get;
			set;
		}

		public JsonSchemaType? Type
		{
			get;
			set;
		}

		public bool UniqueItems
		{
			get;
			set;
		}

		public JsonSchema()
		{
			Class6.yDnXvgqzyB5jw();
			this._internalId = Guid.NewGuid().ToString("N");
			base();
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			JsonSchema jsonSchema;
			ValidationUtils.ArgumentNotNull(json, "json");
			using (JsonReader jsonTextReader = new JsonTextReader(new StringReader(json)))
			{
				jsonSchema = JsonSchema.Read(jsonTextReader, resolver);
			}
			return jsonSchema;
		}

		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return (new JsonSchemaBuilder(resolver)).Read(reader);
		}

		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			(new JsonSchemaWriter(writer, resolver)).WriteSchema(this);
		}
	}
}