using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		public JsonSchemaModel AdditionalItems
		{
			get;
			set;
		}

		public JsonSchemaModel AdditionalProperties
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

		public JsonSchemaType Disallow
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

		public bool ExclusiveMaximum
		{
			get;
			set;
		}

		public bool ExclusiveMinimum
		{
			get;
			set;
		}

		public IList<JsonSchemaModel> Items
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

		public IDictionary<string, JsonSchemaModel> PatternProperties
		{
			get;
			set;
		}

		public IList<string> Patterns
		{
			get;
			set;
		}

		public bool PositionalItemsValidation
		{
			get;
			set;
		}

		public IDictionary<string, JsonSchemaModel> Properties
		{
			get;
			set;
		}

		public bool Required
		{
			get;
			set;
		}

		public JsonSchemaType Type
		{
			get;
			set;
		}

		public bool UniqueItems
		{
			get;
			set;
		}

		public JsonSchemaModel()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			bool? required;
			bool flag;
			bool flag1;
			bool flag2;
			JsonSchemaModel jsonSchemaModel = model;
			if (model.Required)
			{
				flag = true;
			}
			else
			{
				required = schema.Required;
				flag = (required.HasValue ? required.GetValueOrDefault() : false);
			}
			jsonSchemaModel.Required = flag;
			JsonSchemaModel jsonSchemaModel1 = model;
			JsonSchemaType type = model.Type;
			JsonSchemaType? disallow = schema.Type;
			jsonSchemaModel1.Type = type & (disallow.HasValue ? disallow.GetValueOrDefault() : JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			JsonSchemaModel jsonSchemaModel2 = model;
			if (model.ExclusiveMinimum)
			{
				flag1 = true;
			}
			else
			{
				required = schema.ExclusiveMinimum;
				flag1 = (required.HasValue ? required.GetValueOrDefault() : false);
			}
			jsonSchemaModel2.ExclusiveMinimum = flag1;
			JsonSchemaModel jsonSchemaModel3 = model;
			if (model.ExclusiveMaximum)
			{
				flag2 = true;
			}
			else
			{
				required = schema.ExclusiveMaximum;
				flag2 = (required.HasValue ? required.GetValueOrDefault() : false);
			}
			jsonSchemaModel3.ExclusiveMaximum = flag2;
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = (model.PositionalItemsValidation ? true : schema.PositionalItemsValidation);
			model.AllowAdditionalProperties = (!model.AllowAdditionalProperties ? false : schema.AllowAdditionalProperties);
			model.AllowAdditionalItems = (!model.AllowAdditionalItems ? false : schema.AllowAdditionalItems);
			model.UniqueItems = (model.UniqueItems ? true : schema.UniqueItems);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct<JToken>(schema.Enum, JToken.EqualityComparer);
			}
			JsonSchemaModel jsonSchemaModel4 = model;
			JsonSchemaType jsonSchemaType = model.Disallow;
			disallow = schema.Disallow;
			jsonSchemaModel4.Disallow = jsonSchemaType | (disallow.HasValue ? disallow.GetValueOrDefault() : JsonSchemaType.None);
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct<string>(schema.Pattern);
			}
		}

		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schematum in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schematum);
			}
			return jsonSchemaModel;
		}
	}
}