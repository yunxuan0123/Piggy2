using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	public class DefaultContractResolver : IContractResolver
	{
		private readonly static IContractResolver _instance;

		private readonly static string[] BlacklistedTypeNames;

		private readonly static JsonConverter[] BuiltInConverters;

		private readonly DefaultJsonNameTable _nameTable;

		private readonly ThreadSafeStore<Type, JsonContract> _contractCache;

		[Obsolete("DefaultMembersSearchFlags is obsolete. To modify the members serialized inherit from DefaultContractResolver and override the GetSerializableMembers method instead.")]
		public BindingFlags DefaultMembersSearchFlags
		{
			get;
			set;
		}

		public bool DynamicCodeGeneration
		{
			get
			{
				return JsonTypeReflector.DynamicCodeGeneration;
			}
		}

		public bool IgnoreIsSpecifiedMembers
		{
			get;
			set;
		}

		public bool IgnoreSerializableAttribute
		{
			get;
			set;
		}

		public bool IgnoreSerializableInterface
		{
			get;
			set;
		}

		public bool IgnoreShouldSerializeMembers
		{
			get;
			set;
		}

		internal static IContractResolver Instance
		{
			get
			{
				return DefaultContractResolver._instance;
			}
		}

		public Newtonsoft.Json.Serialization.NamingStrategy NamingStrategy
		{
			get;
			set;
		}

		public bool SerializeCompilerGeneratedMembers
		{
			get;
			set;
		}

		static DefaultContractResolver()
		{
			Class6.yDnXvgqzyB5jw();
			DefaultContractResolver._instance = new DefaultContractResolver();
			DefaultContractResolver.BlacklistedTypeNames = new string[] { "System.IO.DriveInfo", "System.IO.FileInfo", "System.IO.DirectoryInfo" };
			DefaultContractResolver.BuiltInConverters = new JsonConverter[] { new EntityKeyMemberConverter(), new ExpandoObjectConverter(), new XmlNodeConverter(), new BinaryConverter(), new DataSetConverter(), new DataTableConverter(), new DiscriminatedUnionConverter(), new KeyValuePairConverter(), new BsonObjectIdConverter(), new RegexConverter() };
		}

		public DefaultContractResolver()
		{
			Class6.yDnXvgqzyB5jw();
			this._nameTable = new DefaultJsonNameTable();
			base();
			this.IgnoreSerializableAttribute = true;
			this.DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public;
			DefaultContractResolver defaultContractResolver = this;
			this._contractCache = new ThreadSafeStore<Type, JsonContract>(new Func<Type, JsonContract>(defaultContractResolver.CreateContract));
		}

		internal static bool CanConvertToString(Type type)
		{
			TypeConverter typeConverter;
			if (JsonTypeReflector.CanTypeDescriptorConvertString(type, out typeConverter))
			{
				return true;
			}
			if (!(type == typeof(Type)) && !type.IsSubclassOf(typeof(Type)))
			{
				return false;
			}
			return true;
		}

		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			this.InitializeContract(jsonArrayContract);
			ConstructorInfo attributeConstructor = this.GetAttributeConstructor(jsonArrayContract.NonNullableUnderlyingType);
			if (attributeConstructor != null)
			{
				ParameterInfo[] parameters = attributeConstructor.GetParameters();
				Type type = (jsonArrayContract.CollectionItemType != null ? typeof(IEnumerable<>).MakeGenericType(new Type[] { jsonArrayContract.CollectionItemType }) : typeof(IEnumerable));
				if (parameters.Length != 0)
				{
					if ((int)parameters.Length != 1 || !type.IsAssignableFrom(parameters[0].ParameterType))
					{
						throw new JsonException("Constructor for '{0}' must have no parameters or a single parameter that implements '{1}'.".FormatWith(CultureInfo.InvariantCulture, jsonArrayContract.UnderlyingType, type));
					}
					jsonArrayContract.HasParameterizedCreator = true;
				}
				else
				{
					jsonArrayContract.HasParameterizedCreator = false;
				}
				jsonArrayContract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(attributeConstructor);
			}
			return jsonArrayContract;
		}

		protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(constructor.DeclaringType);
			ParameterInfo[] parameterInfoArray = parameters;
			for (int i = 0; i < (int)parameterInfoArray.Length; i++)
			{
				ParameterInfo parameterInfo = parameterInfoArray[i];
				JsonProperty jsonProperty = this.MatchProperty(memberProperties, parameterInfo.Name, parameterInfo.ParameterType);
				if (jsonProperty != null || parameterInfo.Name != null)
				{
					JsonProperty jsonProperty1 = this.CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
					if (jsonProperty1 != null)
					{
						jsonPropertyCollection.AddProperty(jsonProperty1);
					}
				}
			}
			return jsonPropertyCollection;
		}

		protected virtual JsonContract CreateContract(Type objectType)
		{
			Type type = ReflectionUtils.EnsureNotByRefType(objectType);
			if (DefaultContractResolver.IsJsonPrimitiveType(type))
			{
				return this.CreatePrimitiveContract(objectType);
			}
			type = ReflectionUtils.EnsureNotNullableType(type);
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute is JsonObjectAttribute)
			{
				return this.CreateObjectContract(objectType);
			}
			if (cachedAttribute is JsonArrayAttribute)
			{
				return this.CreateArrayContract(objectType);
			}
			if (cachedAttribute is JsonDictionaryAttribute)
			{
				return this.CreateDictionaryContract(objectType);
			}
			if (type == typeof(JToken) || type.IsSubclassOf(typeof(JToken)))
			{
				return this.CreateLinqContract(objectType);
			}
			if (CollectionUtils.IsDictionaryType(type))
			{
				return this.CreateDictionaryContract(objectType);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return this.CreateArrayContract(type);
			}
			if (DefaultContractResolver.CanConvertToString(type))
			{
				return this.CreateStringContract(objectType);
			}
			if (!this.IgnoreSerializableInterface && typeof(ISerializable).IsAssignableFrom(type) && JsonTypeReflector.IsSerializable(type))
			{
				return this.CreateISerializableContract(objectType);
			}
			if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
			{
				return this.CreateDynamicContract(objectType);
			}
			if (DefaultContractResolver.IsIConvertible(type))
			{
				return this.CreatePrimitiveContract(type);
			}
			return this.CreateObjectContract(objectType);
		}

		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			Type namingStrategyType;
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			this.InitializeContract(jsonDictionaryContract);
			JsonContainerAttribute attribute = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((ICustomAttributeProvider)objectType);
			if (attribute != null)
			{
				namingStrategyType = attribute.NamingStrategyType;
			}
			else
			{
				namingStrategyType = null;
			}
			if (namingStrategyType == null)
			{
				DefaultContractResolver defaultContractResolver = this;
				jsonDictionaryContract.DictionaryKeyResolver = new Func<string, string>(defaultContractResolver.ResolveDictionaryKey);
			}
			else
			{
				Newtonsoft.Json.Serialization.NamingStrategy containerNamingStrategy = JsonTypeReflector.GetContainerNamingStrategy(attribute);
				jsonDictionaryContract.DictionaryKeyResolver = (string s) => containerNamingStrategy.GetDictionaryKey(s);
			}
			ConstructorInfo attributeConstructor = this.GetAttributeConstructor(jsonDictionaryContract.NonNullableUnderlyingType);
			if (attributeConstructor != null)
			{
				ParameterInfo[] parameters = attributeConstructor.GetParameters();
				Type type = (!(jsonDictionaryContract.DictionaryKeyType != null) || !(jsonDictionaryContract.DictionaryValueType != null) ? typeof(IDictionary) : typeof(IEnumerable<>).MakeGenericType(new Type[] { typeof(KeyValuePair<,>).MakeGenericType(new Type[] { jsonDictionaryContract.DictionaryKeyType, jsonDictionaryContract.DictionaryValueType }) }));
				if (parameters.Length != 0)
				{
					if ((int)parameters.Length != 1 || !type.IsAssignableFrom(parameters[0].ParameterType))
					{
						throw new JsonException("Constructor for '{0}' must have no parameters or a single parameter that implements '{1}'.".FormatWith(CultureInfo.InvariantCulture, jsonDictionaryContract.UnderlyingType, type));
					}
					jsonDictionaryContract.HasParameterizedCreator = true;
				}
				else
				{
					jsonDictionaryContract.HasParameterizedCreator = false;
				}
				jsonDictionaryContract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(attributeConstructor);
			}
			return jsonDictionaryContract;
		}

		protected virtual JsonDynamicContract CreateDynamicContract(Type objectType)
		{
			Type namingStrategyType;
			JsonDynamicContract jsonDynamicContract = new JsonDynamicContract(objectType);
			this.InitializeContract(jsonDynamicContract);
			JsonContainerAttribute attribute = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((ICustomAttributeProvider)objectType);
			if (attribute != null)
			{
				namingStrategyType = attribute.NamingStrategyType;
			}
			else
			{
				namingStrategyType = null;
			}
			if (namingStrategyType == null)
			{
				DefaultContractResolver defaultContractResolver = this;
				jsonDynamicContract.PropertyNameResolver = new Func<string, string>(defaultContractResolver.ResolveDictionaryKey);
			}
			else
			{
				Newtonsoft.Json.Serialization.NamingStrategy containerNamingStrategy = JsonTypeReflector.GetContainerNamingStrategy(attribute);
				jsonDynamicContract.PropertyNameResolver = (string s) => containerNamingStrategy.GetDictionaryKey(s);
			}
			jsonDynamicContract.Properties.AddRange<JsonProperty>(this.CreateProperties(objectType, MemberSerialization.OptOut));
			return jsonDynamicContract;
		}

		protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
		{
			JsonISerializableContract jsonISerializableContract = new JsonISerializableContract(objectType);
			this.InitializeContract(jsonISerializableContract);
			if (jsonISerializableContract.IsInstantiable)
			{
				ConstructorInfo constructor = jsonISerializableContract.NonNullableUnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
				if (constructor != null)
				{
					jsonISerializableContract.ISerializableCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
				}
			}
			return jsonISerializableContract;
		}

		protected virtual JsonLinqContract CreateLinqContract(Type objectType)
		{
			JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
			this.InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			IValueProvider reflectionValueProvider;
			if (!this.DynamicCodeGeneration)
			{
				reflectionValueProvider = new ReflectionValueProvider(member);
			}
			else
			{
				reflectionValueProvider = new DynamicValueProvider(member);
			}
			return reflectionValueProvider;
		}

		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			this.InitializeContract(jsonObjectContract);
			bool ignoreSerializableAttribute = this.IgnoreSerializableAttribute;
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(jsonObjectContract.NonNullableUnderlyingType, ignoreSerializableAttribute);
			jsonObjectContract.Properties.AddRange<JsonProperty>(this.CreateProperties(jsonObjectContract.NonNullableUnderlyingType, jsonObjectContract.MemberSerialization));
			Func<string, string> dictionaryKey = null;
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(jsonObjectContract.NonNullableUnderlyingType);
			if (cachedAttribute != null)
			{
				jsonObjectContract.ItemRequired = cachedAttribute._itemRequired;
				jsonObjectContract.ItemNullValueHandling = cachedAttribute._itemNullValueHandling;
				if (cachedAttribute.NamingStrategyType != null)
				{
					Newtonsoft.Json.Serialization.NamingStrategy containerNamingStrategy = JsonTypeReflector.GetContainerNamingStrategy(cachedAttribute);
					dictionaryKey = (string s) => containerNamingStrategy.GetDictionaryKey(s);
				}
			}
			if (dictionaryKey == null)
			{
				DefaultContractResolver defaultContractResolver = this;
				dictionaryKey = new Func<string, string>(defaultContractResolver.ResolveExtensionDataName);
			}
			jsonObjectContract.ExtensionDataNameResolver = dictionaryKey;
			if (jsonObjectContract.IsInstantiable)
			{
				ConstructorInfo attributeConstructor = this.GetAttributeConstructor(jsonObjectContract.NonNullableUnderlyingType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(attributeConstructor);
					jsonObjectContract.CreatorParameters.AddRange<JsonProperty>(this.CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
				else if (jsonObjectContract.MemberSerialization == MemberSerialization.Fields)
				{
					if (JsonTypeReflector.FullyTrusted)
					{
						jsonObjectContract.DefaultCreator = new Func<object>(jsonObjectContract.GetUninitializedObject);
					}
				}
				else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
				{
					ConstructorInfo parameterizedConstructor = this.GetParameterizedConstructor(jsonObjectContract.NonNullableUnderlyingType);
					if (parameterizedConstructor != null)
					{
						jsonObjectContract.ParameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(parameterizedConstructor);
						jsonObjectContract.CreatorParameters.AddRange<JsonProperty>(this.CreateConstructorParameters(parameterizedConstructor, jsonObjectContract.Properties));
					}
				}
				else if (jsonObjectContract.NonNullableUnderlyingType.IsValueType())
				{
					ConstructorInfo immutableConstructor = this.GetImmutableConstructor(jsonObjectContract.NonNullableUnderlyingType, jsonObjectContract.Properties);
					if (immutableConstructor != null)
					{
						jsonObjectContract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(immutableConstructor);
						jsonObjectContract.CreatorParameters.AddRange<JsonProperty>(this.CreateConstructorParameters(immutableConstructor, jsonObjectContract.Properties));
					}
				}
			}
			MemberInfo extensionDataMemberForType = this.GetExtensionDataMemberForType(jsonObjectContract.NonNullableUnderlyingType);
			if (extensionDataMemberForType != null)
			{
				DefaultContractResolver.SetExtensionDataDelegates(jsonObjectContract, extensionDataMemberForType);
			}
			if (Array.IndexOf<string>(DefaultContractResolver.BlacklistedTypeNames, objectType.FullName) != -1)
			{
				jsonObjectContract.OnSerializingCallbacks.Add(new SerializationCallback(DefaultContractResolver.ThrowUnableToSerializeError));
			}
			return jsonObjectContract;
		}

		protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			JsonPrimitiveContract jsonPrimitiveContract = new JsonPrimitiveContract(objectType);
			this.InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of serializable members returned.");
			}
			DefaultJsonNameTable nameTable = this.GetNameTable();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(type);
			foreach (MemberInfo serializableMember in serializableMembers)
			{
				JsonProperty jsonProperty = this.CreateProperty(serializableMember, memberSerialization);
				if (jsonProperty == null)
				{
					continue;
				}
				lock (nameTable)
				{
					jsonProperty.PropertyName = nameTable.Add(jsonProperty.PropertyName);
				}
				jsonPropertyCollection.AddProperty(jsonProperty);
			}
			return jsonPropertyCollection.OrderBy<JsonProperty, int>((JsonProperty p) => {
				int? order = p.Order;
				if (!order.HasValue)
				{
					return -1;
				}
				return order.GetValueOrDefault();
			}).ToList<JsonProperty>();
		}

		protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			bool flag;
			JsonProperty jsonProperty = new JsonProperty()
			{
				PropertyType = ReflectionUtils.GetMemberUnderlyingType(member),
				DeclaringType = member.DeclaringType,
				ValueProvider = this.CreateMemberValueProvider(member),
				AttributeProvider = new ReflectionAttributeProvider(member)
			};
			this.SetPropertySettingsFromAttributes(jsonProperty, member, member.Name, member.DeclaringType, memberSerialization, out flag);
			if (memberSerialization == MemberSerialization.Fields)
			{
				jsonProperty.Readable = true;
				jsonProperty.Writable = true;
			}
			else
			{
				jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member, flag);
				jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member, flag, jsonProperty.HasMemberAttribute);
			}
			if (!this.IgnoreShouldSerializeMembers)
			{
				jsonProperty.ShouldSerialize = this.CreateShouldSerializeTest(member);
			}
			if (!this.IgnoreIsSpecifiedMembers)
			{
				this.SetIsSpecifiedActions(jsonProperty, member, flag);
			}
			return jsonProperty;
		}

		protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
		{
			bool flag;
			JsonProperty jsonProperty = new JsonProperty()
			{
				PropertyType = parameterInfo.ParameterType,
				AttributeProvider = new ReflectionAttributeProvider(parameterInfo)
			};
			this.SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out flag);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = (jsonProperty.PropertyName != parameterInfo.Name ? jsonProperty.PropertyName : matchingMemberProperty.PropertyName);
				jsonProperty.Converter = jsonProperty.Converter ?? matchingMemberProperty.Converter;
				if (!jsonProperty._hasExplicitDefaultValue && matchingMemberProperty._hasExplicitDefaultValue)
				{
					jsonProperty.DefaultValue = matchingMemberProperty.DefaultValue;
				}
				JsonProperty jsonProperty1 = jsonProperty;
				Required? nullable = jsonProperty._required;
				jsonProperty1._required = (nullable.HasValue ? nullable : matchingMemberProperty._required);
				JsonProperty jsonProperty2 = jsonProperty;
				bool? isReference = jsonProperty.IsReference;
				jsonProperty2.IsReference = (isReference.HasValue ? isReference : matchingMemberProperty.IsReference);
				JsonProperty jsonProperty3 = jsonProperty;
				NullValueHandling? nullValueHandling = jsonProperty.NullValueHandling;
				jsonProperty3.NullValueHandling = (nullValueHandling.HasValue ? nullValueHandling : matchingMemberProperty.NullValueHandling);
				JsonProperty jsonProperty4 = jsonProperty;
				DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling;
				jsonProperty4.DefaultValueHandling = (defaultValueHandling.HasValue ? defaultValueHandling : matchingMemberProperty.DefaultValueHandling);
				JsonProperty jsonProperty5 = jsonProperty;
				ReferenceLoopHandling? referenceLoopHandling = jsonProperty.ReferenceLoopHandling;
				jsonProperty5.ReferenceLoopHandling = (referenceLoopHandling.HasValue ? referenceLoopHandling : matchingMemberProperty.ReferenceLoopHandling);
				JsonProperty jsonProperty6 = jsonProperty;
				ObjectCreationHandling? objectCreationHandling = jsonProperty.ObjectCreationHandling;
				jsonProperty6.ObjectCreationHandling = (objectCreationHandling.HasValue ? objectCreationHandling : matchingMemberProperty.ObjectCreationHandling);
				JsonProperty jsonProperty7 = jsonProperty;
				TypeNameHandling? typeNameHandling = jsonProperty.TypeNameHandling;
				jsonProperty7.TypeNameHandling = (typeNameHandling.HasValue ? typeNameHandling : matchingMemberProperty.TypeNameHandling);
			}
			return jsonProperty;
		}

		private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
		{
			MethodInfo method = member.DeclaringType.GetMethod(string.Concat("ShouldSerialize", member.Name), ReflectionUtils.EmptyTypes);
			if (method == null || method.ReturnType != typeof(bool))
			{
				return null;
			}
			MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)methodCall(o, new object[0]);
		}

		protected virtual JsonStringContract CreateStringContract(Type objectType)
		{
			JsonStringContract jsonStringContract = new JsonStringContract(objectType);
			this.InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		private static bool FilterMembers(MemberInfo member)
		{
			PropertyInfo propertyInfo = member as PropertyInfo;
			PropertyInfo propertyInfo1 = propertyInfo;
			if (propertyInfo != null)
			{
				if (ReflectionUtils.IsIndexedProperty(propertyInfo1))
				{
					return false;
				}
				return !ReflectionUtils.IsByRefLikeType(propertyInfo1.PropertyType);
			}
			FieldInfo fieldInfo = member as FieldInfo;
			FieldInfo fieldInfo1 = fieldInfo;
			if (fieldInfo == null)
			{
				return true;
			}
			return !ReflectionUtils.IsByRefLikeType(fieldInfo1.FieldType);
		}

		private ConstructorInfo GetAttributeConstructor(Type objectType)
		{
			IEnumerator<ConstructorInfo> enumerator = (
				from c in (IEnumerable<ConstructorInfo>)objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where c.IsDefined(typeof(JsonConstructorAttribute), true)
				select c).GetEnumerator();
			if (enumerator.MoveNext())
			{
				ConstructorInfo current = enumerator.Current;
				if (enumerator.MoveNext())
				{
					throw new JsonException("Multiple constructors with the JsonConstructorAttribute.");
				}
				return current;
			}
			if (objectType != typeof(Version))
			{
				return null;
			}
			return objectType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
		}

		private void GetCallbackMethodsForType(Type type, out List<SerializationCallback> onSerializing, out List<SerializationCallback> onSerialized, out List<SerializationCallback> onDeserializing, out List<SerializationCallback> onDeserialized, out List<SerializationErrorCallback> onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			foreach (Type classHierarchyForType in this.GetClassHierarchyForType(type))
			{
				MethodInfo methodInfo = null;
				MethodInfo methodInfo1 = null;
				MethodInfo methodInfo2 = null;
				MethodInfo methodInfo3 = null;
				MethodInfo methodInfo4 = null;
				bool flag = DefaultContractResolver.ShouldSkipSerializing(classHierarchyForType);
				bool flag1 = DefaultContractResolver.ShouldSkipDeserialized(classHierarchyForType);
				MethodInfo[] methods = classHierarchyForType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < (int)methods.Length; i++)
				{
					MethodInfo methodInfo5 = methods[i];
					if (!methodInfo5.ContainsGenericParameters)
					{
						Type type1 = null;
						ParameterInfo[] parameters = methodInfo5.GetParameters();
						if (!flag && DefaultContractResolver.IsValidCallback(methodInfo5, parameters, typeof(OnSerializingAttribute), methodInfo, ref type1))
						{
							onSerializing = onSerializing ?? new List<SerializationCallback>();
							onSerializing.Add(JsonContract.CreateSerializationCallback(methodInfo5));
							methodInfo = methodInfo5;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo5, parameters, typeof(OnSerializedAttribute), methodInfo1, ref type1))
						{
							onSerialized = onSerialized ?? new List<SerializationCallback>();
							onSerialized.Add(JsonContract.CreateSerializationCallback(methodInfo5));
							methodInfo1 = methodInfo5;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo5, parameters, typeof(OnDeserializingAttribute), methodInfo2, ref type1))
						{
							onDeserializing = onDeserializing ?? new List<SerializationCallback>();
							onDeserializing.Add(JsonContract.CreateSerializationCallback(methodInfo5));
							methodInfo2 = methodInfo5;
						}
						if (!flag1 && DefaultContractResolver.IsValidCallback(methodInfo5, parameters, typeof(OnDeserializedAttribute), methodInfo3, ref type1))
						{
							onDeserialized = onDeserialized ?? new List<SerializationCallback>();
							onDeserialized.Add(JsonContract.CreateSerializationCallback(methodInfo5));
							methodInfo3 = methodInfo5;
						}
						if (DefaultContractResolver.IsValidCallback(methodInfo5, parameters, typeof(OnErrorAttribute), methodInfo4, ref type1))
						{
							onError = onError ?? new List<SerializationErrorCallback>();
							onError.Add(JsonContract.CreateSerializationErrorCallback(methodInfo5));
							methodInfo4 = methodInfo5;
						}
					}
				}
			}
		}

		private List<Type> GetClassHierarchyForType(Type type)
		{
			List<Type> types = new List<Type>();
			for (Type i = type; i != null && i != typeof(object); i = i.BaseType())
			{
				types.Add(i);
			}
			types.Reverse();
			return types;
		}

		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition() || !type.ContainsGenericParameters())
			{
				return type.FullName;
			}
			return "{0}.{1}".FormatWith(CultureInfo.InvariantCulture, type.Namespace, type.Name);
		}

		private Func<object> GetDefaultCreator(Type createdType)
		{
			return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		private MemberInfo GetExtensionDataMemberForType(Type type)
		{
			return this.GetClassHierarchyForType(type).SelectMany<Type, MemberInfo>((Type baseType) => {
				List<MemberInfo> memberInfos = new List<MemberInfo>();
				memberInfos.AddRange<MemberInfo>(baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				memberInfos.AddRange<MemberInfo>(baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				return memberInfos;
			}).LastOrDefault<MemberInfo>((MemberInfo m) => {
				Type type1;
				MemberTypes memberType = m.MemberType();
				if (memberType != MemberTypes.Property && memberType != MemberTypes.Field)
				{
					return false;
				}
				if (!m.IsDefined(typeof(JsonExtensionDataAttribute), false))
				{
					return false;
				}
				if (!ReflectionUtils.CanReadMemberValue(m, true))
				{
					throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' must have a getter.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(m.DeclaringType), m.Name));
				}
				if (ReflectionUtils.ImplementsGenericDefinition(ReflectionUtils.GetMemberUnderlyingType(m), typeof(IDictionary<,>), out type1))
				{
					Type genericArguments = type1.GetGenericArguments()[0];
					Type genericArguments1 = type1.GetGenericArguments()[1];
					if (genericArguments.IsAssignableFrom(typeof(string)) && genericArguments1.IsAssignableFrom(typeof(JToken)))
					{
						return true;
					}
				}
				throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' type must implement IDictionary<string, JToken>.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(m.DeclaringType), m.Name));
			});
		}

		private ConstructorInfo GetImmutableConstructor(Type objectType, JsonPropertyCollection memberProperties)
		{
			IEnumerator<ConstructorInfo> enumerator = ((IEnumerable<ConstructorInfo>)objectType.GetConstructors()).GetEnumerator();
			if (enumerator.MoveNext())
			{
				ConstructorInfo current = enumerator.Current;
				if (!enumerator.MoveNext())
				{
					ParameterInfo[] parameters = current.GetParameters();
					if (parameters.Length != 0)
					{
						ParameterInfo[] parameterInfoArray = parameters;
						for (int i = 0; i < (int)parameterInfoArray.Length; i++)
						{
							ParameterInfo parameterInfo = parameterInfoArray[i];
							JsonProperty jsonProperty = this.MatchProperty(memberProperties, parameterInfo.Name, parameterInfo.ParameterType);
							if (jsonProperty == null || jsonProperty.Writable)
							{
								return null;
							}
						}
						return current;
					}
				}
			}
			return null;
		}

		internal virtual DefaultJsonNameTable GetNameTable()
		{
			return this._nameTable;
		}

		private ConstructorInfo GetParameterizedConstructor(Type objectType)
		{
			ConstructorInfo[] constructors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			if ((int)constructors.Length != 1)
			{
				return null;
			}
			return constructors[0];
		}

		public string GetResolvedPropertyName(string propertyName)
		{
			return this.ResolvePropertyName(propertyName);
		}

		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			Type type;
			MemberSerialization objectMemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType, this.IgnoreSerializableAttribute);
			IEnumerable<MemberInfo> memberInfos = ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where<MemberInfo>(new Func<MemberInfo, bool>(DefaultContractResolver.FilterMembers));
			List<MemberInfo> list = new List<MemberInfo>();
			if (objectMemberSerialization == MemberSerialization.Fields)
			{
				foreach (MemberInfo memberInfo in memberInfos)
				{
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					if (fieldInfo == null || fieldInfo.IsStatic)
					{
						continue;
					}
					list.Add(memberInfo);
				}
			}
			else
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
				List<MemberInfo> list1 = ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags).Where<MemberInfo>(new Func<MemberInfo, bool>(DefaultContractResolver.FilterMembers)).ToList<MemberInfo>();
				foreach (MemberInfo memberInfo1 in memberInfos)
				{
					if (!this.SerializeCompilerGeneratedMembers && memberInfo1.IsDefined(typeof(CompilerGeneratedAttribute), true))
					{
						continue;
					}
					if (list1.Contains(memberInfo1))
					{
						list.Add(memberInfo1);
					}
					else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>((ICustomAttributeProvider)memberInfo1) != null)
					{
						list.Add(memberInfo1);
					}
					else if (JsonTypeReflector.GetAttribute<JsonRequiredAttribute>((ICustomAttributeProvider)memberInfo1) != null)
					{
						list.Add(memberInfo1);
					}
					else if (dataContractAttribute == null || JsonTypeReflector.GetAttribute<DataMemberAttribute>((ICustomAttributeProvider)memberInfo1) == null)
					{
						if (objectMemberSerialization != MemberSerialization.Fields || memberInfo1.MemberType() != MemberTypes.Field)
						{
							continue;
						}
						list.Add(memberInfo1);
					}
					else
					{
						list.Add(memberInfo1);
					}
				}
				if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", false, out type))
				{
					list = list.Where<MemberInfo>(new Func<MemberInfo, bool>(this.ShouldSerializeEntityMember)).ToList<MemberInfo>();
				}
				if (typeof(Exception).IsAssignableFrom(objectType))
				{
					list = (
						from m in list
						where !string.Equals(m.Name, "TargetSite", StringComparison.Ordinal)
						select m).ToList<MemberInfo>();
				}
			}
			return list;
		}

		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(contract.NonNullableUnderlyingType);
			if (cachedAttribute == null)
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.NonNullableUnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = new bool?(true);
				}
			}
			else
			{
				contract.IsReference = cachedAttribute._isReference;
			}
			contract.Converter = this.ResolveContractConverter(contract.NonNullableUnderlyingType);
			contract.InternalConverter = JsonSerializer.GetMatchingConverter(DefaultContractResolver.BuiltInConverters, contract.NonNullableUnderlyingType);
			if (contract.IsInstantiable && (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType()))
			{
				contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = (contract.CreatedType.IsValueType() ? false : ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);
			}
			this.ResolveCallbackMethods(contract, contract.NonNullableUnderlyingType);
		}

		private static bool IsConcurrentOrObservableCollection(Type t)
		{
			if (t.IsGenericType())
			{
				string fullName = t.GetGenericTypeDefinition().FullName;
				if (fullName == "System.Collections.Concurrent.ConcurrentQueue`1" || fullName == "System.Collections.Concurrent.ConcurrentStack`1" || fullName == "System.Collections.Concurrent.ConcurrentBag`1" || fullName == "System.Collections.Concurrent.ConcurrentDictionary`2" || fullName == "System.Collections.ObjectModel.ObservableCollection`1")
				{
					return true;
				}
			}
			return false;
		}

		internal static bool IsIConvertible(Type t)
		{
			if (!typeof(IConvertible).IsAssignableFrom(t) && (!ReflectionUtils.IsNullableType(t) || !typeof(IConvertible).IsAssignableFrom(Nullable.GetUnderlyingType(t))))
			{
				return false;
			}
			return !typeof(JToken).IsAssignableFrom(t);
		}

		internal static bool IsJsonPrimitiveType(Type t)
		{
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(t);
			if (typeCode == PrimitiveTypeCode.Empty)
			{
				return false;
			}
			return typeCode != PrimitiveTypeCode.Object;
		}

		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new JsonException("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, method, currentCallback, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (prevAttributeType != null)
			{
				throw new JsonException("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, prevAttributeType, attributeType, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method));
			}
			if (method.IsVirtual)
			{
				throw new JsonException("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, method, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || (int)parameters.Length != 2 || parameters[0].ParameterType != typeof(StreamingContext) || parameters[1].ParameterType != typeof(ErrorContext))
				{
					throw new JsonException("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext), typeof(ErrorContext)));
				}
			}
			else if (parameters == null || (int)parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext)));
			}
			prevAttributeType = attributeType;
			return true;
		}

		private JsonProperty MatchProperty(JsonPropertyCollection properties, string name, Type type)
		{
			if (name == null)
			{
				return null;
			}
			JsonProperty closestMatchProperty = properties.GetClosestMatchProperty(name);
			if (closestMatchProperty != null && !(closestMatchProperty.PropertyType != type))
			{
				return closestMatchProperty;
			}
			return null;
		}

		private void ResolveCallbackMethods(JsonContract contract, Type t)
		{
			List<SerializationCallback> serializationCallbacks;
			List<SerializationCallback> serializationCallbacks1;
			List<SerializationCallback> serializationCallbacks2;
			List<SerializationCallback> serializationCallbacks3;
			List<SerializationErrorCallback> serializationErrorCallbacks;
			this.GetCallbackMethodsForType(t, out serializationCallbacks, out serializationCallbacks1, out serializationCallbacks2, out serializationCallbacks3, out serializationErrorCallbacks);
			if (serializationCallbacks != null)
			{
				contract.OnSerializingCallbacks.AddRange<SerializationCallback>(serializationCallbacks);
			}
			if (serializationCallbacks1 != null)
			{
				contract.OnSerializedCallbacks.AddRange<SerializationCallback>(serializationCallbacks1);
			}
			if (serializationCallbacks2 != null)
			{
				contract.OnDeserializingCallbacks.AddRange<SerializationCallback>(serializationCallbacks2);
			}
			if (serializationCallbacks3 != null)
			{
				contract.OnDeserializedCallbacks.AddRange<SerializationCallback>(serializationCallbacks3);
			}
			if (serializationErrorCallbacks != null)
			{
				contract.OnErrorCallbacks.AddRange<SerializationErrorCallback>(serializationErrorCallbacks);
			}
		}

		public virtual JsonContract ResolveContract(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return this._contractCache.Get(type);
		}

		protected virtual JsonConverter ResolveContractConverter(Type objectType)
		{
			return JsonTypeReflector.GetJsonConverter(objectType);
		}

		protected virtual string ResolveDictionaryKey(string dictionaryKey)
		{
			if (this.NamingStrategy == null)
			{
				return this.ResolvePropertyName(dictionaryKey);
			}
			return this.NamingStrategy.GetDictionaryKey(dictionaryKey);
		}

		protected virtual string ResolveExtensionDataName(string extensionDataName)
		{
			if (this.NamingStrategy == null)
			{
				return extensionDataName;
			}
			return this.NamingStrategy.GetExtensionDataName(extensionDataName);
		}

		protected virtual string ResolvePropertyName(string propertyName)
		{
			if (this.NamingStrategy == null)
			{
				return propertyName;
			}
			return this.NamingStrategy.GetPropertyName(propertyName, false);
		}

		private static void SetExtensionDataDelegates(JsonObjectContract contract, MemberInfo member)
		{
			Type type;
			Type type1;
			Action<object, object> action;
			MethodInfo setMethod;
			MethodInfo methodInfo;
			JsonExtensionDataAttribute attribute = ReflectionUtils.GetAttribute<JsonExtensionDataAttribute>(member);
			if (attribute == null)
			{
				return;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member);
			ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof(IDictionary<,>), out type);
			Type genericArguments = type.GetGenericArguments()[0];
			Type genericArguments1 = type.GetGenericArguments()[1];
			type1 = (!ReflectionUtils.IsGenericDefinition(memberUnderlyingType, typeof(IDictionary<,>)) ? memberUnderlyingType : typeof(Dictionary<,>).MakeGenericType(new Type[] { genericArguments, genericArguments1 }));
			Func<object, object> func = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(member);
			if (attribute.ReadData)
			{
				if (ReflectionUtils.CanSetMemberValue(member, true, false))
				{
					action = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(member);
				}
				else
				{
					action = null;
				}
				Action<object, object> action1 = action;
				Func<object> func1 = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type1);
				PropertyInfo property = memberUnderlyingType.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, null, genericArguments1, new Type[] { genericArguments }, null);
				if (property != null)
				{
					setMethod = property.GetSetMethod();
				}
				else
				{
					setMethod = null;
				}
				MethodInfo methodInfo1 = setMethod;
				if (methodInfo1 == null)
				{
					PropertyInfo propertyInfo = type.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, null, genericArguments1, new Type[] { genericArguments }, null);
					if (propertyInfo != null)
					{
						methodInfo = propertyInfo.GetSetMethod();
					}
					else
					{
						methodInfo = null;
					}
					methodInfo1 = methodInfo;
				}
				MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodInfo1);
				contract.ExtensionDataSetter = (object o, string key, object value) => {
					object cSu0024u003cu003e8_locals1 = func(o);
					if (cSu0024u003cu003e8_locals1 == null)
					{
						if (action1 == null)
						{
							throw new JsonSerializationException("Cannot set value onto extension data member '{0}'. The extension data collection is null and it cannot be set.".FormatWith(CultureInfo.InvariantCulture, member.Name));
						}
						cSu0024u003cu003e8_locals1 = func1();
						action1(o, cSu0024u003cu003e8_locals1);
					}
					methodCall(cSu0024u003cu003e8_locals1, new object[] { key, value });
				};
			}
			if (attribute.WriteData)
			{
				ConstructorInfo constructorInfo = typeof(DefaultContractResolver.EnumerableDictionaryWrapper<,>).MakeGenericType(new Type[] { genericArguments, genericArguments1 }).GetConstructors().First<ConstructorInfo>();
				ObjectConstructor<object> objectConstructor = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructorInfo);
				contract.ExtensionDataGetter = (object o) => {
					object cSu0024u003cu003e8_locals2 = func(o);
					if (cSu0024u003cu003e8_locals2 == null)
					{
						return null;
					}
					return (IEnumerable<KeyValuePair<object, object>>)objectConstructor(new object[] { cSu0024u003cu003e8_locals2 });
				};
			}
			contract.ExtensionDataValueType = genericArguments1;
		}

		private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
		{
			MemberInfo field = member.DeclaringType.GetProperty(string.Concat(member.Name, "Specified"), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				field = member.DeclaringType.GetField(string.Concat(member.Name, "Specified"), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (field == null || ReflectionUtils.GetMemberUnderlyingType(field) != typeof(bool))
			{
				return;
			}
			Func<object, object> func = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(field);
			property.GetIsSpecified = (object o) => (bool)func(o);
			if (ReflectionUtils.CanSetMemberValue(field, allowNonPublicAccess, false))
			{
				property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(field);
			}
		}

		private void SetPropertySettingsFromAttributes(JsonProperty property, object attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess)
		{
			DataMemberAttribute dataMemberAttribute;
			string propertyName;
			bool flag;
			Newtonsoft.Json.Serialization.NamingStrategy namingStrategy;
			Type namingStrategyType;
			Type type;
			int? nullable;
			DefaultValueHandling? nullable1;
			JsonConverter jsonConverter;
			MemberInfo memberInfo = attributeProvider as MemberInfo;
			if (JsonTypeReflector.GetDataContractAttribute(declaringType) == null || !(memberInfo != null))
			{
				dataMemberAttribute = null;
			}
			else
			{
				dataMemberAttribute = JsonTypeReflector.GetDataMemberAttribute(memberInfo);
			}
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
			JsonRequiredAttribute jsonRequiredAttribute = JsonTypeReflector.GetAttribute<JsonRequiredAttribute>(attributeProvider);
			if (attribute != null && attribute.PropertyName != null)
			{
				propertyName = attribute.PropertyName;
				flag = true;
			}
			else if (dataMemberAttribute == null || dataMemberAttribute.Name == null)
			{
				propertyName = name;
				flag = false;
			}
			else
			{
				propertyName = dataMemberAttribute.Name;
				flag = true;
			}
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((ICustomAttributeProvider)declaringType);
			if (attribute != null)
			{
				namingStrategyType = attribute.NamingStrategyType;
			}
			else
			{
				namingStrategyType = null;
			}
			if (namingStrategyType == null)
			{
				if (jsonContainerAttribute != null)
				{
					type = jsonContainerAttribute.NamingStrategyType;
				}
				else
				{
					type = null;
				}
				namingStrategy = (type == null ? this.NamingStrategy : JsonTypeReflector.GetContainerNamingStrategy(jsonContainerAttribute));
			}
			else
			{
				namingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(attribute.NamingStrategyType, attribute.NamingStrategyParameters);
			}
			if (namingStrategy == null)
			{
				property.PropertyName = this.ResolvePropertyName(propertyName);
			}
			else
			{
				property.PropertyName = namingStrategy.GetPropertyName(propertyName, flag);
			}
			property.UnderlyingName = name;
			bool flag1 = false;
			if (attribute == null)
			{
				property.NullValueHandling = null;
				ReferenceLoopHandling? nullable2 = null;
				property.ReferenceLoopHandling = nullable2;
				property.ObjectCreationHandling = null;
				TypeNameHandling? nullable3 = null;
				property.TypeNameHandling = nullable3;
				bool? nullable4 = null;
				property.IsReference = nullable4;
				nullable4 = null;
				property.ItemIsReference = nullable4;
				property.ItemConverter = null;
				nullable2 = null;
				property.ItemReferenceLoopHandling = nullable2;
				nullable3 = null;
				property.ItemTypeNameHandling = nullable3;
				if (dataMemberAttribute != null)
				{
					property._required = new Required?((dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default));
					JsonProperty jsonProperty = property;
					if (dataMemberAttribute.Order != -1)
					{
						nullable = new int?(dataMemberAttribute.Order);
					}
					else
					{
						nullable = null;
					}
					jsonProperty.Order = nullable;
					JsonProperty jsonProperty1 = property;
					if (!dataMemberAttribute.EmitDefaultValue)
					{
						nullable1 = new DefaultValueHandling?(DefaultValueHandling.Ignore);
					}
					else
					{
						nullable1 = null;
					}
					jsonProperty1.DefaultValueHandling = nullable1;
					flag1 = true;
				}
			}
			else
			{
				property._required = attribute._required;
				property.Order = attribute._order;
				property.DefaultValueHandling = attribute._defaultValueHandling;
				flag1 = true;
				property.NullValueHandling = attribute._nullValueHandling;
				property.ReferenceLoopHandling = attribute._referenceLoopHandling;
				property.ObjectCreationHandling = attribute._objectCreationHandling;
				property.TypeNameHandling = attribute._typeNameHandling;
				property.IsReference = attribute._isReference;
				property.ItemIsReference = attribute._itemIsReference;
				JsonProperty jsonProperty2 = property;
				if (attribute.ItemConverterType != null)
				{
					jsonConverter = JsonTypeReflector.CreateJsonConverterInstance(attribute.ItemConverterType, attribute.ItemConverterParameters);
				}
				else
				{
					jsonConverter = null;
				}
				jsonProperty2.ItemConverter = jsonConverter;
				property.ItemReferenceLoopHandling = attribute._itemReferenceLoopHandling;
				property.ItemTypeNameHandling = attribute._itemTypeNameHandling;
			}
			if (jsonRequiredAttribute != null)
			{
				property._required = new Required?(Required.Always);
				flag1 = true;
			}
			property.HasMemberAttribute = flag1;
			bool flag2 = (JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<JsonExtensionDataAttribute>(attributeProvider) != null ? true : JsonTypeReflector.IsNonSerializable(attributeProvider));
			if (memberSerialization == MemberSerialization.OptIn)
			{
				property.Ignored = (flag2 ? true : !flag1);
			}
			else
			{
				bool attribute1 = false;
				attribute1 = JsonTypeReflector.GetAttribute<IgnoreDataMemberAttribute>(attributeProvider) != null;
				property.Ignored = flag2 | attribute1;
			}
			property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider);
			DefaultValueAttribute defaultValueAttribute = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
			if (defaultValueAttribute != null)
			{
				property.DefaultValue = defaultValueAttribute.Value;
			}
			allowNonPublicAccess = false;
			if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
			{
				allowNonPublicAccess = true;
			}
			if (flag1)
			{
				allowNonPublicAccess = true;
			}
			if (memberSerialization == MemberSerialization.Fields)
			{
				allowNonPublicAccess = true;
			}
		}

		private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			PropertyInfo propertyInfo1 = propertyInfo;
			if (propertyInfo != null && propertyInfo1.PropertyType.IsGenericType() && propertyInfo1.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1")
			{
				return false;
			}
			return true;
		}

		private static bool ShouldSkipDeserialized(Type t)
		{
			if (DefaultContractResolver.IsConcurrentOrObservableCollection(t))
			{
				return true;
			}
			if (!(t.Name == "FSharpSet`1") && !(t.Name == "FSharpMap`2"))
			{
				return false;
			}
			return true;
		}

		private static bool ShouldSkipSerializing(Type t)
		{
			if (DefaultContractResolver.IsConcurrentOrObservableCollection(t))
			{
				return true;
			}
			if (!(t.Name == "FSharpSet`1") && !(t.Name == "FSharpMap`2"))
			{
				return false;
			}
			return true;
		}

		private static void ThrowUnableToSerializeError(object o, StreamingContext context)
		{
			throw new JsonSerializationException("Unable to serialize instance of '{0}'.".FormatWith(CultureInfo.InvariantCulture, o.GetType()));
		}

		internal class EnumerableDictionaryWrapper<TEnumeratorKey, TEnumeratorValue> : IEnumerable<KeyValuePair<object, object>>, IEnumerable
		{
			private readonly IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public EnumerableDictionaryWrapper(IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
			{
				return new DefaultContractResolver.EnumerableDictionaryWrapper<TEnumeratorKey, TEnumeratorValue>.<GetEnumerator>d__2(0)
				{
					<>4__this = this
				};
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}