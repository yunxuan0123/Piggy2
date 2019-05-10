using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModelBuilder
	{
		private JsonSchemaNodeCollection _nodes;

		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels;

		private JsonSchemaNode _node;

		public JsonSchemaModelBuilder()
		{
			Class6.yDnXvgqzyB5jw();
			this._nodes = new JsonSchemaNodeCollection();
			this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			base();
		}

		public void AddAdditionalItems(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalItems = this.AddSchema(parentNode.AdditionalItems, schema);
		}

		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
		}

		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode item;
			if (parentNode.Items.Count > index)
			{
				item = parentNode.Items[index];
			}
			else
			{
				item = null;
			}
			JsonSchemaNode jsonSchemaNode = this.AddSchema(item, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
				return;
			}
			parentNode.Items[index] = jsonSchemaNode;
		}

		public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
		{
			if (source != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in source)
				{
					this.AddProperty(target, keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
		{
			JsonSchemaNode jsonSchemaNode;
			target.TryGetValue(propertyName, out jsonSchemaNode);
			target[propertyName] = this.AddSchema(jsonSchemaNode, schema);
		}

		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string id;
			if (existingNode == null)
			{
				id = JsonSchemaNode.GetId((IEnumerable<JsonSchema>)(new JsonSchema[] { schema }));
			}
			else
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = JsonSchemaNode.GetId(existingNode.Schemas.Union<JsonSchema>((IEnumerable<JsonSchema>)(new JsonSchema[] { schema })));
			}
			if (this._nodes.Contains(id))
			{
				return this._nodes[id];
			}
			JsonSchemaNode jsonSchemaNode = (existingNode != null ? existingNode.Combine(schema) : new JsonSchemaNode(schema));
			this._nodes.Add(jsonSchemaNode);
			this.AddProperties(schema.Properties, jsonSchemaNode.Properties);
			this.AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					this.AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				this.AddAdditionalItems(jsonSchemaNode, schema.AdditionalItems);
			}
			if (schema.AdditionalProperties != null)
			{
				this.AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				foreach (JsonSchema extend in schema.Extends)
				{
					jsonSchemaNode = this.AddSchema(jsonSchemaNode, extend);
				}
			}
			return jsonSchemaNode;
		}

		public JsonSchemaModel Build(JsonSchema schema)
		{
			this._nodes = new JsonSchemaNodeCollection();
			this._node = this.AddSchema(null, schema);
			this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return this.BuildNodeModel(this._node);
		}

		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			JsonSchemaModel strs;
			if (this._nodeModels.TryGetValue(node, out strs))
			{
				return strs;
			}
			strs = JsonSchemaModel.Create(node.Schemas);
			this._nodeModels[node] = strs;
			foreach (KeyValuePair<string, JsonSchemaNode> property in node.Properties)
			{
				if (strs.Properties == null)
				{
					strs.Properties = new Dictionary<string, JsonSchemaModel>();
				}
				strs.Properties[property.Key] = this.BuildNodeModel(property.Value);
			}
			foreach (KeyValuePair<string, JsonSchemaNode> patternProperty in node.PatternProperties)
			{
				if (strs.PatternProperties == null)
				{
					strs.PatternProperties = new Dictionary<string, JsonSchemaModel>();
				}
				strs.PatternProperties[patternProperty.Key] = this.BuildNodeModel(patternProperty.Value);
			}
			foreach (JsonSchemaNode item in node.Items)
			{
				if (strs.Items == null)
				{
					strs.Items = new List<JsonSchemaModel>();
				}
				strs.Items.Add(this.BuildNodeModel(item));
			}
			if (node.AdditionalProperties != null)
			{
				strs.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
			}
			if (node.AdditionalItems != null)
			{
				strs.AdditionalItems = this.BuildNodeModel(node.AdditionalItems);
			}
			return strs;
		}
	}
}