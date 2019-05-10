using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		public IList<JsonSchema> LoadedSchemas
		{
			get;
			protected set;
		}

		public JsonSchemaResolver()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.LoadedSchemas = new List<JsonSchema>();
		}

		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal)) ?? this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			return jsonSchema;
		}
	}
}