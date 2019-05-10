using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaBuilder
	{
		private readonly IList<JsonSchema> _stack;

		private readonly JsonSchemaResolver _resolver;

		private readonly IDictionary<string, JsonSchema> _documentSchemas;

		private JsonSchema _currentSchema;

		private JObject _rootSchema;

		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._stack = new List<JsonSchema>();
			this._documentSchemas = new Dictionary<string, JsonSchema>();
			this._resolver = resolver;
		}

		private JsonSchema BuildSchema(JToken token)
		{
			JToken jTokens;
			JsonSchema jsonSchema;
			JObject jObjects = token as JObject;
			JObject jObjects1 = jObjects;
			if (jObjects == null)
			{
				throw JsonException.Create(token, token.Path, "Expected object while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			if (jObjects1.TryGetValue("$ref", out jTokens))
			{
				return new JsonSchema()
				{
					DeferredReference = (string)jTokens
				};
			}
			string str = token.Path.Replace(".", "/").Replace("[", "/").Replace("]", string.Empty);
			if (!string.IsNullOrEmpty(str))
			{
				str = string.Concat("/", str);
			}
			str = string.Concat("#", str);
			if (this._documentSchemas.TryGetValue(str, out jsonSchema))
			{
				return jsonSchema;
			}
			this.Push(new JsonSchema()
			{
				Location = str
			});
			this.ProcessSchemaProperties(jObjects1);
			return this.Pop();
		}

		internal static JsonSchemaType MapType(string type)
		{
			JsonSchemaType jsonSchemaType;
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out jsonSchemaType))
			{
				throw new JsonException("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, type));
			}
			return jsonSchemaType;
		}

		internal static string MapType(JsonSchemaType type)
		{
			KeyValuePair<string, JsonSchemaType> keyValuePair = JsonSchemaConstants.JsonSchemaTypeMapping.Single<KeyValuePair<string, JsonSchemaType>>((KeyValuePair<string, JsonSchemaType> kv) => kv.Value == type);
			return keyValuePair.Key;
		}

		private JsonSchema Pop()
		{
			JsonSchema jsonSchema = this._currentSchema;
			this._stack.RemoveAt(this._stack.Count - 1);
			this._currentSchema = this._stack.LastOrDefault<JsonSchema>();
			return jsonSchema;
		}

		private void ProcessAdditionalItems(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				this.CurrentSchema.AllowAdditionalItems = (bool)token;
				return;
			}
			this.CurrentSchema.AdditionalItems = this.BuildSchema(token);
		}

		private void ProcessAdditionalProperties(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				this.CurrentSchema.AllowAdditionalProperties = (bool)token;
				return;
			}
			this.CurrentSchema.AdditionalProperties = this.BuildSchema(token);
		}

		private void ProcessEnum(JToken token)
		{
			if (token.Type != JTokenType.Array)
			{
				throw JsonException.Create(token, token.Path, "Expected Array token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			this.CurrentSchema.Enum = new List<JToken>();
			foreach (JToken jTokens in (IEnumerable<JToken>)token)
			{
				this.CurrentSchema.Enum.Add(jTokens.DeepClone());
			}
		}

		private void ProcessExtends(JToken token)
		{
			IList<JsonSchema> jsonSchemas = new List<JsonSchema>();
			if (token.Type != JTokenType.Array)
			{
				JsonSchema jsonSchema = this.BuildSchema(token);
				if (jsonSchema != null)
				{
					jsonSchemas.Add(jsonSchema);
				}
			}
			else
			{
				foreach (JToken jTokens in (IEnumerable<JToken>)token)
				{
					jsonSchemas.Add(this.BuildSchema(jTokens));
				}
			}
			if (jsonSchemas.Count > 0)
			{
				this.CurrentSchema.Extends = jsonSchemas;
			}
		}

		private void ProcessItems(JToken token)
		{
			this.CurrentSchema.Items = new List<JsonSchema>();
			JTokenType type = token.Type;
			if (type == JTokenType.Object)
			{
				this.CurrentSchema.Items.Add(this.BuildSchema(token));
				this.CurrentSchema.PositionalItemsValidation = false;
				return;
			}
			if (type != JTokenType.Array)
			{
				throw JsonException.Create(token, token.Path, "Expected array or JSON schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			this.CurrentSchema.PositionalItemsValidation = true;
			foreach (JToken jTokens in (IEnumerable<JToken>)token)
			{
				this.CurrentSchema.Items.Add(this.BuildSchema(jTokens));
			}
		}

		private IDictionary<string, JsonSchema> ProcessProperties(JToken token)
		{
			IDictionary<string, JsonSchema> strs = new Dictionary<string, JsonSchema>();
			if (token.Type != JTokenType.Object)
			{
				throw JsonException.Create(token, token.Path, "Expected Object token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			foreach (JProperty jProperty in (IEnumerable<JToken>)token)
			{
				if (strs.ContainsKey(jProperty.Name))
				{
					throw new JsonException("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, jProperty.Name));
				}
				strs.Add(jProperty.Name, this.BuildSchema(jProperty.Value));
			}
			return strs;
		}

		private void ProcessSchemaProperties(JObject schemaObject)
		{
			foreach (KeyValuePair<string, JToken> keyValuePair in schemaObject)
			{
				string key = keyValuePair.Key;
				uint num = <PrivateImplementationDetails>{39BAF5C6-2ACA-458F-94B0-6671FAB39C26}.ComputeStringHash(key);
				if (num <= -2071165408)
				{
					if (num <= 981021583)
					{
						if (num <= 353304077)
						{
							if (num == 299789532)
							{
								if (key != "properties")
								{
									continue;
								}
								this.CurrentSchema.Properties = this.ProcessProperties(keyValuePair.Value);
							}
							else if (num == 334560121)
							{
								if (key != "minItems")
								{
									continue;
								}
								this.CurrentSchema.MinimumItems = new int?((int)keyValuePair.Value);
							}
							else if (num == 353304077)
							{
								if (key != "divisibleBy")
								{
									continue;
								}
								this.CurrentSchema.DivisibleBy = new double?((double)((double)keyValuePair.Value));
							}
						}
						else if (num <= 879704937)
						{
							if (num == 479998177)
							{
								if (key != "additionalProperties")
								{
									continue;
								}
								this.ProcessAdditionalProperties(keyValuePair.Value);
							}
							else if (num == 879704937)
							{
								if (key != "description")
								{
									continue;
								}
								this.CurrentSchema.Description = (string)keyValuePair.Value;
							}
						}
						else if (num == 926444256)
						{
							if (key != "id")
							{
								continue;
							}
							this.CurrentSchema.Id = (string)keyValuePair.Value;
						}
						else if (num == 981021583)
						{
							if (key != "items")
							{
								continue;
							}
							this.ProcessItems(keyValuePair.Value);
						}
					}
					else if (num <= 1693958795)
					{
						if (num == 1361572173)
						{
							if (key != "type")
							{
								continue;
							}
							this.CurrentSchema.Type = this.ProcessType(keyValuePair.Value);
						}
						else if (num == 1542649473)
						{
							if (key != "maximum")
							{
								continue;
							}
							this.CurrentSchema.Maximum = new double?((double)((double)keyValuePair.Value));
						}
						else if (num == 1693958795)
						{
							if (key != "exclusiveMaximum")
							{
								continue;
							}
							this.CurrentSchema.ExclusiveMaximum = new bool?((bool)keyValuePair.Value);
						}
					}
					else if (num <= 2051482624)
					{
						if (num == 1913005517)
						{
							if (key != "exclusiveMinimum")
							{
								continue;
							}
							this.CurrentSchema.ExclusiveMinimum = new bool?((bool)keyValuePair.Value);
						}
						else if (num == 2051482624)
						{
							if (key != "additionalItems")
							{
								continue;
							}
							this.ProcessAdditionalItems(keyValuePair.Value);
						}
					}
					else if (num == -2123583488)
					{
						if (key != "enum")
						{
							continue;
						}
						this.ProcessEnum(keyValuePair.Value);
					}
					else if (num == -2071165408)
					{
						if (key != "required")
						{
							continue;
						}
						this.CurrentSchema.Required = new bool?((bool)keyValuePair.Value);
					}
				}
				else if (num <= -1602722880)
				{
					if (num <= -1820253449)
					{
						if (num == -2026045143)
						{
							if (key != "pattern")
							{
								continue;
							}
							this.CurrentSchema.Pattern = (string)keyValuePair.Value;
						}
						else if (num == -1824826402)
						{
							if (key != "default")
							{
								continue;
							}
							this.CurrentSchema.Default = keyValuePair.Value.DeepClone();
						}
						else if (num == -1820253449)
						{
							if (key != "minimum")
							{
								continue;
							}
							this.CurrentSchema.Minimum = new double?((double)((double)keyValuePair.Value));
						}
					}
					else if (num <= -1685280171)
					{
						if (num == -1738164983)
						{
							if (key != "title")
							{
								continue;
							}
							this.CurrentSchema.Title = (string)keyValuePair.Value;
						}
						else if (num == -1685280171)
						{
							if (key != "requires")
							{
								continue;
							}
							this.CurrentSchema.Requires = (string)keyValuePair.Value;
						}
					}
					else if (num == -1652173234)
					{
						if (key != "extends")
						{
							continue;
						}
						this.ProcessExtends(keyValuePair.Value);
					}
					else if (num == -1602722880)
					{
						if (key != "disallow")
						{
							continue;
						}
						this.CurrentSchema.Disallow = this.ProcessType(keyValuePair.Value);
					}
				}
				else if (num <= -772364702)
				{
					if (num <= -1180859054)
					{
						if (num == -1337705481)
						{
							if (key != "minLength")
							{
								continue;
							}
							this.CurrentSchema.MinimumLength = new int?((int)keyValuePair.Value);
						}
						else if (num == -1180859054)
						{
							if (key != "format")
							{
								continue;
							}
							this.CurrentSchema.Format = (string)keyValuePair.Value;
						}
					}
					else if (num == -838078473)
					{
						if (key != "readonly")
						{
							continue;
						}
						this.CurrentSchema.ReadOnly = new bool?((bool)keyValuePair.Value);
					}
					else if (num == -772364702)
					{
						if (key != "uniqueItems")
						{
							continue;
						}
						this.CurrentSchema.UniqueItems = (bool)keyValuePair.Value;
					}
				}
				else if (num <= -347360656)
				{
					if (num == -768407359)
					{
						if (key != "maxLength")
						{
							continue;
						}
						this.CurrentSchema.MaximumLength = new int?((int)keyValuePair.Value);
					}
					else if (num == -347360656)
					{
						if (key != "patternProperties")
						{
							continue;
						}
						this.CurrentSchema.PatternProperties = this.ProcessProperties(keyValuePair.Value);
					}
				}
				else if (num == -166137543)
				{
					if (key != "hidden")
					{
						continue;
					}
					this.CurrentSchema.Hidden = new bool?((bool)keyValuePair.Value);
				}
				else if (num == -50645197)
				{
					if (key != "maxItems")
					{
						continue;
					}
					this.CurrentSchema.MaximumItems = new int?((int)keyValuePair.Value);
				}
			}
		}

		private JsonSchemaType? ProcessType(JToken token)
		{
			JsonSchemaType? nullable;
			JTokenType type = token.Type;
			if (type != JTokenType.Array)
			{
				if (type != JTokenType.String)
				{
					throw JsonException.Create(token, token.Path, "Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
				}
				return new JsonSchemaType?(JsonSchemaBuilder.MapType((string)token));
			}
			JsonSchemaType? nullable1 = new JsonSchemaType?(JsonSchemaType.None);
			foreach (JToken jTokens in (IEnumerable<JToken>)token)
			{
				if (jTokens.Type != JTokenType.String)
				{
					throw JsonException.Create(jTokens, jTokens.Path, "Expected JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
				}
				JsonSchemaType? nullable2 = nullable1;
				JsonSchemaType jsonSchemaType = JsonSchemaBuilder.MapType((string)jTokens);
				if (nullable2.HasValue)
				{
					nullable = new JsonSchemaType?(nullable2.GetValueOrDefault() | jsonSchemaType);
				}
				else
				{
					nullable = null;
				}
				nullable1 = nullable;
			}
			return nullable1;
		}

		private void Push(JsonSchema value)
		{
			this._currentSchema = value;
			this._stack.Add(value);
			this._resolver.LoadedSchemas.Add(value);
			this._documentSchemas.Add(value.Location, value);
		}

		internal JsonSchema Read(JsonReader reader)
		{
			JToken jTokens = JToken.ReadFrom(reader);
			this._rootSchema = jTokens as JObject;
			JsonSchema jsonSchema = this.BuildSchema(jTokens);
			this.ResolveReferences(jsonSchema);
			return jsonSchema;
		}

		private JsonSchema ResolveReferences(JsonSchema schema)
		{
			int num;
			if (schema.DeferredReference != null)
			{
				string deferredReference = schema.DeferredReference;
				bool flag = deferredReference.StartsWith("#", StringComparison.Ordinal);
				bool flag1 = flag;
				if (flag)
				{
					deferredReference = this.UnescapeReference(deferredReference);
				}
				JsonSchema jsonSchema = this._resolver.GetSchema(deferredReference);
				if (jsonSchema == null)
				{
					if (flag1)
					{
						string[] strArrays = schema.DeferredReference.TrimStart(new char[] { '#' }).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						JToken item = this._rootSchema;
						string[] strArrays1 = strArrays;
						for (int i = 0; i < (int)strArrays1.Length; i++)
						{
							string str = this.UnescapeReference(strArrays1[i]);
							if (item.Type == JTokenType.Object)
							{
								item = item[str];
							}
							else if (item.Type == JTokenType.Array || item.Type == JTokenType.Constructor)
							{
								if (!int.TryParse(str, out num) || num < 0 || num >= item.Count<JToken>())
								{
									item = null;
								}
								else
								{
									item = item[num];
								}
							}
							if (item == null)
							{
								break;
							}
						}
						if (item != null)
						{
							jsonSchema = this.BuildSchema(item);
						}
					}
					if (jsonSchema == null)
					{
						throw new JsonException("Could not resolve schema reference '{0}'.".FormatWith(CultureInfo.InvariantCulture, schema.DeferredReference));
					}
				}
				schema = jsonSchema;
			}
			if (schema.ReferencesResolved)
			{
				return schema;
			}
			schema.ReferencesResolved = true;
			if (schema.Extends != null)
			{
				for (int j = 0; j < schema.Extends.Count; j++)
				{
					schema.Extends[j] = this.ResolveReferences(schema.Extends[j]);
				}
			}
			if (schema.Items != null)
			{
				for (int k = 0; k < schema.Items.Count; k++)
				{
					schema.Items[k] = this.ResolveReferences(schema.Items[k]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				schema.AdditionalItems = this.ResolveReferences(schema.AdditionalItems);
			}
			if (schema.PatternProperties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> list in schema.PatternProperties.ToList<KeyValuePair<string, JsonSchema>>())
				{
					schema.PatternProperties[list.Key] = this.ResolveReferences(list.Value);
				}
			}
			if (schema.Properties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in schema.Properties.ToList<KeyValuePair<string, JsonSchema>>())
				{
					schema.Properties[keyValuePair.Key] = this.ResolveReferences(keyValuePair.Value);
				}
			}
			if (schema.AdditionalProperties != null)
			{
				schema.AdditionalProperties = this.ResolveReferences(schema.AdditionalProperties);
			}
			return schema;
		}

		private string UnescapeReference(string reference)
		{
			return Uri.UnescapeDataString(reference).Replace("~1", "/").Replace("~0", "~");
		}
	}
}