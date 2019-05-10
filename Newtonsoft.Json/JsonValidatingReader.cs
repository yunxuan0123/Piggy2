using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Newtonsoft.Json
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		private readonly JsonReader _reader;

		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		private JsonSchema _schema;

		private JsonSchemaModel _model;

		private JsonValidatingReader.SchemaScope _currentScope;

		private readonly static IList<JsonSchemaModel> EmptySchemaList;

		private IList<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				JsonSchemaModel jsonSchemaModel;
				if (this._currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[] { this._model });
				}
				if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
				{
					return JsonValidatingReader.EmptySchemaList;
				}
				switch (this._currentScope.TokenType)
				{
					case JTokenType.None:
					{
						return this._currentScope.Schemas;
					}
					case JTokenType.Object:
					{
						if (this._currentScope.CurrentPropertyName == null)
						{
							throw new JsonReaderException("CurrentPropertyName has not been set on scope.");
						}
						IList<JsonSchemaModel> jsonSchemaModels = new List<JsonSchemaModel>();
						foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
						{
							if (currentSchema.Properties != null && currentSchema.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out jsonSchemaModel))
							{
								jsonSchemaModels.Add(jsonSchemaModel);
							}
							if (currentSchema.PatternProperties != null)
							{
								foreach (KeyValuePair<string, JsonSchemaModel> patternProperty in currentSchema.PatternProperties)
								{
									if (!Regex.IsMatch(this._currentScope.CurrentPropertyName, patternProperty.Key))
									{
										continue;
									}
									jsonSchemaModels.Add(patternProperty.Value);
								}
							}
							if (jsonSchemaModels.Count != 0 || !currentSchema.AllowAdditionalProperties || currentSchema.AdditionalProperties == null)
							{
								continue;
							}
							jsonSchemaModels.Add(currentSchema.AdditionalProperties);
						}
						return jsonSchemaModels;
					}
					case JTokenType.Array:
					{
						IList<JsonSchemaModel> jsonSchemaModels1 = new List<JsonSchemaModel>();
						foreach (JsonSchemaModel currentSchema1 in this.CurrentSchemas)
						{
							if (currentSchema1.PositionalItemsValidation)
							{
								if (currentSchema1.Items != null && currentSchema1.Items.Count > 0 && currentSchema1.Items.Count > this._currentScope.ArrayItemCount - 1)
								{
									jsonSchemaModels1.Add(currentSchema1.Items[this._currentScope.ArrayItemCount - 1]);
								}
								if (!currentSchema1.AllowAdditionalItems || currentSchema1.AdditionalItems == null)
								{
									continue;
								}
								jsonSchemaModels1.Add(currentSchema1.AdditionalItems);
							}
							else
							{
								if (currentSchema1.Items == null || currentSchema1.Items.Count <= 0)
								{
									continue;
								}
								jsonSchemaModels1.Add(currentSchema1.Items[0]);
							}
						}
						return jsonSchemaModels1;
					}
					case JTokenType.Constructor:
					{
						return JsonValidatingReader.EmptySchemaList;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, this._currentScope.TokenType));
					}
				}
			}
		}

		private IList<JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return this._currentScope.Schemas;
			}
		}

		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		int Newtonsoft.Json.IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				IJsonLineInfo jsonLineInfo1 = jsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo1.LineNumber;
			}
		}

		int Newtonsoft.Json.IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				IJsonLineInfo jsonLineInfo1 = jsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo1.LinePosition;
			}
		}

		public override string Path
		{
			get
			{
				return this._reader.Path;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new InvalidOperationException("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		static JsonValidatingReader()
		{
			Class6.yDnXvgqzyB5jw();
			JsonValidatingReader.EmptySchemaList = new List<JsonSchemaModel>();
		}

		public JsonValidatingReader(JsonReader reader)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput)
			{
				JsonReader jsonReader = this._reader;
				if (jsonReader == null)
				{
					return;
				}
				jsonReader.Close();
			}
		}

		private static double FloatingPointRemainder(double dividend, double divisor)
		{
			return dividend - Math.Floor(dividend / divisor) * divisor;
		}

		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			JsonSchemaType? nullable;
			switch (this._reader.TokenType)
			{
				case JsonToken.StartObject:
				{
					return new JsonSchemaType?(JsonSchemaType.Object);
				}
				case JsonToken.StartArray:
				{
					return new JsonSchemaType?(JsonSchemaType.Array);
				}
				case JsonToken.StartConstructor:
				case JsonToken.PropertyName:
				case JsonToken.Comment:
				case JsonToken.Raw:
				{
					nullable = null;
					return nullable;
				}
				case JsonToken.Integer:
				{
					return new JsonSchemaType?(JsonSchemaType.Integer);
				}
				case JsonToken.Float:
				{
					return new JsonSchemaType?(JsonSchemaType.Float);
				}
				case JsonToken.String:
				{
					return new JsonSchemaType?(JsonSchemaType.String);
				}
				case JsonToken.Boolean:
				{
					return new JsonSchemaType?(JsonSchemaType.Boolean);
				}
				case JsonToken.Null:
				{
					return new JsonSchemaType?(JsonSchemaType.Null);
				}
				default:
				{
					nullable = null;
					return nullable;
				}
			}
		}

		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			bool flag;
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				using (IEnumerator<string> enumerator = schema.PatternProperties.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!Regex.IsMatch(propertyName, enumerator.Current))
						{
							continue;
						}
						flag = true;
						return flag;
					}
					return false;
				}
				return flag;
			}
			return false;
		}

		private static bool IsZero(double value)
		{
			return Math.Abs(value) < 4.44089209850063E-15;
		}

		bool Newtonsoft.Json.IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			IJsonLineInfo jsonLineInfo1 = jsonLineInfo;
			if (jsonLineInfo == null)
			{
				return false;
			}
			return jsonLineInfo1.HasLineInfo();
		}

		private void OnValidationEvent(JsonSchemaException exception)
		{
			Newtonsoft.Json.Schema.ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler == null)
			{
				throw exception;
			}
			validationEventHandler(this, new ValidationEventArgs(exception));
		}

		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope schemaScope;
			JsonValidatingReader.SchemaScope schemaScope1 = this._stack.Pop();
			if (this._stack.Count != 0)
			{
				schemaScope = this._stack.Peek();
			}
			else
			{
				schemaScope = null;
			}
			this._currentScope = schemaScope;
			return schemaScope1;
		}

		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				JsonValidatingReader.SchemaScope arrayItemCount = this._currentScope;
				arrayItemCount.ArrayItemCount = arrayItemCount.ArrayItemCount + 1;
				foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
				{
					if (currentSchema == null || !currentSchema.PositionalItemsValidation || currentSchema.AllowAdditionalItems || currentSchema.Items != null && this._currentScope.ArrayItemCount - 1 < currentSchema.Items.Count)
					{
						continue;
					}
					this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, this._currentScope.ArrayItemCount), currentSchema);
				}
			}
		}

		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		private void RaiseError(string message, JsonSchemaModel schema)
		{
			IJsonLineInfo jsonLineInfo = this;
			string str = (jsonLineInfo.HasLineInfo() ? string.Concat(message, " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, jsonLineInfo.LineNumber, jsonLineInfo.LinePosition)) : message);
			this.OnValidationEvent(new JsonSchemaException(str, null, this.Path, jsonLineInfo.LineNumber, jsonLineInfo.LinePosition));
		}

		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			this.ValidateCurrentToken();
			return true;
		}

		public override bool? ReadAsBoolean()
		{
			bool? nullable = this._reader.ReadAsBoolean();
			this.ValidateCurrentToken();
			return nullable;
		}

		public override byte[] ReadAsBytes()
		{
			byte[] numArray = this._reader.ReadAsBytes();
			this.ValidateCurrentToken();
			return numArray;
		}

		public override DateTime? ReadAsDateTime()
		{
			DateTime? nullable = this._reader.ReadAsDateTime();
			this.ValidateCurrentToken();
			return nullable;
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? nullable = this._reader.ReadAsDateTimeOffset();
			this.ValidateCurrentToken();
			return nullable;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? nullable = this._reader.ReadAsDecimal();
			this.ValidateCurrentToken();
			return nullable;
		}

		public override double? ReadAsDouble()
		{
			double? nullable = this._reader.ReadAsDouble();
			this.ValidateCurrentToken();
			return nullable;
		}

		public override string ReadAsString()
		{
			string str = this._reader.ReadAsString();
			this.ValidateCurrentToken();
			return str;
		}

		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				return true;
			}
			this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
			return false;
		}

		private bool ValidateArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return this.TestType(schema, JsonSchemaType.Array);
		}

		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		private void ValidateCurrentToken()
		{
			if (this._model == null)
			{
				this._model = (new JsonSchemaModelBuilder()).Build(this._schema);
				if (!JsonTokenUtils.IsStartToken(this._reader.TokenType))
				{
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.None, this.CurrentMemberSchemas));
				}
			}
			switch (this._reader.TokenType)
			{
				case JsonToken.None:
				{
					return;
				}
				case JsonToken.StartObject:
				{
					this.ProcessValue();
					IList<JsonSchemaModel> list = this.CurrentMemberSchemas.Where<JsonSchemaModel>(new Func<JsonSchemaModel, bool>(this.ValidateObject)).ToList<JsonSchemaModel>();
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, list));
					this.WriteToken(this.CurrentSchemas);
					return;
				}
				case JsonToken.StartArray:
				{
					this.ProcessValue();
					IList<JsonSchemaModel> jsonSchemaModels = this.CurrentMemberSchemas.Where<JsonSchemaModel>(new Func<JsonSchemaModel, bool>(this.ValidateArray)).ToList<JsonSchemaModel>();
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, jsonSchemaModels));
					this.WriteToken(this.CurrentSchemas);
					return;
				}
				case JsonToken.StartConstructor:
				{
					this.ProcessValue();
					this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
					this.WriteToken(this.CurrentSchemas);
					return;
				}
				case JsonToken.PropertyName:
				{
					this.WriteToken(this.CurrentSchemas);
					using (IEnumerator<JsonSchemaModel> enumerator = this.CurrentSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidatePropertyName(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.Comment:
				{
					throw new ArgumentOutOfRangeException();
				}
				case JsonToken.Raw:
				{
					this.ProcessValue();
					return;
				}
				case JsonToken.Integer:
				{
					this.ProcessValue();
					this.WriteToken(this.CurrentMemberSchemas);
					using (enumerator = this.CurrentMemberSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidateInteger(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.Float:
				{
					this.ProcessValue();
					this.WriteToken(this.CurrentMemberSchemas);
					using (enumerator = this.CurrentMemberSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidateFloat(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.String:
				{
					this.ProcessValue();
					this.WriteToken(this.CurrentMemberSchemas);
					using (enumerator = this.CurrentMemberSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidateString(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.Boolean:
				{
					this.ProcessValue();
					this.WriteToken(this.CurrentMemberSchemas);
					using (enumerator = this.CurrentMemberSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidateBoolean(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.Null:
				{
					this.ProcessValue();
					this.WriteToken(this.CurrentMemberSchemas);
					using (enumerator = this.CurrentMemberSchemas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							this.ValidateNull(enumerator.Current);
						}
						return;
					}
					break;
				}
				case JsonToken.Undefined:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					this.WriteToken(this.CurrentMemberSchemas);
					return;
				}
				case JsonToken.EndObject:
				{
					this.WriteToken(this.CurrentSchemas);
					foreach (JsonSchemaModel currentSchema in this.CurrentSchemas)
					{
						this.ValidateEndObject(currentSchema);
					}
					this.Pop();
					return;
				}
				case JsonToken.EndArray:
				{
					this.WriteToken(this.CurrentSchemas);
					foreach (JsonSchemaModel jsonSchemaModel in this.CurrentSchemas)
					{
						this.ValidateEndArray(jsonSchemaModel);
					}
					this.Pop();
					return;
				}
				case JsonToken.EndConstructor:
				{
					this.WriteToken(this.CurrentSchemas);
					this.Pop();
					return;
				}
				default:
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			int? maximumItems = schema.MaximumItems;
			if (maximumItems.HasValue)
			{
				maximumItems = schema.MaximumItems;
				if (arrayItemCount > maximumItems.GetValueOrDefault() & maximumItems.HasValue)
				{
					this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
				}
			}
			maximumItems = schema.MinimumItems;
			if (maximumItems.HasValue)
			{
				maximumItems = schema.MinimumItems;
				if (arrayItemCount < maximumItems.GetValueOrDefault() & maximumItems.HasValue)
				{
					this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
				}
			}
		}

		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				if (requiredProperties.Values.Any<bool>((bool v) => !v))
				{
					IEnumerable<string> value = 
						from kv in requiredProperties
						where !kv.Value
						select kv.Key;
					this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", value)), schema);
				}
			}
		}

		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
			double? maximum = schema.Maximum;
			if (maximum.HasValue)
			{
				maximum = schema.Maximum;
				if (num > maximum.GetValueOrDefault() & maximum.HasValue)
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum)
				{
					maximum = schema.Maximum;
					if (num == maximum.GetValueOrDefault() & maximum.HasValue)
					{
						this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
					}
				}
			}
			maximum = schema.Minimum;
			if (maximum.HasValue)
			{
				maximum = schema.Minimum;
				if (num < maximum.GetValueOrDefault() & maximum.HasValue)
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum)
				{
					maximum = schema.Minimum;
					if (num == maximum.GetValueOrDefault() & maximum.HasValue)
					{
						this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
					}
				}
			}
			maximum = schema.DivisibleBy;
			if (maximum.HasValue)
			{
				maximum = schema.DivisibleBy;
				if (!JsonValidatingReader.IsZero(JsonValidatingReader.FloatingPointRemainder(num, maximum.GetValueOrDefault())))
				{
					this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.DivisibleBy), schema);
				}
			}
		}

		private void ValidateInteger(JsonSchemaModel schema)
		{
			double? divisibleBy;
			bool bigInteger;
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			object value = this._reader.Value;
			if (schema.Maximum.HasValue)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Maximum) > 0)
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && JValue.Compare(JTokenType.Integer, value, schema.Maximum) == 0)
				{
					this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
			}
			if (schema.Minimum.HasValue)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Minimum) < 0)
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && JValue.Compare(JTokenType.Integer, value, schema.Minimum) == 0)
				{
					this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy.HasValue)
			{
				object obj = value;
				object obj1 = obj;
				if (!(obj is BigInteger))
				{
					double num = (double)Convert.ToInt64(value, CultureInfo.InvariantCulture);
					divisibleBy = schema.DivisibleBy;
					bigInteger = !JsonValidatingReader.IsZero(num % divisibleBy.GetValueOrDefault());
				}
				else
				{
					BigInteger bigInteger1 = (BigInteger)obj1;
					if (Math.Abs(schema.DivisibleBy.Value - Math.Truncate(schema.DivisibleBy.Value)).Equals(0))
					{
						divisibleBy = schema.DivisibleBy;
						bigInteger = (bigInteger1 % new BigInteger(divisibleBy.Value)) != 0L;
					}
					else
					{
						bigInteger = bigInteger1 != 0L;
					}
				}
				if (bigInteger)
				{
					this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
				}
			}
		}

		private void ValidateNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType.HasValue && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.GetValueOrDefault()))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, currentNodeSchemaType), schema);
			}
		}

		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
		}

		private bool ValidateObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return this.TestType(schema, JsonSchemaType.Object);
		}

		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string str = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			if (this._currentScope.RequiredProperties.ContainsKey(str))
			{
				this._currentScope.RequiredProperties[str] = true;
			}
			if (!schema.AllowAdditionalProperties && !this.IsPropertyDefinied(schema, str))
			{
				this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, str), schema);
			}
			this._currentScope.CurrentPropertyName = str;
		}

		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateNotDisallowed(schema);
			string str = this._reader.Value.ToString();
			int? maximumLength = schema.MaximumLength;
			if (maximumLength.HasValue)
			{
				int length = str.Length;
				maximumLength = schema.MaximumLength;
				if (length > maximumLength.GetValueOrDefault() & maximumLength.HasValue)
				{
					this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, str, schema.MaximumLength), schema);
				}
			}
			maximumLength = schema.MinimumLength;
			if (maximumLength.HasValue)
			{
				int num = str.Length;
				maximumLength = schema.MinimumLength;
				if (num < maximumLength.GetValueOrDefault() & maximumLength.HasValue)
				{
					this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, str, schema.MinimumLength), schema);
				}
			}
			if (schema.Patterns != null)
			{
				foreach (string pattern in schema.Patterns)
				{
					if (Regex.IsMatch(str, pattern))
					{
						continue;
					}
					this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, str, pattern), schema);
				}
			}
		}

		public override int? vmethod_0()
		{
			int? nullable = this._reader.vmethod_0();
			this.ValidateCurrentToken();
			return nullable;
		}

		private void WriteToken(IList<JsonSchemaModel> schemas)
		{
			bool flag;
			foreach (JsonValidatingReader.SchemaScope jTokenWriter in this._stack)
			{
				flag = (jTokenWriter.TokenType != JTokenType.Array || !jTokenWriter.IsUniqueArray ? false : jTokenWriter.ArrayItemCount > 0);
				bool flag1 = flag;
				if (!flag)
				{
					if (!schemas.Any<JsonSchemaModel>((JsonSchemaModel s) => s.Enum != null))
					{
						continue;
					}
				}
				if (jTokenWriter.CurrentItemWriter == null)
				{
					if (JsonTokenUtils.IsEndToken(this._reader.TokenType))
					{
						continue;
					}
					jTokenWriter.CurrentItemWriter = new JTokenWriter();
				}
				jTokenWriter.CurrentItemWriter.WriteToken(this._reader, false);
				if (jTokenWriter.CurrentItemWriter.Top != 0 || this._reader.TokenType == JsonToken.PropertyName)
				{
					continue;
				}
				JToken token = jTokenWriter.CurrentItemWriter.Token;
				jTokenWriter.CurrentItemWriter = null;
				if (!flag1)
				{
					if (!schemas.Any<JsonSchemaModel>((JsonSchemaModel s) => s.Enum != null))
					{
						continue;
					}
					foreach (JsonSchemaModel schema in schemas)
					{
						if (schema.Enum == null || schema.Enum.ContainsValue<JToken>(token, JToken.EqualityComparer))
						{
							continue;
						}
						StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
						token.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
						this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, stringWriter.ToString()), schema);
					}
				}
				else
				{
					if (jTokenWriter.UniqueArrayItems.Contains<JToken>(token, JToken.EqualityComparer))
					{
						this.RaiseError("Non-unique array item at index {0}.".FormatWith(CultureInfo.InvariantCulture, jTokenWriter.ArrayItemCount - 1), jTokenWriter.Schemas.First<JsonSchemaModel>((JsonSchemaModel s) => s.UniqueItems));
					}
					jTokenWriter.UniqueArrayItems.Add(token);
				}
			}
		}

		public event Newtonsoft.Json.Schema.ValidationEventHandler ValidationEventHandler;

		private class SchemaScope
		{
			private readonly JTokenType _tokenType;

			private readonly IList<JsonSchemaModel> _schemas;

			private readonly Dictionary<string, bool> _requiredProperties;

			public int ArrayItemCount
			{
				get;
				set;
			}

			public JTokenWriter CurrentItemWriter
			{
				get;
				set;
			}

			public string CurrentPropertyName
			{
				get;
				set;
			}

			public bool IsUniqueArray
			{
				get;
			}

			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			public IList<JsonSchemaModel> Schemas
			{
				get
				{
					return this._schemas;
				}
			}

			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			public IList<JToken> UniqueArrayItems
			{
				get;
			}

			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this._tokenType = tokenType;
				this._schemas = schemas;
				this._requiredProperties = schemas.SelectMany<JsonSchemaModel, string>(new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties)).Distinct<string>().ToDictionary<string, string, bool>((string p) => p, (string p) => false);
				if (tokenType == JTokenType.Array)
				{
					if (schemas.Any<JsonSchemaModel>((JsonSchemaModel s) => s.UniqueItems))
					{
						this.IsUniqueArray = true;
						this.UniqueArrayItems = new List<JToken>();
					}
				}
			}

			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				bool properties;
				if (schema != null)
				{
					properties = schema.Properties;
				}
				else
				{
					properties = false;
				}
				if (!properties)
				{
					return Enumerable.Empty<string>();
				}
				return 
					from p in schema.Properties
					where p.Value.Required
					select p.Key;
			}
		}
	}
}