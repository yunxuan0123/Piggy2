using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace Newtonsoft.Json.Serialization
{
	public class JsonObjectContract : JsonContainerContract
	{
		internal bool ExtensionDataIsJToken;

		private bool? _hasRequiredOrDefaultValueProperties;

		private ObjectConstructor<object> _overrideCreator;

		private ObjectConstructor<object> _parameterizedCreator;

		private JsonPropertyCollection _creatorParameters;

		private Type _extensionDataValueType;

		public JsonPropertyCollection CreatorParameters
		{
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		public Newtonsoft.Json.Serialization.ExtensionDataGetter ExtensionDataGetter
		{
			get;
			set;
		}

		public Func<string, string> ExtensionDataNameResolver
		{
			get;
			set;
		}

		public Newtonsoft.Json.Serialization.ExtensionDataSetter ExtensionDataSetter
		{
			get;
			set;
		}

		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = (value == null ? false : typeof(JToken).IsAssignableFrom(value));
			}
		}

		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				DefaultValueHandling? nullable;
				DefaultValueHandling? nullable1;
				if (!this._hasRequiredOrDefaultValueProperties.HasValue)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) == Required.Default)
					{
						using (IEnumerator<JsonProperty> enumerator = this.Properties.GetEnumerator())
						{
							do
							{
								if (enumerator.MoveNext())
								{
									JsonProperty current = enumerator.Current;
									if (current.Required != Required.Default)
									{
										break;
									}
									DefaultValueHandling? defaultValueHandling = current.DefaultValueHandling;
									if (defaultValueHandling.HasValue)
									{
										nullable1 = new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault() & DefaultValueHandling.Populate);
									}
									else
									{
										nullable1 = null;
									}
									nullable = nullable1;
								}
								else
								{
									return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
								}
							}
							while (nullable.GetValueOrDefault() != DefaultValueHandling.Populate | !nullable.HasValue);
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
						}
					}
					else
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		public NullValueHandling? ItemNullValueHandling
		{
			get;
			set;
		}

		public Required? ItemRequired
		{
			get;
			set;
		}

		public Newtonsoft.Json.MemberSerialization MemberSerialization
		{
			get;
			set;
		}

		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return this._overrideCreator;
			}
			set
			{
				this._overrideCreator = value;
			}
		}

		internal ObjectConstructor<object> ParameterizedCreator
		{
			get
			{
				return this._parameterizedCreator;
			}
			set
			{
				this._parameterizedCreator = value;
			}
		}

		public JsonPropertyCollection Properties
		{
			get;
		}

		public JsonObjectContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			base(underlyingType);
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}
	}
}